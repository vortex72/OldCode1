using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using System.Data.SqlClient;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Logs messages to sql logging table
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class SQLLogger : ISQLLogger
    {
        private string connectionString = string.Empty;
        private bool logAllRequests = false;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="_settings"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public SQLLogger(IOptions<AppSettings> _settings)
        {
            connectionString = _settings.Value.SQLConnectionString;
            logAllRequests = _settings.Value.LogAllRequests;
        }

        /// <summary>
        /// Logs message to SQL Log Table
        /// </summary>
        /// <param name="log"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public void LogRequest(RequestLog log)
        {
            bool successFlag = log.ErrorMessage.Length.Equals(0);

            //do not log successful requests if turned off
            if (successFlag && !logAllRequests)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ShipExecLog_Insert";

                    cmd.Parameters.Add(new SqlParameter("@RequestType", log.RequestType));
                    cmd.Parameters.Add(new SqlParameter("@RequestID", log.RequestID));
                    cmd.Parameters.Add(new SqlParameter("@SuccessFlag", successFlag));
                    cmd.Parameters.Add(new SqlParameter("@RequestBody", log.RequestBody));
                    cmd.Parameters.Add(new SqlParameter("@ResponseBody", log.ResponseBody));
                    cmd.Parameters.Add(new SqlParameter("@ErrorMessage", log.ErrorMessage));

                    cmd.ExecuteNonQuery();
                }
            }


        }


    }

    public interface ISQLLogger
    {
        void LogRequest(RequestLog log);
        
    }
}
