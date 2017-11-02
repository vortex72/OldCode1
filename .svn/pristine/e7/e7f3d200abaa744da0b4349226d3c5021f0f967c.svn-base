using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Shipment Request Model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [TableAttribute(tableName: "EPWCOMN.CNSSHRQPF")]
    public class ShipRequest : BaseModel, IModel
    {

        #region Fields

        [FieldAttribute(fieldName: "R4SHRQID", primaryKey: true)]
        public int ShipRequestID { get; set; }

        [FieldAttribute(fieldName: "R4REQDATE")]
        public DateTime RequestDate { get; set; }

        [FieldAttribute(fieldName: "R4SHIPPER")]
        public string ShipperCode { get; set; }

        [FieldAttribute(fieldName: "R4SHIPDATE")]
        public DateTime ShipmentDate { get; set; }

        [FieldAttribute(fieldName: "R4SERVICE")]
        public string Service { get; set; }

        [FieldAttribute(fieldName: "R4DSTNAME")]
        public string DestContact { get; set; }

        [FieldAttribute(fieldName: "R4DSTADDR1")]
        public string DestAddress1 { get; set; }

        [FieldAttribute(fieldName: "R4DSTADDR2")]
        public string DestAddress2 { get; set; }

        [FieldAttribute(fieldName: "R4DSTCITY")]
        public string DestCity { get; set; }

        [FieldAttribute(fieldName: "R4DSTSTATE")]
        public string DestState { get; set; }

        [FieldAttribute(fieldName: "R4DSTZIP")]
        public string DestZip { get; set; }

        [FieldAttribute(fieldName: "R4DSTCNTRY")]
        public string DestCountryCode { get; set; }

        [FieldAttribute(fieldName: "R4DSTPHONE")]
        public string DestPhone { get; set; }

        [FieldAttribute(fieldName: "R4DIMWT")]
        public decimal DimWeight { get; set; }

        [FieldAttribute(fieldName: "R4WEIGHT")]
        public decimal Weight { get; set; }

        [FieldAttribute(fieldName: "R4DIML")]
        public decimal Length { get; set; }

        [FieldAttribute(fieldName: "R4DIMW")]
        public decimal Width { get; set; }

        [FieldAttribute(fieldName: "R4DIMH")]
        public decimal Height { get; set; }

        [FieldAttribute(fieldName: "R4VAFLAG")]
        public bool ValidateAddress { get; set; }

        [FieldAttribute(fieldName: "R4ORGNAME")]
        public string OrigContact { get; set; }

        [FieldAttribute(fieldName: "R4ORGADDR1")]
        public string OrigAddress1 { get; set; }

        [FieldAttribute(fieldName: "R4ORGADDR2")]
        public string OrigAddress2 { get; set; }

        [FieldAttribute(fieldName: "R4ORGCITY")]
        public string OrigCity { get; set; }

        [FieldAttribute(fieldName: "R4ORGSTATE")]
        public string OrigState { get; set; }

        [FieldAttribute(fieldName: "R4ORGZIP")]
        public string OrigZip { get; set; }

        [FieldAttribute(fieldName: "R4ORGCNTRY")]
        public string OrigCountryCode { get; set; }

        [FieldAttribute(fieldName: "R4ORGPHONE")]
        public string OrigPhone { get; set; }

        [FieldAttribute(fieldName: "R4BTONAME")]
        public string BillToContact { get; set; }

        [FieldAttribute(fieldName: "R4BTOADDR1")]
        public string BillToAddress1 { get; set; }

        [FieldAttribute(fieldName: "R4BTOADDR2")]
        public string BillToAddress2 { get; set; }

        [FieldAttribute(fieldName: "R4BTOCITY")]
        public string BillToCity { get; set; }

        [FieldAttribute(fieldName: "R4BTOSTATE")]
        public string BillToState { get; set; }

        [FieldAttribute(fieldName: "R4BTOZIP")]
        public string BillToZip { get; set; }

        [FieldAttribute(fieldName: "R4BTOCNTRY")]
        public string BillToCountryCode { get; set; }

        [FieldAttribute(fieldName: "R4BBILLTO")]
        public string BillToAccount { get; set; }

        [FieldAttribute(fieldName: "R4CODAMT")]
        public decimal CODAmt { get; set; }

        [FieldAttribute(fieldName: "R4CODFLAG")]
        public bool CODFlag { get; set; }

        [FieldAttribute(fieldName: "R4CODPYMTH")]
        public int CODPaymentMethod { get; set; }

        [FieldAttribute(fieldName: "R4RESFLAG")]
        public bool ResidentialFlag { get; set; }

        [FieldAttribute(fieldName: "R4COMMENT1")]
        public string Comment1 { get; set; }

        [FieldAttribute(fieldName: "R4COMMENT2")]
        public string Comment2 { get; set; }

        [FieldAttribute(fieldName: "R4COMMENT3")]
        public string Comment3 { get; set; }

        [FieldAttribute(fieldName: "R4COMMENT4")]
        public string Comment4 { get; set; }

        [FieldAttribute(fieldName: "R4ETEMAIL")]
        public string ReturnEmail { get; set; }

        [FieldAttribute(fieldName: "R4DELMTHD")]
        public int ReturnDeliveryMethod { get; set; }

        [FieldAttribute(fieldName: "R4RETFLAG")]
        public bool ReturnFlag { get; set; }

        [FieldAttribute(fieldName: "R4RETDESC")]
        public string ReturnDescription { get; set; }

        [FieldAttribute(fieldName: "R4TRACKNO",store:true)]
        public string TrackingNo { get; set; }

        [FieldAttribute(fieldName: "R4AVFLAG", store: true)]
        public bool? AddressIsValid { get; set; }

        [FieldAttribute(fieldName: "R4TOTALAMT", store: true)]
        public decimal TotalCharges { get; set; }

        [FieldAttribute(fieldName: "R4COMPDATE", store: true)]
        public DateTime CompletedDate { get; set; }

       

        #endregion
    }
}
