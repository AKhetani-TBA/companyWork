using EMS_BASE.Models;
using EMS_BASE.Models.Utilities;    
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EMS_DAL
{
    public class EmployeeDBHandler
    {

        /// <summary>
        /// Object of Config Class
        /// </summary>
        Config config = new Config();

        public List<DepartmentBase> GetDepartmentNames(int deptId)
        {
            try
            {
                List<DepartmentBase> lstDeptName = new List<DepartmentBase>();
                lstDeptName.Add(new DepartmentBase(0, "Select"));
                lstDeptName.Add(new DepartmentBase(1, ".Net"));
                lstDeptName.Add(new DepartmentBase(2, "C++"));

                return lstDeptName;
            }
            catch
            {
                return null;
            }
        }

        public string saveDetails(EmployeeBasicDetails empbasic)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {                    
                    using (SqlCommand sqlCommand = new SqlCommand(""))
                    {                        
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@FirstName", empbasic.FirstName);
                        sqlCommand.Parameters.AddWithValue("@MiddleName", empbasic.MiddleName);
                        sqlCommand.Parameters.AddWithValue("@LastName", empbasic.LastName);
                        sqlCommand.Parameters.AddWithValue("@DatofJoining", empbasic.DateOfJoining);
                        sqlCommand.Parameters.AddWithValue("@WorkEmialId", empbasic.WorkEmailId);

                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {

                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            return null;
        }

        public DataSet ValidateUser(string userName)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;            
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_validate_user",sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@P_UserName", userName));
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);
                        sqlConnection.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            return dataSet;
        }

        public DataSet GetMenuList(int employeeId)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetMenuDetailsRoleBased", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@P_EmployeeId", employeeId));
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);
                        sqlConnection.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            return dataSet;
        }
    }
}
