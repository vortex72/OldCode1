using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Rate Request Service Model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [TableAttribute(tableName: "EPWCOMN.CNSRTMSPF")]
    public class RateReqService : BaseModel, IModel
    {

        #region fields

        [FieldAttribute(fieldName: "R2RSVCID", primaryKey: true,store:true)]
        public int RateServiceID { get; set; }

        [FieldAttribute(fieldName: "R2RTRQID", parentKey:"R1RTRQID",store:true)]
        public int RateRequestID { get; set; }

        [FieldAttribute(fieldName: "R2SERVICE",store:true)]
        public string ServiceCode { get; set; }

        [FieldAttribute(fieldName: "R2TOTALAMT",store:true)]
        public decimal TotalAmount { get; set; }

        [FieldAttribute(fieldName: "R2TRANDAYS",store:true)]
        public int TransitDays { get; set; }

        [FieldAttribute(fieldName: "R2TOTALCST", store: true)]
        public decimal TotalCost { get; set; }

        [FieldAttribute(fieldName: "R2DIMWT", store: true)]
        public decimal DimWeight { get; set; }

        #endregion


        /// <summary>
        /// Override, sets RateChargeID in RateReqServCharge child
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public override T AddChild<T>()
        {
            T retVal = base.AddChild<T>();

            //RateChargeID is a sequence within the Rate Request Service
            if (typeof(T) == typeof(RateReqServCharge))
                ((RateReqServCharge)(object)retVal).RateChargeID = children.Where(x => x.GetType() == typeof(RateReqServCharge)).Count();

            return retVal;
        }



    }
}
