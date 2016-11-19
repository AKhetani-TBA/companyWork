using EMS_BASE.Models;
using EMS_BASE.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_DAL
{
    public class MastersDBHandler
    {
        /// <summary>
        /// Object of Config Class
        /// </summary>
        Config config = new Config();


        public DataSet GetEmployeeName()
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("Select EmployeeId,FirstName from Emp_Basic_Details", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

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

        #region Role
        public List<Role> GetRoleDetails(int roleId)
        {
            SqlConnection sqlConnection = null;
            List<Role> roleDetails = new List<Role>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetRoleDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleId", roleId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {

                            while (dataReader.Read())
                            {
                                Role role = new Role();
                                role.RoleId = Convert.ToInt32(dataReader[0]);
                                role.RoleName = Convert.ToString(dataReader["RoleName"]);
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    role.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    role.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    role.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    role.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("LastAction"))))
                                {
                                    role.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("CeaseDate"))))
                                {
                                    role.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                roleDetails.Add(role);
                            }
                        }


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
            return roleDetails;
        }

        public DataSet AddRoleDetails(Role roleDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleId", roleDetails.RoleId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleName", roleDetails.RoleName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", roleDetails.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", roleDetails.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", roleDetails.LastAction));
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

        public DataSet UpdateRoleDetails(Role roleDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleId", roleDetails.RoleId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleName", roleDetails.RoleName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", roleDetails.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", roleDetails.ModifyDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", roleDetails.LastAction));
                        if (roleDetails.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", roleDetails.CeaseDate));
                        }
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
        #endregion

        #region Role Allocation
        public DataSet GetRoleName()
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("Select RoleId,RoleName from Role_Mst", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

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

        public List<RoleAllocation> GetRoleAllocationDetails(int roleAllocationId)
        {
            SqlConnection sqlConnection = null;
            List<RoleAllocation> roleAllocationDetails = new List<RoleAllocation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetRoleAllocationDetails", sqlConnection))
                    //using (SqlCommand sqlCommand = new SqlCommand("Select * from Emp_Role_Details", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleAllocationId", roleAllocationId));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_EmployeeRoleId", roleAllocationId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                RoleAllocation roleAllocation = new RoleAllocation();
                                roleAllocation.EmployeeId = Convert.ToInt32(dataReader["EmployeeId"]);
                                roleAllocation.RoleId = Convert.ToInt32(dataReader["RoleId"]);
                                roleAllocation.WithEffectFrom = Convert.ToDateTime(dataReader["WithEffectFrom"]);

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ToDate")))
                                {
                                    roleAllocation.ToDate = Convert.ToDateTime(dataReader["ToDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    roleAllocation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("CreatedDate"))))
                                {
                                    roleAllocation.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    roleAllocation.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    roleAllocation.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    roleAllocation.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    roleAllocation.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                roleAllocationDetails.Add(roleAllocation);
                            }
                        }
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
            return roleAllocationDetails;

        }

        public DataSet AddRoleAllocationDetails(RoleAllocation roleAllocation)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    //string query = "Insert into Emp_Role_Allocation values (@RoleAllocationId,@EmployeeId, @RoleId, @WEF, @ToDate, @CreatedBy, @CreatedDate, @LastAction,@CeaseDate)";
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleAllocation", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.Text;

                        sqlCommand.Parameters.AddWithValue("@p_RoleAllocationId", roleAllocation.RoleAllocationId);
                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", roleAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_RoleId", roleAllocation.RoleId);
                        sqlCommand.Parameters.AddWithValue("@p_WEF", roleAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", roleAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", roleAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate", roleAllocation.CreatedDate);
                        sqlCommand.Parameters.AddWithValue("@p_ModifyBy", roleAllocation.ModifyBy);
                        sqlCommand.Parameters.AddWithValue("@p_ModifyDate", roleAllocation.ModifyDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", roleAllocation.LastAction);

                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                    //using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleAllocationDetails", sqlConnection))
                    //{
                    //    sqlConnection.Open();
                    //    sqlCommand.CommandType = CommandType.StoredProcedure;

                    //    sqlCommand.Parameters.AddWithValue("@p_RoleAllocationId", roleAllocation.RoleAllocationId);
                    //    sqlCommand.Parameters.AddWithValue("@p_EmployeeId", roleAllocation.EmployeeId);
                    //    sqlCommand.Parameters.AddWithValue("@p_RoleId", roleAllocation.RoleId);
                    //    sqlCommand.Parameters.AddWithValue("@p_WEF", roleAllocation.WithEffectFrom);
                    //    sqlCommand.Parameters.AddWithValue("@p_ToDate", roleAllocation.ToDate);
                    //    sqlCommand.Parameters.AddWithValue("@p_By", roleAllocation.By);
                    //    sqlCommand.Parameters.AddWithValue("@p_Date", roleAllocation.Date);
                    //    sqlCommand.Parameters.AddWithValue("@LastAction", roleAllocation.LastAction);

                    //    sqlCommand.ExecuteNonQuery();
                    //    sqlConnection.Close();

                    //}
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

        public DataSet UpdateRoleAllocationDetails(RoleAllocation roleAllocation)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleAllocationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleAllocationId", roleAllocation.RoleAllocationId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_EmployeeId", roleAllocation.EmployeeId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_RoleId", roleAllocation.ToDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", roleAllocation.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", roleAllocation.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", roleAllocation.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", roleAllocation.ModifyDate));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", roleAllocation.RoleAllocationId));
                        if (roleAllocation.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", roleAllocation.CeaseDate));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        #endregion

        #region Department

        public List<Department> GetDepartmentDetails(int deptId)
        {
            SqlConnection sqlConnection = null;
            List<Department> departmentDetails = new List<Department>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", deptId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Department department = new Department();
                                department.DeptId = Convert.ToInt32(dataReader["DeptId"]);
                                department.DeptName = Convert.ToString(dataReader["DeptName"]);
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    department.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    department.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    department.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    department.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    department.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    department.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                departmentDetails.Add(department);
                            }
                        }


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
            return departmentDetails;
        }

        public DataSet AddDepartmentDetails(Department departmentDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDepartmentDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", departmentDetails.DeptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptName", departmentDetails.DeptName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", departmentDetails.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", departmentDetails.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", departmentDetails.LastAction));
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        public DataSet UpdateDepartmentDetails(Department departmentDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDepartmentDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", departmentDetails.DeptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptName", departmentDetails.DeptName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", departmentDetails.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", departmentDetails.ModifyDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", departmentDetails.LastAction));
                        if (departmentDetails.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", departmentDetails.CeaseDate));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        #endregion Department

        #region Department Allocation

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

        public DataSet AddDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDepartmentAllocation", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", departmentAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DepartmentId", departmentAllocation.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@p_WEF", departmentAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", departmentAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", departmentAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate", departmentAllocation.CreatedDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", departmentAllocation.LastAction);

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

        public List<DepartmentAllocation> GetDepartmentAllocation(int employeeId, int departmentId)
        {
            SqlConnection sqlConnection = null;
            List<DepartmentAllocation> departmentAllocationDetails = new List<DepartmentAllocation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentAllocation", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_EmployeeId", employeeId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesignationId", departmentId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                DepartmentAllocation departmentAllocation = new DepartmentAllocation();
                                //designationAllocation.EmpName = Convert.ToString(dataReader["EmployeeName"]);
                                departmentAllocation.EmployeeId = Convert.ToInt32(dataReader["EmployeeId"]);
                                departmentAllocation.DepartmentId = Convert.ToInt32(dataReader["DesignationId"]);
                                //designationAllocation.DesignationName = Convert.ToString(dataReader["DesignationName"]);
                                departmentAllocation.WithEffectFrom = Convert.ToDateTime(dataReader["WithEffectFrom"]);

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ToDate")))
                                {
                                    departmentAllocation.ToDate = Convert.ToDateTime(dataReader["ToDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    departmentAllocation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                //if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                //{
                                //    designationAllocation.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                //}
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    departmentAllocation.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    departmentAllocation.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    departmentAllocation.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    departmentAllocation.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                departmentAllocationDetails.Add(departmentAllocation);
                            }
                        }
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
            return departmentAllocationDetails;
        }

        public DataSet UpdateDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDepartmentAllocation", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", departmentAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DesignationId", departmentAllocation.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@p_WEF", departmentAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", departmentAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", departmentAllocation.ModifyBy);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate", departmentAllocation.ModifyDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", departmentAllocation.LastAction);
                        if (departmentAllocation.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", departmentAllocation.CeaseDate));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }


        #endregion

        #region Designation

        public List<Designation> GetDesignationDetails(int desigId)
        {
            SqlConnection sqlConnection = null;
            List<Designation> designationDetails = new List<Designation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDesignationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigId", desigId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Designation designation = new Designation();
                                designation.DesigId = Convert.ToInt32(dataReader["DesignationId"]);
                                designation.DesigName = Convert.ToString(dataReader["DesignationName"]);
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    designation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    designation.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    designation.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    designation.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    designation.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    designation.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                designationDetails.Add(designation);
                            }
                        }
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
            return designationDetails;
        }

        public DataSet AddDesignationDetails(Designation designationDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDesignationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigId", designationDetails.DesigId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigName", designationDetails.DesigName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", designationDetails.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", designationDetails.CreatedDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", designationDetails.LastAction));
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        public DataSet UpdateDesignationDetails(Designation designationDetails)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDesignationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigId", designationDetails.DesigId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigName", designationDetails.DesigName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", designationDetails.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", designationDetails.ModifyDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", designationDetails.LastAction));
                        if (designationDetails.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", designationDetails.CeaseDate));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        #endregion

        #region Designation Allocation

        public DataSet GetDesignationNames(int desigId, char lastAction)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDesignationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesigId", desigId));
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

        public DataSet AddDesignationAllocationDetails(DesignationAllocation designationAllocation)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDesignationAllocationDetails", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", designationAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DesignationId", designationAllocation.DesignationId);
                        sqlCommand.Parameters.AddWithValue("@p_WEF", designationAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", designationAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", designationAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate", designationAllocation.CreatedDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", designationAllocation.LastAction);

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

        public List<DesignationAllocation> GetDesignationAllocationDetails(int employeeId, int designationId)
        {
            SqlConnection sqlConnection = null;
            List<DesignationAllocation> designationAllocationDetails = new List<DesignationAllocation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDesignationAllocationDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_DesigAllocationId", desigAllocationId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_EmployeeId", employeeId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesignationId", designationId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                DesignationAllocation designationAllocation = new DesignationAllocation();
                                //designationAllocation.EmpName = Convert.ToString(dataReader["EmployeeName"]);
                                designationAllocation.EmployeeId = Convert.ToInt32(dataReader["EmployeeId"]);
                                designationAllocation.DesignationId = Convert.ToInt32(dataReader["DesignationId"]);
                                //designationAllocation.DesignationName = Convert.ToString(dataReader["DesignationName"]);
                                designationAllocation.WithEffectFrom = Convert.ToDateTime(dataReader["WithEffectFrom"]);

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ToDate")))
                                {
                                    designationAllocation.ToDate = Convert.ToDateTime(dataReader["ToDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    designationAllocation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                //if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                //{
                                //    designationAllocation.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                //}
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                {
                                    designationAllocation.CreatedBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    designationAllocation.CreatedDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    designationAllocation.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    designationAllocation.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
                                }
                                designationAllocationDetails.Add(designationAllocation);
                            }
                        }
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
            return designationAllocationDetails;
        }

        public DataSet UpdateDesignationAllocation(DesignationAllocation designationAllocation)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDesignationAllocationDetails", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", designationAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DesignationId", designationAllocation.DesignationId);
                        sqlCommand.Parameters.AddWithValue("@p_WEF", designationAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", designationAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", designationAllocation.ModifyBy);
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate", designationAllocation.ModifyDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", designationAllocation.LastAction);
                        if (designationAllocation.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", designationAllocation.CeaseDate));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        #endregion

        /// <summary>
        /// This is for Section and Exemption
        /// </summary>
        /// <returns></returns>

        #region Section and Exemption

        public dynamic YearList()
        {
            try
            {
                SqlConnection sqlConnection = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    sqlConnection.Open();
                    string SqlStringTemp = "exec proc_GetFinancialYearDetails";
                    SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    var list = new List<FYBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new FYBase() { FinancialYearId = reader["FinancialYearId"].ToString(), FinancialYear = reader["FinancialYear"].ToString(), CurrentFlag = Convert.ToBoolean(reader["CurrentFlag"])});
                        }
                    }
                    catch (SqlException se)
                    { }
                    finally
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                    }
                    return list;
                }
            }
            catch (SqlException se)
            {
                return null;
            }
        }

        public dynamic SectionList()
        {
            try
            {
                SqlConnection sqlConnection = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    sqlConnection.Open();
                    string SqlStringTemp = "exec proc_GetSectionList";
                    SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    var list = new List<SectionBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new SectionBase() { SectionId = Convert.ToInt32(reader["SectionId"]), SectionName = reader["SectionName"].ToString() });
                        }
                    }
                    catch (SqlException se)
                    { }
                    finally
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                    }
                    return list;
                }
            }
            catch (SqlException se)
            {
                return null;
            }
        }

        public dynamic BasisList()
        {
            try
            {
                SqlConnection sqlConnection = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    sqlConnection.Open();
                    string SqlStringTemp = "exec proc_GetBasisList";
                    SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    var list = new List<BasisBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new BasisBase() { BasisId = Convert.ToInt32(reader["BasisId"]), BasisName = reader["BasisName"].ToString() });
                        }
                    }
                    catch (SqlException se)
                    { }
                    finally
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                    }
                    return list;
                }
            }
            catch (SqlException se)
            {
                return null;
            }
        }

        public List<DocumentUploadAdminBase> GetPreviousRecordOfSectionsExemptionsDetails(int SecExecId)
        {
            SqlConnection sqlConnection = null;
            List<DocumentUploadAdminBase> DocumentUploadDetails = new List<DocumentUploadAdminBase>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetSectionExemptionDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_SecExemID", SecExecId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                DocumentUploadAdminBase document = new DocumentUploadAdminBase();

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("SecExemID")))
                                {
                                    document.SecExemID = Convert.ToInt32(dataReader["SecExemID"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("FinancialYearId")))
                                {
                                    document.FYId = Convert.ToString(dataReader["FinancialYearId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("FinancialYear")))
                                {
                                    document.FY = Convert.ToString(dataReader["FinancialYear"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("SectionId")))
                                {
                                    document.SectionId = Convert.ToString(dataReader["SectionId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("SectionName")))
                                {
                                    document.Section = Convert.ToString(dataReader["SectionName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("HeadName")))
                                {
                                    document.Head = Convert.ToString(dataReader["HeadName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("BasisId")))
                                {
                                    document.BasisId = Convert.ToString(dataReader["BasisId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("BasisName")))
                                {
                                    document.Basis = Convert.ToString(dataReader["BasisName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Abbreviation")))
                                {
                                    document.Abbreviation = Convert.ToString(dataReader["Abbreviation"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("MaximumAmt")))
                                {
                                    document.Maximum = Convert.ToString(dataReader["MaximumAmt"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EffectFromDate")))
                                {
                                    document.WEF = Convert.ToString(dataReader["EffectFromDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    document.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyDate")))
                                {
                                    document.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    document.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                DocumentUploadDetails.Add(document);
                            }
                        }
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
            return DocumentUploadDetails;
        }

        public DataSet DocumentUploadAdmin(DocumentUploadAdminBase DUA)
        {
            try
            {
                SqlConnection sqlConnection = null;
                DataSet dataSet = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    //DateTime date = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                    using (SqlCommand command = new SqlCommand("proc_SubmitSectionExemptionDetails", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_SecExemId", 0);
                        command.Parameters.AddWithValue("@p_FinancialYearId", Convert.ToInt32(DUA.FY));
                        command.Parameters.AddWithValue("@p_EffectFromDate", DateTime.ParseExact(DUA.WEF, "dd/MM/yyyy", null));
                        command.Parameters.AddWithValue("@p_SectionId", Convert.ToInt32(DUA.Section));
                        command.Parameters.AddWithValue("@p_HeadName", DUA.Head);
                        command.Parameters.AddWithValue("@p_Abbreviation", DUA.Abbreviation);
                        command.Parameters.AddWithValue("@p_BasisId", Convert.ToInt32(DUA.Basis));
                        command.Parameters.AddWithValue("@p_MaximumAmt", Convert.ToDecimal(DUA.Maximum));
                        command.Parameters.AddWithValue("@p_Remarks", DUA.Remarks);
                        command.Parameters.AddWithValue("@p_CreatedBy", DUA.CreatedBy);
                        command.Parameters.AddWithValue("@p_CreatedDate", DUA.CreatedDate);
                        command.Parameters.AddWithValue("@p_LastAction", DUA.LastAction);

                        sqlConnection.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                        dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);
                        sqlConnection.Close();
                    }
                }
                return dataSet;
            }
            catch (SqlException se)
            {
                throw;
             
            }
        }

        public DataSet UpdateDocumentUploadAdminDetails(DocumentUploadAdminBase DUA)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitSectionExemptionDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_SecExemId", Convert.ToInt32(DUA.SecExemID)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_FinancialYearId", Convert.ToInt32(DUA.FY)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_EffectFromDate", DateTime.ParseExact(DUA.WEF, "dd/MM/yyyy", null)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_SectionId", Convert.ToInt32(DUA.Section)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_HeadName", DUA.Head));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_Abbreviation", DUA.Abbreviation));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_BasisId", Convert.ToInt32(DUA.Basis)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_MaximumAmt", Convert.ToDecimal(DUA.Maximum)));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", DUA.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", DUA.ModifyDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", DUA.LastAction));
                        if (DUA.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", DateTime.ParseExact(DUA.CeaseDate, "dd/MM/yyyy", null)));
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataset = new DataSet();
                        sqlDataAdapter.Fill(dataset);
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
            return dataset;
        }

        #endregion
    }
}
