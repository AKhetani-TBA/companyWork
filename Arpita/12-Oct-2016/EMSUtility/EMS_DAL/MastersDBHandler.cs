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

        /// <summary>
        /// Document Upload Admin Controller
        /// </summary>
        /// <returns></returns>
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
                            list.Add(new FYBase() { FinancialYearId = reader["FinancialYearId"].ToString(), FinancialYear = reader["FinancialYear"].ToString(), CurrentFlag = reader["CurrentFlag"].ToString() });
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


        public dynamic GetPreviousRecordOfSectionsExemptions()
        {
            try
            {
                SqlConnection sqlConnection = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    sqlConnection.Open();
                    string SqlStringTemp = "SELECT * FROM [EMS_2016].[dbo].[SectionExemptionDetails]";
                    SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    var list = new List<DocumentUploadSetDataBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new DocumentUploadSetDataBase() { Section = reader["SectionId"].ToString(), Head = reader["HeadName"].ToString(), Basis = reader["BasisId"].ToString(), Minimum = reader["MinimumAmt"].ToString(), Maximum = reader["MaximumAmt"].ToString(), WEF = reader["EffectFromDate"].ToString() });
                        }
                    }
                    catch (SqlException ex)
                    { }
                    finally
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet DocumentUploadAdmin(DocumentUploadAdminBase DUA)
        {
            try
            {
                SqlConnection sqlConnection = null;
                DataSet dataSet = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("proc_SubmitSectionExemptionDetails", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_SecExemId", 0);
                        command.Parameters.AddWithValue("@p_FinancialYearId", Convert.ToInt32(DUA.FY));
                        command.Parameters.AddWithValue("@p_EffectFromDate", Convert.ToDateTime(DUA.WEF));
                        command.Parameters.AddWithValue("@p_SectionId", Convert.ToInt32(DUA.Section));
                        command.Parameters.AddWithValue("@p_HeadName", DUA.Head);
                        command.Parameters.AddWithValue("@p_Abbreviation", DUA.Abrevation);
                        command.Parameters.AddWithValue("@p_BasisId", Convert.ToInt32(DUA.Basis));
                        command.Parameters.AddWithValue("@p_MinimumAmt", Convert.ToDecimal(DUA.Minimum));
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

    }
}
