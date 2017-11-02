using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Rate Type Model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [TableAttribute(tableName: "EPWCOMN.CNSRTTPPF")]
    public class RateType : BaseModel, IModel
    {

        #region Fields

        [FieldAttribute(fieldName: "R5TYPEID", primaryKey: true)]
        public int RateTypeID { get; set; }

        [FieldAttribute(fieldName: "R5DESCR")]
        public string Description { get; set; }

        [FieldAttribute(fieldName: "R5FIELDNM")]
        public string FieldName { get; set; }

        #endregion

    }
}
