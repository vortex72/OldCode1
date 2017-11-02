using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EPWI.ShipExecInterface.API
{
    /// <summary>
    /// Rate Controller
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(AppExceptionFilterAttribute))]
    public class RateController
    {
        IShipExecInterface _shipExec;
        IDataFactory _data;

        /// <summary>
        /// DI constructor
        /// </summary>
        /// <param name="ShipExecInterface"></param>
        /// <param name="DataFactory"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public RateController(IShipExecInterface ShipExecInterface,IDataFactory DataFactory)
        {
            _shipExec = ShipExecInterface;
            _data = DataFactory;
        }

        /// <summary>
        /// Sends a shipment to ship exec and populates the AS/400 tables with the results
        /// </summary>
        /// <param name="requestId">Shipment Request ID</param>
        /// <returns>true or false</returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        [HttpGet]
        public async Task<bool> Get_Rates(int requestId)
        {
            RateRequest request = _data.GetModel<RateRequest>(requestId);
            
            return await _shipExec.GetRates(request);
        }
    }
}
