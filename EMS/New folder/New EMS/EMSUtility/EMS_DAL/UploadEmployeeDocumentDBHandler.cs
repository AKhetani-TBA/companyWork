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

        public List<DepartmentBase> GetBase()
        {
            try
            {
                List<DepartmentBase> lstDeptName = new List<DepartmentBase>();
                lstDeptName.Add(new DepartmentBase(0, "Select"));
                lstDeptName.Add(new DepartmentBase(1, "Salary"));
                lstDeptName.Add(new DepartmentBase(2, "InvDec"));

                return lstDeptName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DepartmentBase> GetDocumentName()
        {
            try
            {
                List<DepartmentBase> lstDeptName = new List<DepartmentBase>();
                lstDeptName.Add(new DepartmentBase(0, "Select"));
                lstDeptName.Add(new DepartmentBase(1, "House Rent Allowance"));
                lstDeptName.Add(new DepartmentBase(2, "Medical Bills"));
                lstDeptName.Add(new DepartmentBase(3, "Professional Attire Bills"));
                lstDeptName.Add(new DepartmentBase(4, "Leave Travel Allowance"));

                return lstDeptName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
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
