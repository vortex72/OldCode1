using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Text;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Handles interactions between the models and the database layer
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class DataFactory : IDataFactory
    {
        #region private methods

        /// <summary>
        /// Loads a model for the given ID
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="recordId">ID of record to load</param>
        /// <param name="model">Model to load data into</param>
        /// <returns>Populated model</returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private T Load<T>(int recordId, T model)
        {
            IModel modelRef = (IModel)model;
            modelRef.RecordID = recordId;

            string sql = string.Format("select * from {0} where {1} = {2}", modelRef.TableName, modelRef.PKName, recordId);

            DataRow dr = _interface.GetRecord(sql);

            return LoadModel(model, dr);
        }

        /// <summary>
        /// Loads data into a model based on a data row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        private T LoadModel<T>(T model, DataRow dr)
        {
            //get list of properties that are fields
            var properties = typeof(T).GetProperties().ToList().Where(x => Attribute.IsDefined(x, typeof(FieldAttribute)));

            IModel modelRef = (IModel)model;

            foreach (DataColumn dc in dr.Table.Columns)
            {
                var prop = properties.Where(x => modelRef.GetFieldName(x) == dc.ColumnName);

                if (prop.Count() > 0)
                {
                    PropertyInfo pi = prop.First();
                    pi.SetValue(model, _interface.TransformValue(pi.PropertyType, dr[dc]));
                }
            }

            return model;
        }


        #endregion

        #region public methods


        private IDataInterface _interface;
        public IDataInterface DataInterface { get { return _interface; } }

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="DataInterface"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public DataFactory(IDataInterface DataInterface)
        {
            _interface = DataInterface;
        }

        /// <summary>
        /// Creates and returns an instance of a model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public T GetModel<T>()
        {
            var retVal = Activator.CreateInstance<T>();
            ((IModel)retVal).Data = this;

            return retVal;
        }

        /// <summary>
        /// Creates an instance of a model and populates it from the supplied recordId
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recordId"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public T GetModel<T>(int recordId)
        {
            T retVal = GetModel<T>();

            return Load<T>(recordId, retVal);

        }

        /// <summary>
        /// Returns all records of a model type as a list of models
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public List<T> GetList<T>()
        {
            //get instance of model to retrieve table name from
            T m = GetModel<T>();
            IModel modelRef = (IModel)m; 

            List<T> retVal = new List<T>();

            string sql = string.Format("Select * From {0}", modelRef.TableName);

            DataTable dt = _interface.GetTable(sql);

            foreach(DataRow dr in dt.Rows)
            {
                T model = LoadModel(GetModel<T>(),dr);
                retVal.Add(model);
            }

            return retVal;
        }

        /// <summary>
        /// Updates a model in the database
        /// </summary>
        /// <param name="model"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public void Update(BaseModel model)
        {
            IModel modelRef = (IModel)model;

            StringBuilder valSql = new StringBuilder();

            //get all properties that should be saved
            var properties = modelRef.StoreableProperties();

            //build SQL string based on properties
            foreach (var pi in properties)
            {
                if (valSql.Length > 0)
                {
                    valSql.Append(",");
                }

                string val = _interface.GetStorableValue(pi.PropertyType, pi.GetValue(model));

                string charStr = val.Equals("null") ? "" : _interface.GetCharStr(pi.PropertyType);
               

                valSql.AppendFormat("{2}={0}{1}{0}", charStr, val, modelRef.GetFieldName(pi));

            }

            string sql = string.Format("Update {0} set {1} where {2}={3}", modelRef.TableName, valSql,modelRef.PKName,modelRef.RecordID);

            //update database
            _interface.ExecuteSQL(sql);
        }

        /// <summary>
        /// Creates a new record in database from a model
        /// </summary>
        /// <param name="model"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public void Insert(BaseModel model)
        {
            StringBuilder fieldSql = new StringBuilder();
            StringBuilder valSql = new StringBuilder();
            IModel modelRef = (IModel)model;

            //get list of storable properties
            var properties = modelRef.StoreableProperties();

            //build sql statement based on property values of model
            foreach(var pi in properties)
            {
                if (fieldSql.Length > 0)
                {
                    fieldSql.Append(",");
                    valSql.Append(",");
                }

                fieldSql.Append(modelRef.GetFieldName(pi));

                string val = _interface.GetStorableValue(pi.PropertyType, pi.GetValue(model));
                string charStr = val.Equals("null") ? "" : _interface.GetCharStr(pi.PropertyType);

                valSql.AppendFormat("{0}{1}{0}",charStr, val);

            }

            string sql = string.Format("Insert into {0} ({1}) values ({2})", ((IModel)model).TableName, fieldSql, valSql);

            //update database
            _interface.ExecuteSQL(sql);
        }

       

        #endregion

    }

   /// <summary>
   /// Interface for Data Factory
   /// </summary>
   public interface IDataFactory
    {
        T GetModel<T>();
        void Insert(BaseModel model);
        void Update(BaseModel model);
        T GetModel<T>(int recordId);
        IDataInterface DataInterface { get; }
        List<T> GetList<T>();
    }

    
    
}
