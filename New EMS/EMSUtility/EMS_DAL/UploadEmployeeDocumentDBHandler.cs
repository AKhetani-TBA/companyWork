using EMS_BASE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_DAL
{
    public class UploadEmployeeDocumentDBHandler
    {
        public DataTable GetEmployeeDocuments(int empId, string connStr)
        {
            try
            {
                SqlConnection Conn;
                Conn = new SqlConnection(connStr);

                string SqlString = "SELECT * FROM [EMS_2016].[dbo].[Emp_Documents]";
                SqlDataAdapter sda = new SqlDataAdapter(SqlString, Conn);
                DataTable dt = new DataTable();
                try
                {
                    Conn.Open();
                    sda.Fill(dt);
                }
                catch (SqlException se)
                { }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                    sda.Dispose();

                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
