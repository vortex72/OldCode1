using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.Odbc;
using Microsoft.Extensions.Options;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Handles interaction with Database
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class DataInterface : IDataInterface
    {
        private string connectionString;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="_settings"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public DataInterface(IOptions<AppSettings> _settings)
        {
            connectionString = _settings.Value.ConnectionString;
        }

        /// <summary>
        /// Returns datatable based on SQL Query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public DataTable GetTable(string sql)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    OdbcDataAdapter adapt = new OdbcDataAdapter();
                    adapt.SelectCommand = cmd;

                    DataSet ds = new DataSet();

                    adapt.Fill(ds);

                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Returns first record returned by data row, or null if result set is empty
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public DataRow GetRecord(string sql)
        {
            DataTable dt = GetTable(sql);

            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// Executes a SQL statement
        /// </summary>
        /// <param name="sql"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public void ExecuteSQL(string sql)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

            }
        }

        /// <summary>
        /// Returns character string to use based on field type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public string GetCharStr(Type propertyType)
        {
            string retVal = string.Empty;

            if (propertyType == typeof(string) || propertyType == typeof(bool) || propertyType == typeof(bool?))
                retVal = "'";

            return retVal;
        }

        /// <summary>
        /// Returns string representation of value to be added to insert or update SQL statement. Performs transformation to set to database specific type.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public string GetStorableValue(Type propertyType, object value)
        {
            string retVal = string.Empty;

            if (value != null)
            {
                if (propertyType == typeof(bool))
                {
                    retVal = (bool)value ? "Y" : "N"; //use "Y" or "N" for bool
                }
                else if (propertyType == typeof(bool?))
                {
                    bool? x = (bool?)value;

                    //needs to use empty string if null and a boolean
                    if (x.HasValue)
                    {
                        retVal = x.Value ? "Y" : "N";
                    }
                    
                }
                else if (propertyType == typeof(DateTime))
                {
                    retVal = ((DateTime)value).ToString("yyyyddMMHHmmss");
                }
                else
                    retVal = value.ToString();
            }
            else
                retVal = propertyType == typeof(string) || propertyType == typeof(bool?) ? string.Empty : "null";

            return retVal;

        }

        /// <summary>
        /// Transforms value from Database to Model Type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public object TransformValue(Type propertyType,object value)
        {
            object retVal = value;

            //int
            if (propertyType == typeof(Int32))
            {
                retVal = Convert.ToInt32(value);
            }

            //bool
            if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                retVal = false;

                if (value != null && value.ToString().Equals("Y"))
                    retVal = true;
                else if (value == null || value.ToString().Trim().Length.Equals(0) && propertyType == typeof(bool?))
                    retVal = null;

            }

            //string
            if (propertyType == typeof(string))
            {
                retVal = value.ToString().Trim();
            }

            //date time
            if (propertyType == typeof(DateTime))
            {
                retVal = DateTime.MinValue;

                if (value != null)
                {
                    string dateStr = value.ToString();
                    DateTime outval;

                    if (dateStr.Length.Equals(14))
                    {
                        dateStr = string.Format("{0}-{1}-{2} {3}:{4}:{5}",
                            dateStr.Substring(4, 2),
                            dateStr.Substring(6, 2),
                            dateStr.Substring(0, 4),
                            dateStr.Substring(8, 2),
                            dateStr.Substring(10, 2),
                            dateStr.Substring(12, 2)
                            );
                    }
                    else if (dateStr.Length > 7)
                    {
                        dateStr = string.Format("{0}-{1}-{2}",
                            dateStr.Substring(4, 2),
                            dateStr.Substring(6, 2),
                            dateStr.Substring(0, 4)
                            );
                    }

                    if (DateTime.TryParse(dateStr, out outval))
                        retVal = outval;
                    else
                        retVal = DateTime.MinValue;
                }

            }

            return retVal;
        }
    }

    /// <summary>
    /// Interface definition
    /// </summary>
    public interface IDataInterface
    {
        DataRow GetRecord(string sql);
        void ExecuteSQL(string sql);
        string GetStorableValue(Type propertyType, object value);
        object TransformValue(Type propertyType, object value);
        string GetCharStr(Type propertyType);
        DataTable GetTable(string sql);
    }
}
