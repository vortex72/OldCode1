using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// TableAttribute, used to apply table attributes to model
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        private string _tableName = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public TableAttribute(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Table Name
        /// </summary>
        public virtual string TableName
        {
            get { return _tableName; }
        }

    }

    /// <summary>
    /// Applies field attributes to model properties
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute: Attribute
    {
        private string _fieldName = string.Empty;
        private bool _primaryKey = false;
        private bool _store = false;
        private string _parentKey = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="primaryKey"></param>
        /// <param name="store"></param>
        /// <param name="parentKey"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public FieldAttribute(string fieldName, bool primaryKey=false,bool store=false,string parentKey = "")
        {
            _fieldName = fieldName;
            _primaryKey = primaryKey;
            _store = store;
            _parentKey = parentKey;
        }

        /// <summary>
        /// name of field in parent model this should be set from
        /// </summary>
        public virtual string ParentKey
        {
            get { return _parentKey;  }
        }

        /// <summary>
        /// should field be stored when model is saved
        /// </summary>
        public virtual bool Store
        {
            get { return _store; }
        }

        /// <summary>
        /// Field name in database
        /// </summary>
        public virtual string FieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// field is a primary key field
        /// </summary>
        public virtual bool PrimaryKey
        {
            get { return _primaryKey; }
        }
    }
}
