using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipExec;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Interface to Shipment Exec
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class ShipExecInterface : IShipExecInterface
    {
        string Url = string.Empty;
        IDataFactory data;
        ISQLLogger logger;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="_settings"></param>
        /// <param name="DataFactory"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public ShipExecInterface(IOptions<AppSettings> _settings, IDataFactory DataFactory, ISQLLogger SQLLogger)
        {
            Url = _settings.Value.ShipExecShipURL;
            data = DataFactory;
            logger = SQLLogger;
        }

        /// <summary>
        /// Creates a shipment with ShipExec
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public async Task<bool> CreateShipment(ShipRequest shipment)
        {
            bool retVal = false;
            string errorMessage = string.Empty;

            IwcfShipClient client = ShipClient();

            try
            {

                ShipmentRequest request = new ShipmentRequest();
                SoxDictionaryItem[] paramsList = { };

                //set consignee
                request.def_attr = new ShipExec.Package()
                {
                    consignee = new ShipExec.NameAddress()
                    {
                        contact = shipment.ReturnFlag ? shipment.OrigContact : shipment.DestContact,
                        address1 = shipment.ReturnFlag ? shipment.OrigAddress1 : shipment.DestAddress1,
                        address2 = shipment.ReturnFlag ? shipment.OrigAddress2 : shipment.DestAddress2,
                        city = shipment.ReturnFlag ? shipment.OrigCity : shipment.DestCity,
                        state_province = shipment.ReturnFlag ? shipment.OrigState : shipment.DestState,
                        postal_code = shipment.ReturnFlag ? shipment.OrigZip : shipment.DestZip,
                        country_ISO2 = shipment.ReturnFlag ? shipment.OrigCountryCode : shipment.DestCountryCode

                    },
                    shipper = shipment.ShipperCode,
                    shipdate = shipment.ShipmentDate.ToShortDateString(),
                    service = shipment.Service
                };

                //validate address
                if (shipment.ValidateAddress)
                {
                    var addrResponse = await ValidateAddress(request.def_attr.consignee);

                    shipment.AddressIsValid = addrResponse[0].addressIsValid;
                    shipment.ResidentialFlag = addrResponse[0].residential;
                }


                //set return specific fields
                if (shipment.ReturnFlag)
                {
                    request.def_attr.return_address_method = shipment.ReturnDeliveryMethod;
                    request.def_attr.return_delivery = true;
                    request.def_attr.return_delivery_address_email = shipment.ReturnEmail;

                }

                //set origin address fields
                if (shipment.OrigAddress1.Length > 0 || shipment.ReturnFlag)
                {
                    request.def_attr.origin_address = new ShipExec.NameAddress()
                    {
                        contact = shipment.ReturnFlag ? shipment.DestContact : shipment.OrigContact,
                        address1 = shipment.ReturnFlag ? shipment.DestAddress1 : shipment.OrigAddress1,
                        address2 = shipment.ReturnFlag ? shipment.DestAddress2 : shipment.OrigAddress2,
                        city = shipment.ReturnFlag ? shipment.OrigCity : shipment.OrigCity,
                        state_province = shipment.ReturnFlag ? shipment.DestState : shipment.OrigState,
                        postal_code = shipment.ReturnFlag ? shipment.DestZip : shipment.OrigZip,
                        country_ISO2 = shipment.ReturnFlag ? shipment.DestCountryCode : shipment.OrigCountryCode
                    };
                }

                

                //shipment comments
                request.def_attr.misc_reference_1 = shipment.Comment1;
                request.def_attr.misc_reference_2 = shipment.Comment2;
                request.def_attr.misc_reference_3 = shipment.Comment3;
                request.def_attr.misc_reference_4 = shipment.Comment4;

                //bill to address fields
                if (shipment.BillToAddress1.Length > 0)
                {
                    request.def_attr.third_party_billing_address = new ShipExec.NameAddress()
                    {
                        contact = shipment.BillToContact,
                        address1 = shipment.BillToAddress1,
                        address2 = shipment.BillToAddress2,
                        city = shipment.BillToCity,
                        state_province = shipment.BillToState,
                        postal_code = shipment.BillToZip,
                        country_ISO2 = shipment.BillToCountryCode

                    };
                    request.def_attr.third_party_billing = true;
                    request.def_attr.third_party_billing_account = shipment.BillToAccount;
                }
                else
                    request.def_attr.consignee_account = shipment.BillToAccount;

                //create package
                request.packages = new ShipExec.PackageRequest[] {
                     new ShipExec.PackageRequest(){
                         weight = shipment.DimWeight > 0 ? shipment.DimWeight : shipment.Weight,
                         dimension = string.Format("{0}X{1}X{2}",shipment.Length, shipment.Width,shipment.Height),
                         description = shipment.ReturnDescription,
                         dimensional_weight_rated = shipment.DimWeight > 0
                    
                        }
                    };

                //set COD fields
                if (shipment.CODFlag)
                {
                    request.packages[0].cod_amount = shipment.CODAmt;
                    request.packages[0].cod_payment_method = shipment.CODPaymentMethod;
                }


                //create object
                ShipExec.ShipRequest ship = new ShipExec.ShipRequest();
                ship.request = request;
                ship.Params = paramsList;

                //send request
                var response = await client.ShipAsync(ship);

                //process response
                if (response.ShipResult.def_attr.error_code.Equals(0))
                {
                    shipment.TotalCharges = response.ShipResult.def_attr.total;
                    shipment.TrackingNo = response.ShipResult.packages[0].tracking_number;
                    shipment.CompletedDate = DateTime.Now;

                    shipment.Save();

                    retVal = true;
                }
                else
                    throw new Exception("Error returned by ShipExec API - " + response.ShipResult.def_attr.error_message);
            }
            catch(Exception ex)
            {
                errorMessage = ex.ToString();
                retVal = false;
            }

            RequestLog log = new RequestLog()
            {
                ErrorMessage = errorMessage,
                RequestID = shipment.RecordID,
                RequestType = RequestType.ShipRequest,
                RequestBody = GetRequest(client),
                ResponseBody = GetResponse(client)
            };

            logger.LogRequest(log);

            return retVal;

        }


        /// <summary>
        /// Retrieves rates from Ship Exec for request
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public async Task<bool> GetRates(RateRequest rate)
        {
            bool retVal = false;
            string errorMessage = string.Empty;

            IwcfShipClient client = ShipClient();

            try
            {
                //get list of possible rate types
                List<RateType> rateTypes = data.GetList<RateType>();

                //retrieve services to use for shipper
                var carriers = await client.GetCarriersbyShippersAsync(new string[] { rate.ShipperCode });

                var serv = await client.GetServicesbyCarriersAsync(carriers.Select(x => x.key.ToString()).ToArray());

                string[] services = serv.Select(x => x.key.ToString()).ToArray();

                ShipmentRequest request = new ShipmentRequest();
                SoxDictionaryItem[] paramsList = { };

                //consignee
                request.def_attr = new ShipExec.Package()
                {
                    consignee = new ShipExec.NameAddress()
                    {
                        address1 = rate.DestAddress1,
                        address2 = rate.DestAddress2,
                        city = rate.DestCity,
                        state_province = rate.DestState,
                        postal_code = rate.DestZip,
                        country_ISO2 = rate.DestCountryCode,
                        validateAddress = rate.ValidateAddress

                    },
                    shipper = rate.ShipperCode,
                    shipdate = rate.ShipDate.ToShortDateString()
                };

                //validate address
                if (rate.ValidateAddress)
                {
                    var response = await ValidateAddress(request.def_attr.consignee);

                    if (response.Count() > 0)
                    {
                        rate.AddressValid = response[0].isResComValid;
                        rate.ResidentialFlag = response[0].residential;
                    }
                    else
                    {
                        rate.AddressValid = false;
                    }

                    if (!rate.AddressValid.Value)
                        rate.ResidentialFlag = null;
                    else
                        request.def_attr.consignee.residential = rate.ResidentialFlag.Value;

                }
                else if (rate.ResidentialFlag.HasValue && rate.ResidentialFlag.Value)
                    request.def_attr.consignee.residential = true;


                //package
                request.packages = new ShipExec.PackageRequest[] {
                 new ShipExec.PackageRequest(){
                     weight = rate.Weight,
                     dimension = string.Format("{0}X{1}X{2}",rate.Length, rate.Width,rate.Height)
                 }
                };

                if (rate.CODFlag.HasValue && rate.CODFlag.Value )
                {
                    request.packages[0].cod_amount = 5;
                    request.packages[0].cod_payment_method = 2;
                }

                //send request to ShipExec
                List<ShipmentResponse> responses = await GetRatesInternal(client, request, services, paramsList);

                //loop through responses, will return one for each service requested
                if (responses.Count > 0)
                {
                    rate.CompleteDate = DateTime.Now;

                    //only process ones that returned a rate
                    var rates = responses.Where(x => x.def_attr.total > 0).ToList();

                    if (rates.Count.Equals(0))
                        throw new Exception("All rates returned are 0 - " + responses[0].def_attr.error_message);

                    //loop through rates
                    foreach (var r in rates)
                    {
                        RateReqService req = rate.AddChild<RateReqService>();
                        req.ServiceCode = r.def_attr.service;
                        req.TotalCost = r.def_attr.total;
                        req.TransitDays = r.def_attr.time_in_transit_days;
                        req.TotalAmount = r.def_attr.base_charge + r.def_attr.special;

                        if (r.packages[0].rated_weight > decimal.Zero)
                            req.DimWeight = r.packages[0].rated_weight;

                        //retrieve any special charges that are greater than 0
                        foreach (RateType rt in rateTypes)
                        {
                            PropertyInfo pi = r.packages[0].GetType().GetProperty(rt.FieldName);
                            
                            if (pi != null && pi.PropertyType == typeof(decimal))
                            {
                                decimal c = Convert.ToDecimal(pi.GetValue(r.packages[0]));

                                if (c != 0)
                                {
                                    RateReqServCharge chrg = req.AddChild<RateReqServCharge>();
                                    chrg.RateTypeID = rt.RateTypeID;
                                    chrg.RateCost = c;
                                }
                            }
                        }

                    }

                    //save rate request
                    rate.Save();
                    retVal = true;

                }
                else
                    throw new Exception("No rates returned by Ship Exec API");
            }
            catch(Exception ex)
            {
                errorMessage = ex.ToString();
                retVal = false;
            }

            RequestLog log = new RequestLog()
            {
                ErrorMessage = errorMessage,
                RequestID = rate.RecordID,
                RequestType = RequestType.RateRequest,
                RequestBody = GetRequest(client),
                ResponseBody = GetResponse(client)
            };

            logger.LogRequest(log);

            return retVal;
        }

        /// <summary>
        /// Validates Address with Ship Exec
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private async Task<NameAddressValidationCandidate[]> ValidateAddress(NameAddress address)
        {

            IwcfShipClient client = ShipClient();

            address.country_symbol = await ConvertToCountrySymbol(address.country_ISO2);

            var response = await client.GetNameAddressValidationCandidatesAsync(address,true);

            return response;
           
        }

        /// <summary>
        /// Gets Country Symbol for ISO2 Country Code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private async Task<string> ConvertToCountrySymbol(string countryCode)
        {
            string retVal = "UNITED_STATES";

            IwcfShipClient client = ShipClient();

            var countries = await client.GetCountriesAsync();

            var country = countries.Where(x => x.ISO2 == countryCode);

            if (country.Count() > 0)
                retVal = country.First().Symbol;

            return retVal;
        }

        /// <summary>
        /// Returns ShipExec API client
        /// </summary>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private IwcfShipClient ShipClient()
        {
            IwcfShipClient client = new IwcfShipClient(IwcfShipClient.EndpointConfiguration.wcfShip, Url);
            client.Endpoint.EndpointBehaviors.Add(new InspectorBehavior());

            return client;
        }

        /// <summary>
        /// Performs rate request with ShipExec
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="services"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private async Task<List<ShipmentResponse>> GetRatesInternal(IwcfShipClient client, ShipmentRequest request,string[] services,SoxDictionaryItem[] parameters)
        {
            IwcfShip shipClient = (IwcfShip)client;

            ShipExec.RateServicesRequest inValue = new RateServicesRequest();

            inValue.request = request;
            inValue.services = services;
            inValue.Params = parameters;
            ShipExec.RateServicesResponse retVal = await shipClient.RateServicesAsync(inValue);
            parameters = retVal.Params;

            return retVal.RateServicesResult.ToList();
        }

        /// <summary>
        /// Returns response body for Request, used for logging
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private string GetResponse(IwcfShipClient client)
        {
            string retVal = string.Empty;

            var endpoint = client.Endpoint.EndpointBehaviors.Where(x => x.GetType() == typeof(InspectorBehavior)).FirstOrDefault();

            if (endpoint != null && ((InspectorBehavior)endpoint).Inspector != null)
                retVal = ((InspectorBehavior)endpoint).Inspector.ResponseBody;

            return retVal;
        }

        /// <summary>
        /// Returns Request body for request, used for logging
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private string GetRequest(IwcfShipClient client)
        {
            string retVal = string.Empty;

            var endpoint = client.Endpoint.EndpointBehaviors.Where(x => x.GetType() == typeof(InspectorBehavior)).FirstOrDefault();

            if (endpoint != null && ((InspectorBehavior)endpoint).Inspector != null)
                retVal = ((InspectorBehavior)endpoint).Inspector.RequestBody;

            return retVal;
        }

    }

    /// <summary>
    /// ShipExec Interface
    /// </summary>
    public interface IShipExecInterface
    {
        Task<bool> GetRates(RateRequest rate);
        Task<bool> CreateShipment(ShipRequest shipment);
    }
}
