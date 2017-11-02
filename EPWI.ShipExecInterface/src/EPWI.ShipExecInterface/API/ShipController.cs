using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EPWI.ShipExecInterface.API
{
    /// <summary>
    /// Shipment Controller
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(AppExceptionFilterAttribute))]
    public class ShipController
    {
        IShipExecInterface _shipExec;
        IDataFactory _data;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="ShipExecInterface"></param>
        /// <param name="DataFactory"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public ShipController(IShipExecInterface ShipExecInterface, IDataFactory DataFactory)
        {
            _shipExec = ShipExecInterface;
            _data = DataFactory;
        }

        /// <summary>
        /// Creates a shipment through Ship Exec
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        [HttpGet]
        public async Task<bool> Create_Shipment(int requestId)
        {
            ShipRequest request = _data.GetModel<ShipRequest>(requestId);

            return await _shipExec.CreateShipment(request);

        }

    }
}
