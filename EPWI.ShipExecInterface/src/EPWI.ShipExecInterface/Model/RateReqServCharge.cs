using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Rate Request Service Charge model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [TableAttribute(tableName: "EPWCOMN.CNSRTDTPF")]
    public class RateReqServCharge : BaseModel, IModel
    {

        #region fields

        [FieldAttribute(fieldName: "R3RTRQID", primaryKey: true,store:true,parentKey: "R2RTRQID")]
        public int RateRequestID { get; set; }

        [FieldAttribute(fieldName: "R3RSVCID", store: true,parentKey: "R2RSVCID")]
        public int RateServiceID { get; set; }

        [FieldAttribute(fieldName: "R3RDTLID", store: true)]
        public int RateChargeID { get; set; }

        [FieldAttribute(fieldName: "R3TYPEID", store: true)]
        public int RateTypeID { get; set; }

        [FieldAttribute(fieldName: "R3AMT")]
        public decimal RateAmount { get; set; }

        [FieldAttribute(fieldName: "R3COST", store: true)]
        public decimal RateCost { get; set; }

        #endregion


    }
}
