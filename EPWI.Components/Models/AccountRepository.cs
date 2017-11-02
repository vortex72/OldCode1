using System;
using System.Collections.Generic;
using System.Linq;
using EPWI.Components.Proxies;
using log4net;

namespace EPWI.Components.Models
{
    public class AccountRepository : Repository
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(AccountRepository));

        public Statement GetAccountStatement(ICustomerData customerData, DateTime statementDate)
        {
            var proxy = StatementProxy.Instance;

            var statement = proxy.SubmitRequest(customerData, statementDate);
            //TODO: Remove
            if (statement.CustomerID != customerData.CustomerID) log.Warn($"!! Customer id in the statement is different requested: {customerData.CustomerID}, received: {statement.CustomerID}");

            statement.CompanyAddress = getCompanyAddress(customerData.CompanyCode.GetValueOrDefault(' '));

            return statement;
        }

        public Invoice GetInvoice(ICustomerData customerData, string invoiceNumber)
        {
            var proxy = InvoiceProxy.Instance;

            var invoice = proxy.SubmitRequest(customerData, invoiceNumber);
            //TODO: Remove
            if (invoice.CustomerID != customerData.CustomerID) log.Warn($"!! Customer id in the invioce is different requested: {customerData.CustomerID}, received: {invoice.CustomerID}");
            getInvoiceAddresses(invoice, customerData);

            return invoice;
        }

        public IEnumerable<Invoice> GetMultipleInvoices(ICustomerData customerData,
            IEnumerable<InvoiceSelections> invoiceSelections)
        {
            var invoices = new List<Invoice>();

            foreach (var invoiceSelection in invoiceSelections.Where(s => s.Selected))
            {
                var proxy = InvoiceProxy.Instance;
                var invoice = proxy.SubmitRequest(customerData, invoiceSelection.InvoiceNumber);
                //TODO: Remove
                if (invoice.CustomerID != customerData.CustomerID) log.Warn($"!! Customer id in the invioce is different requested: {customerData.CustomerID}, received: {invoice.CustomerID}");
                getInvoiceAddresses(invoice, customerData);

                invoices.Add(invoice);
            }

            return invoices;
        }

        public InvoiceDateSearch GetInvoiceListByDate(ICustomerData customerData, DateTime invoiceDate, string direction)
        {
            var proxy = InvoiceDateSearchProxy.Instance;
            var results = proxy.SubmitRequest(customerData, invoiceDate, direction);

            results.CustomerData = customerData;

            results.Invoices = from r in results.Invoices
                orderby r.ShipmentDate descending, r.InvoiceNumber descending
                select r;

            results.SearchDirection = direction == "N" ? InvoiceSearchDirection.After : InvoiceSearchDirection.Before;
            results.SearchDate = invoiceDate;

            return results;
        }

        public InvoicePartSearch GetInvoiceListByPartNumber(ICustomerData customerData, string partNumber)
        {
            var proxy = InvoicePartSearchProxy.Instance;
            var results = proxy.SubmitRequest(customerData, partNumber);

            var result = new InvoicePartSearch
            {
                CustomerData = customerData,
                PartNumber = partNumber,
                Invoices =
                    from r in results
                    orderby r.InvoiceDate descending, r.InvoiceNumber descending
                    select r
            };

            return result;
        }

        public AccountStatus GetAccountStatus(ICustomerData customerData)
        {
            var proxy = AccountStatusProxy.Instance;
            return proxy.SubmitRequest(customerData);
        }

        private Address getCompanyAddress(char companyCode)
        {
            string locationCode = null;

            // get the correct company address
            switch (companyCode)
            {
                case 'N':
                    locationCode = "DEN";
                    break;
                case 'S':
                    locationCode = "DAL";
                    break;
            }
            return getCompanyAddress(locationCode);
        }

        private void getInvoiceAddresses(Invoice invoice, ICustomerData customerData)
        {
            invoice.CompanyAddress = getCompanyAddress(customerData.CompanyCode.GetValueOrDefault(' '));
            invoice.CustomerWarehouseAddress = getCompanyAddress(invoice.CustomerWarehouse);
            invoice.ShippingWarehouseAddress = getCompanyAddress(invoice.ShippingWarehouse);
        }

        private Address getCompanyAddress(string locationCode)
        {
            Address address = null;

            if (!string.IsNullOrEmpty(locationCode))
            {
                var companyAddress = (from l in Db.Locations
                    where l.LocationCode == locationCode
                    select l).SingleOrDefault();

                if (companyAddress != null)
                    address = new Address
                    {
                        StreetAddress1 = companyAddress.Address1,
                        StreetAddress2 = companyAddress.Address2,
                        City = companyAddress.City,
                        State = companyAddress.State,
                        Zip = companyAddress.ZipCode,
                        Phone = companyAddress.Phone,
                        AlternatePhone = companyAddress.TollFree,
                        Fax = companyAddress.Fax
                    };
                else
                    log.WarnFormat("Error getting company address for location '{0}'", locationCode);
            }

            return address;
        }
    }
}