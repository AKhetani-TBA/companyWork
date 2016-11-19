using System;
using EMS_BASE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace EMS_DAL
{
    public class DocumentUploadAdminDBHandler
    {

        public List<DocumentUploadAdminBase> GetYear()
        {
            try
            {
                List<DocumentUploadAdminBase> lstYear = new List<DocumentUploadAdminBase>();
                lstYear.Add(new DocumentUploadAdminBase(0, "--Select--"));
                lstYear.Add(new DocumentUploadAdminBase(1, "2010-11"));
                lstYear.Add(new DocumentUploadAdminBase(2, "2011-12"));
                lstYear.Add(new DocumentUploadAdminBase(3, "2012-13"));
                lstYear.Add(new DocumentUploadAdminBase(4, "2013-14"));
                lstYear.Add(new DocumentUploadAdminBase(5, "2014-15"));
                lstYear.Add(new DocumentUploadAdminBase(6, "2015-16"));
                lstYear.Add(new DocumentUploadAdminBase(7, "2016-17"));

                return lstYear;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public dynamic GetPreviousRecord(int Id, String ConnStr)
        {
            try
            {
                SqlConnection conn;
                conn = new SqlConnection(ConnStr);
                conn.Open();
                string SqlStringTemp = "SELECT * FROM [EMS_2016].[dbo].[Emp_Documents]";
                SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, conn);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                var list = new List<DocumentUploadSetDataBase>();

                try
                {
                    while (reader.Read())
                    {
                        list.Add(new DocumentUploadSetDataBase() { Section = reader["docId"].ToString(), Head = reader["empId"].ToString(), Basis = reader["documentId"].ToString(), Minimum = reader["LastAction"].ToString(), Maximum = reader["CreatedBy"].ToString(), WEF = reader["CreatedDate"].ToString() });
                    }
                }
                catch (SqlException se)
                { }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string DocumentUploadAdmin(DocumentUploadAdminBase DUA)
        {

            return "Successfully Saved";
        }

    }
}
