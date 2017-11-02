using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Rate Request model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [TableAttribute(tableName: "EPWCOMN.CNSRTRQPF")]
    public class RateRequest : BaseModel, IModel
    {

        #region Fields
        [FieldAttribute(fieldName: "R1RTRQID",primaryKey:true)]
        public int RateRequestID { get; set; }

        [FieldAttribute(fieldName: "R1REQDATE")]
        public DateTime RequestDate { get; set; }

        [FieldAttribute(fieldName: "R1SHIPPER")]
        public string ShipperCode { get; set; }

        [FieldAttribute(fieldName: "R1SHIPDATE")]
        public DateTime ShipDate { get; set; }

        [FieldAttribute(fieldName: "R1DSTADDR1")]
        public string DestAddress1 { get; set; }

        [FieldAttribute(fieldName: "R1DSTADDR2")]
        public string DestAddress2 { get; set; }

        [FieldAttribute(fieldName: "R1DSTCITY")]
        public string DestCity { get; set; }

        [FieldAttribute(fieldName: "R1DSTSTATE")]
        public string DestState { get; set; }

        [FieldAttribute(fieldName: "R1DSTZIP")]
        public string DestZip { get; set; }

        [FieldAttribute(fieldName: "R1DSTCNTRY")]
        public string DestCountryCode { get; set; }

        [FieldAttribute(fieldName: "R1VAFLAG")]
        public bool ValidateAddress { get; set; }

        [FieldAttribute(fieldName: "R1WEIGHT")]
        public decimal Weight { get; set; }

        [FieldAttribute(fieldName: "R1DIML")]
        public decimal Length { get; set; }

        [FieldAttribute(fieldName: "R1DIMW")]
        public decimal Width { get; set; }

        [FieldAttribute(fieldName: "R1DIMH")]
        public decimal Height { get; set; }

        [FieldAttribute(fieldName: "R1ORGADDR1")]
        public string OrigAddress1 { get; set; }

        [FieldAttribute(fieldName: "R1ORGADDR2")]
        public string OrigAddress2 { get; set; }

        [FieldAttribute(fieldName: "R1ORGCITY")]
        public string OrigCity { get; set; }

        [FieldAttribute(fieldName: "R1ORGSTATE")]
        public string OrigState { get; set; }

        [FieldAttribute(fieldName: "R1ORGZIP")]
        public string OrigZip { get; set; }

        [FieldAttribute(fieldName: "R1ORGCNTRY")]
        public string OrigCountry { get; set; }

        [FieldAttribute(fieldName: "R1RESFLAG", store: true)]
        public bool? ResidentialFlag { get; set; } = null;

        [FieldAttribute(fieldName: "R1COMPDATE",store:true)]
        public DateTime CompleteDate { get; set; }

        [FieldAttribute(fieldName: "R1AVFLAG", store: true)]
        public bool? AddressValid { get; set; } = null;

        [FieldAttribute(fieldName: "R1CODFLAG")]
        public bool? CODFlag { get; set; }

        #endregion

        /// <summary>
        /// Save override, removes child records before saving
        /// </summary>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public override void Save()
        {
            DeleteChildren();

            base.Save();
        }

        /// <summary>
        /// Override, sets RateServiceId in RateReqService
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

            // RateserviceId is a sequence within Rate Request 
            if (typeof(T) == typeof(RateReqService))
                ((RateReqService)(object)retVal).RateServiceID = children.Where(x => x.GetType() == typeof(RateReqService)).Count();
            
            return retVal;
        }

        /// <summary>
        /// Deletes child records
        /// </summary>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private void DeleteChildren()
        {
            //delete rate request service charge records
            string sql = string.Format("delete from EPWCOMN.CNSRTDTPF where R3RTRQID = {0}", RecordID);

            Data.DataInterface.ExecuteSQL(sql);

            //delete rate request service records
            sql = string.Format("delete from EPWCOMN.CNSRTMSPF where R2RTRQID = {0}", RecordID);

            Data.DataInterface.ExecuteSQL(sql);

        }


    }
}
