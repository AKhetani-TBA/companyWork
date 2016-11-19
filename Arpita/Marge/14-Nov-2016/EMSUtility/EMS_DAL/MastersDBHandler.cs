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

        public List<RoleAllocation> GetRoleAllocationDetails(int roleAllocationId, char lastAction)
        {
            SqlConnection sqlConnection = null;
            List<RoleAllocation> roleAllocationDetails = new List<RoleAllocation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetRoleAllocation", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_GetRoleAllocationId", roleAllocationId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", lastAction));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                RoleAllocation roleAllocation = new RoleAllocation();
                                roleAllocation.RoleAllocationId = Convert.ToInt32(dataReader["RoleAllocationId"]);
                                roleAllocation.RoleName = Convert.ToString(dataReader["RoleName"]);
                                roleAllocation.EmployeeName = Convert.ToString(dataReader["EmployeeName"]);
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
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitRoleAllocation", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@p_RoleAllocationId", roleAllocation.RoleAllocationId);
                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", roleAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_RoleId", roleAllocation.RoleId);
                        sqlCommand.Parameters.AddWithValue("@p_EffectFromDate", roleAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", roleAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_By", roleAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_Date", roleAllocation.CreatedDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", roleAllocation.LastAction);

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

        //public List<Department> GetDepartmentDetails(int deptId)
        //{
        //    SqlConnection sqlConnection = null;
        //    List<Department> departmentDetails = new List<Department>();
        //    try
        //    {
        //        using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
        //        {
        //            using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentDetails", sqlConnection))
        //            {
        //                sqlCommand.CommandType = CommandType.StoredProcedure;
        //                sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", deptId));
        //                sqlConnection.Open();
        //                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        Department department = new Department();
        //                        department.DeptId = Convert.ToInt32(dataReader["DeptId"]);
        //                        department.DeptName = Convert.ToString(dataReader["DeptName"]);
        //                        //department.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
        //                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedName")))
        //                        {
        //                            department.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
        //                        }
        //                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
        //                        {
        //                            department.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
        //                        }
        //                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyName")))
        //                        {
        //                            department.ModifyBy = Convert.ToString(dataReader["ModifyName"]);
        //                        }
        //                        if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
        //                        {
        //                            department.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
        //                        }
        //                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
        //                        {
        //                            department.LastAction = Convert.ToChar(dataReader["LastAction"]);
        //                        }
        //                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
        //                        {
        //                            department.CeaseDate = Convert.ToDateTime(dataReader["CeaseDate"]);
        //                        }
        //                        departmentDetails.Add(department);
        //                    }
        //                }


        //                sqlConnection.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
        //        {
        //            sqlConnection.Close();
        //        }
        //    }
        //    return departmentDetails;
        //}

        public List<Department> GetDepartmentDetails(int deptId, int activeId)
        {
            SqlConnection sqlConnection = null;
            List<Department> departmentDetails = new List<Department>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDepartmentsDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DeptId", deptId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_active", activeId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Department department = new Department();
                                department.DeptId = Convert.ToInt32(dataReader["DeptId"]);
                                department.DeptName = Convert.ToString(dataReader["DeptName"]);
                                //department.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedName")))
                                {
                                    department.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    department.CreatedDate = Convert.ToString(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyName")))
                                {
                                    department.ModifyBy = Convert.ToString(dataReader["ModifyName"]);
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
                                    department.CeaseDate = Convert.ToString(dataReader["CeaseDate"]);
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
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", DateTime.ParseExact(departmentDetails.CreatedDate, "dd/MM/yyyy", null)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", departmentDetails.CreatedDate));                        
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
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", DateTime.ParseExact(departmentDetails.CeaseDate, "dd/MM/yyyy", null)));
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

        public DataSet AddDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = new DataSet();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDepartmentAllocation", sqlConnection))
                    {

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@p_DepartmentAllocationId", departmentAllocation.DepartmentAllocationId);
                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", departmentAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DepartmentId", departmentAllocation.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@p_EffectFromDate", DateTime.ParseExact(departmentAllocation.EffectFromDate, "dd/MM/yyyy", null));
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", departmentAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_By", departmentAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_Date", DateTime.ParseExact(departmentAllocation.CreatedDate, "dd/MM/yyyy", null));
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", departmentAllocation.LastAction);

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

        public List<DepartmentAllocation> GetDepartmentAllocation(int deptAllocationId, int activeId)
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
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DepartmentAllocationId", deptAllocationId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_active", activeId));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", lastAction));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                DepartmentAllocation departmentAllocation = new DepartmentAllocation();
                                departmentAllocation.DepartmentAllocationId = Convert.ToInt32(dataReader["DepartmentAllocationId"]);
                                departmentAllocation.DepartmentName = Convert.ToString(dataReader["DeptName"]);
                                departmentAllocation.FirstName = Convert.ToString(dataReader["FirstName"]);
                                departmentAllocation.LastName = Convert.ToString(dataReader["LastName"]);
                                departmentAllocation.EmployeeName = departmentAllocation.FirstName + " " + departmentAllocation.LastName;
                                departmentAllocation.EmployeeId = Convert.ToInt32(dataReader["EmployeeId"]);
                                departmentAllocation.DepartmentId = Convert.ToInt32(dataReader["DepartmentId"]);

                                //departmentAllocation.WithEffectFrom = Convert.ToDateTime(dataReader["EffectFromDate"]);
                                //if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")))
                                //{
                                //    departmentAllocation.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                //}
                                //if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                //{
                                //    departmentAllocation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                //}

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ToDate")))
                                {
                                    departmentAllocation.ToDate = Convert.ToDateTime(dataReader["ToDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EffectFromDate")))
                                {
                                    departmentAllocation.EffectFromDate = Convert.ToString(dataReader["EffectFromDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedName")))
                                {
                                    departmentAllocation.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    departmentAllocation.CreatedDate = Convert.ToString(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyName")))
                                {
                                    departmentAllocation.ModifyBy = Convert.ToString(dataReader["ModifyName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyDate")))
                                {
                                    departmentAllocation.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    departmentAllocation.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    departmentAllocation.CeaseDate = Convert.ToString(dataReader["CeaseDate"]);
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
                        sqlCommand.Parameters.AddWithValue("@p_DepartmentAllocationId", departmentAllocation.DepartmentAllocationId);
                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", departmentAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DepartmentId", departmentAllocation.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@p_EffectFromDate", DateTime.ParseExact(departmentAllocation.EffectFromDate, "dd/MM/yyyy", null));
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", departmentAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_By", departmentAllocation.ModifyBy);
                        sqlCommand.Parameters.AddWithValue("@p_Date", DateTime.ParseExact(departmentAllocation.EffectFromDate, "dd/MM/yyyy", null));
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", departmentAllocation.LastAction);
                        if (departmentAllocation.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", DateTime.ParseExact(departmentAllocation.CeaseDate, "dd/MM/yyyy", null)));
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
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitDesignationAllocationDetails", sqlConnection))
                    {

                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@p_DesignationAllocationId", designationAllocation.DesignationAllocationId);
                        sqlCommand.Parameters.AddWithValue("@p_EmployeeId", designationAllocation.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@p_DesignationId", designationAllocation.DesignationId);
                        sqlCommand.Parameters.AddWithValue("@p_EffectFromDate", designationAllocation.WithEffectFrom);
                        sqlCommand.Parameters.AddWithValue("@p_ToDate", designationAllocation.ToDate);
                        sqlCommand.Parameters.AddWithValue("@p_By", designationAllocation.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@p_Date", designationAllocation.CreatedDate);
                        sqlCommand.Parameters.AddWithValue("@p_LastAction", designationAllocation.LastAction);

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

        public List<DesignationAllocation> GetDesignationAllocation(int desigAllocationId, char lastAction)
        {
            SqlConnection sqlConnection = null;
            List<DesignationAllocation> designationAllocationDetails = new List<DesignationAllocation>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetDesignationAllocation", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_DesignationAllocationId", desigAllocationId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", lastAction));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                DesignationAllocation designationAllocation = new DesignationAllocation();
                                designationAllocation.DesignationAllocationId = Convert.ToInt32(dataReader["DesignationAllocationId"]);
                                designationAllocation.DesignationName = Convert.ToString(dataReader["DesignationName"]);
                                designationAllocation.EmployeeName = Convert.ToString(dataReader["FirstName"]);
                                designationAllocation.WithEffectFrom = Convert.ToDateTime(dataReader["EffectFromDate"]);

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ToDate")))
                                {
                                    designationAllocation.ToDate = Convert.ToDateTime(dataReader["ToDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedBy")))
                                {
                                    designationAllocation.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    designationAllocation.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                                }
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
                        sqlCommand.Parameters.AddWithValue("@p_EffectFromDate", designationAllocation.WithEffectFrom);
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

        #region Technology

        public List<Technology> GetTechnology(int techId, int activeId)
        {
            SqlConnection sqlConnection = null;
            List<Technology> tech = new List<Technology>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GetTechnologyDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechId", techId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_active", activeId));
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Technology technology = new Technology();
                                technology.TechId = Convert.ToInt32(dataReader["TechId"]);
                                technology.TechName = Convert.ToString(dataReader["TechName"]);
                                //department.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedName")))
                                {
                                    technology.CreatedBy = Convert.ToString(dataReader["CreatedName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CreatedDate")))
                                {
                                    technology.CreatedDate = Convert.ToString(dataReader["CreatedDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ModifyName")))
                                {
                                    technology.ModifyBy = Convert.ToString(dataReader["ModifyName"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32(dataReader.GetOrdinal("ModifyDate"))))
                                {
                                    technology.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastAction")))
                                {
                                    technology.LastAction = Convert.ToChar(dataReader["LastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("CeaseDate")))
                                {
                                    technology.CeaseDate = Convert.ToString(dataReader["CeaseDate"]);
                                }
                                tech.Add(technology);
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
            return tech;
        }

        public DataSet AddTechnology(Technology tech)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitTechnologyDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechId", tech.TechId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_techName", tech.TechName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedBy", tech.CreatedBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", DateTime.ParseExact(tech.CreatedDate, "dd/MM/yyyy", null)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_CreatedDate", departmentDetails.CreatedDate));                        
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", tech.LastAction));
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

        public DataSet UpdateTechnology(Technology tech)
        {
            SqlConnection sqlConnection = null;
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitTechnologyDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechId", tech.TechId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_TechName", tech.TechName));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", tech.ModifyBy));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", tech.ModifyDate));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", tech.LastAction));
                        if (tech.LastAction == 'D')
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", DateTime.ParseExact(tech.CeaseDate, "dd/MM/yyyy", null)));
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
