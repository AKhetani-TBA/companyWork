using EMS_BASE.Models;
using EMS_BASE.Models.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;

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
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentsDetails", sqlConnection))
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

        public DataSet GetTechnology(int techId, char lastAction)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    //using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentDetails", sqlConnection))
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetTechnologyDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechId", techId));
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

        public Int32 saveDetails(EmployeeBasicDetails empbasic)
        {
            SqlConnection sqlConnection = new SqlConnection();
            int result;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitEmployeeBasicDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.Add("@p_EmployeeId", Convert.ToInt32(empbasic.EmployeeId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_FirstName", empbasic.FirstName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_MiddleName", empbasic.MiddleName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastName", empbasic.LastName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_Gender", empbasic.Gender));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DateOfBirth", DateTime.ParseExact(empbasic.DateOfBirth, "dd/MM/yyyy", null)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ContactNo", empbasic.ContactNo));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DateOfJoining", DateTime.ParseExact(empbasic.DateOfJoining, "dd/MM/yyyy", null)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechId", empbasic.TechId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DepartmentId", empbasic.DeptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DomainUser", empbasic.DomainUser));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", empbasic.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", empbasic.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", empbasic.LastAction));

                        if (sqlConnection.State == ConnectionState.Closed)
                        {
                            sqlConnection.Open();
                        }
                        result = sqlCommand.ExecuteNonQuery();
                        sqlCommand.Dispose();
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
            return result;
        }

        public DataSet ValidateUser(string domainUser)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_validate_user", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@P_DomainUser", domainUser.ToLower()));
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
