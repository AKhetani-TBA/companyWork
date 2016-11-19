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

        public DataSet GetDepartmentNames(int deptId, char lastAction)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", deptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", lastAction));
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
        //    try
        //    {
        //        List<DepartmentBase> lstDeptName = new List<DepartmentBase>();
        //        lstDeptName.Add(new DepartmentBase(0, "Select"));
        //        lstDeptName.Add(new DepartmentBase(1, ".Net"));
        //        lstDeptName.Add(new DepartmentBase(2, "C++"));

        //        return lstDeptName;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

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

        public DataSet ValidateUser(string domainUser)
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
                        sqlCommand.Parameters.Add(new SqlParameter("@P_DomainUser", domainUser.ToLower() ));
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

        public DataSet AddEmployeePersonalDetails(EmployeeBasicDetails empBasic)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitEmployeeBasicDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_EmployeeId", empBasic.EmployeeId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_FirstName", empBasic.FirstName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_MiddleName", empBasic.MiddleName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastName", empBasic.LastName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_Gender", empBasic.Gender));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DateOfBirth", empBasic.DateOfBirth));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ContactNo", empBasic.ContactNo));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DateOfJoining", empBasic.DateOfJoining));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DepartmentId", empBasic.DeptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DomainUser", empBasic.DomainUser));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_WorkEmailId", empBasic.WorkEmailId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", empBasic.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", empBasic.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", empBasic.LastAction));

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
