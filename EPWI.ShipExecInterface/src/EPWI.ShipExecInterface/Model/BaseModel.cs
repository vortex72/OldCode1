using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Base Model, contains shared functionality
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class BaseModel :IModel
    {
        protected List<object> children { get; set; } = new List<object>();

        public IDataFactory Data { get; set; }
        public int RecordID { get; set; } = -1;
        public string PKName { get; set; }
        public string TableName { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public BaseModel()
        {
            //determine table name based on attributes
            TableAttribute attribute = (TableAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(TableAttribute));

            if (attribute != null)
                TableName = attribute.TableName;

            //determine primary key field
            var properties = this.GetType().GetProperties().ToList().Where(x => Attribute.IsDefined(x, typeof(FieldAttribute)));

            var pkProp = properties.Where(x => ((FieldAttribute)Attribute.GetCustomAttribute(x, typeof(FieldAttribute))).PrimaryKey)
                .Select(x=> (FieldAttribute)Attribute.GetCustomAttribute(x, typeof(FieldAttribute)));

            if (pkProp.Count() > 0)
                PKName = pkProp.First().FieldName;

        }

        /// <summary>
        /// Add a child to the model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public virtual T AddChild<T>()
        {
            //get model instance
            T retVal = Data.GetModel<T>();

            //get list of field properties of child
            var props = retVal.GetType().GetProperties().ToList().Where(x => Attribute.IsDefined(x, typeof(FieldAttribute))).
                Where(x => GetParentKey(x).Length > 0);

            //get list of parent field properties
            var parentProps = GetFields();

            //set value of fields that should set from parent
            foreach (PropertyInfo pi in props)
            {
                PropertyInfo ppi = parentProps.Where(x=>GetFieldName(x) == GetParentKey(pi)).FirstOrDefault();

                if (ppi != null)
                {
                    var val = ppi.GetValue(this);

                    pi.SetValue(retVal, val);
                }
            }

            //add to child collection
            children.Add(retVal);

            return retVal;
        }

        /// <summary>
        /// Save Model and all child models
        /// </summary>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public virtual void Save()
        {
            //save model
            if (RecordID > -1)
            {
                Data.Update(this);
            }
            else
            {
                Data.Insert(this);
            }

            //recursively save children
            foreach(var y in children)
            {
                BaseModel modelRef = (BaseModel)y;
                modelRef.Save();
            }
        }

        /// <summary>
        /// Returns field name for property
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public string GetFieldName(PropertyInfo pi)
        {
            return ((FieldAttribute)Attribute.GetCustomAttribute(pi, typeof(FieldAttribute))).FieldName;
        }

        /// <summary>
        /// Returns parent key from property
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        protected string GetParentKey(PropertyInfo pi)
        {
            return ((FieldAttribute)Attribute.GetCustomAttribute(pi, typeof(FieldAttribute))).ParentKey;
        }

        /// <summary>
        /// returns all field properties
        /// </summary>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public IEnumerable<PropertyInfo> GetFields()
        {
            return this.GetType().GetProperties().ToList().Where(x => Attribute.IsDefined(x, typeof(FieldAttribute)));
        }

        /// <summary>
        /// Returns properties that should be stored on a save
        /// </summary>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public IEnumerable<PropertyInfo> StoreableProperties()
        {
            return GetFields()
                .Where(y => ((FieldAttribute)Attribute.GetCustomAttribute(y, typeof(FieldAttribute))).Store);
        }
       
    }

    /// <summary>
    /// Interface for model
    /// </summary>
    public interface IModel
    {
        IDataFactory Data { get; set; }
        int RecordID { get; set; }
        string PKName { get; set; }
        string TableName { get; set; }
        string GetFieldName(PropertyInfo pi);
        IEnumerable<PropertyInfo> StoreableProperties();
    }
}
