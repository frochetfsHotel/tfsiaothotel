namespace SuccessHotelierHub
{
    #region 'LIBRARY IMPORTS'
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Xml;
    #endregion

    public static class DALHelper
    {
        #region 'GLOBALS'
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        #endregion

        #region 'REGULER OPERATIONS'
        public static DataSet GetRecordSetT(string SPName, params SqlParameter[] SqlPrms)
        {
            var conn = new SqlConnection(connectionString);

            DataSet ds = SqlHelper.ExecuteDataset(
                conn,
                CommandType.StoredProcedure,
                SPName,
                SqlPrms
                );

            return ds;
        }

        /// <summary>
        /// Gets Record set from Stored procedure
        /// </summary>
        /// <param name="SPName">Stored Procedure Name</param>
        /// <param name="SqlPrms">Optional Parameters</param>
        /// <returns></returns>
        public static DataTable GetDataTableWithExtendedTimeOut(string SPName, params SqlParameter[] SqlPrms)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(SPName, conn))
                {
                    command.Parameters.AddRange(SqlPrms);
                    command.CommandTimeout = 0;
                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.SelectCommand = command;
                        dataAdapter.Fill(dt);
                        conn.Close();
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Gets Record set from Stored procedure
        /// </summary>
        /// <param name="SPName">Stored Procedure Name</param>
        /// <param name="SqlPrms">Optional Parameters</param>
        /// <returns></returns>
        public static DataSet GetDataSetWithExtendedTimeOut(string SPName, params SqlParameter[] SqlPrms)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(SPName, conn))
                {
                    command.Parameters.AddRange(SqlPrms);
                    command.CommandTimeout = 0;
                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.SelectCommand = command;
                        dataAdapter.Fill(ds);
                        conn.Close();
                        return ds;
                    }
                }
            }
        }

        public static object ExecuteSP(string SPName, params SqlParameter[] SqlPrms)
        {
            var conn = new SqlConnection(connectionString);


            var result = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, SPName, SqlPrms);
            

            return result;
        }

        public async static Task SetRecordAsync(string SPName, params SqlParameter[] SqlPrms)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(connectionString);

            await Task.Run(() =>
            {
                cmd = new SqlCommand(SPName, con);
                cmd.Parameters.AddRange(SqlPrms);
                cmd.CommandTimeout = 240;
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(ds);
            });
        }
        
        #endregion

        #region 'OTHER USERFUL OPERATIONS'
        //---------------------------------------------------------------------------------------
        public static XmlReader GetXML(string SPName, params SqlParameter[] SqlPrms)
        {
            XmlReader XmlR = SqlHelper.ExecuteXmlReader(
                new SqlConnection(Utility.SessionManager.ConnectionString.ToString()),
                CommandType.StoredProcedure,
                SPName,
                SqlPrms);

            return XmlR;
        }
        //---------------------------------------------------------------------------------------
        public static SqlDataReader GetRecordSet(string SPName, params SqlParameter[] SqlPrms)
        {
            SqlDataReader dr = SqlHelper.ExecuteReader(
                new SqlConnection(Utility.SessionManager.ConnectionString.ToString()),
                CommandType.StoredProcedure,
                SPName,
                SqlPrms);

            return dr;
        }

        //---------------------------------------------------------------------------------------
        public static object ExecuteScalar(string SPName, params SqlParameter[] SqlPrms)
        {
            var conn = new SqlConnection(connectionString);

            var result = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, SPName, SqlPrms);

            return result;
        }
        //---------------------------------------------------------------------------------------
        public static int Execute(string SPName, params SqlParameter[] SqlPrms)
        {
            int n = SqlHelper.ExecuteNonQuery(
                new SqlConnection(Utility.SessionManager.ConnectionString.ToString()),
                CommandType.StoredProcedure,
                SPName,
                SqlPrms);

            return n;
        }
        #endregion

        #region 'Get List Object from DataTable'
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            if (tbl != null && tbl.Rows.Count > 0)
            {
                // go through each row
                foreach (DataRow r in tbl.Rows)
                {
                    // add to the list
                    lst.Add(CreateItemFromRow<T>(r));
                }
            }

            // return the list
            return lst;
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    //p.SetValue(item, Convert.ChangeType(row[c], p.PropertyType), null);
                    p.SetValue(item, ChangeType(row[c], p.PropertyType), null);
                }
            }
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
        #endregion
    }
}
