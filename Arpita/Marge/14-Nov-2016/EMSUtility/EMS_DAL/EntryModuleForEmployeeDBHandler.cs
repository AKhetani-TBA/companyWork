using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_BASE.Models;
using System.Data.SqlClient;
using EMS_BASE.Models.Utilities;
using System.Data;

namespace EMS_DAL
{
    public class EntryModuleForEmployeeDBHandler
    {
        Config config = new Config();

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
                            list.Add(new FYBase() { FinancialYearId = reader["FinancialYearId"].ToString(), FinancialYear = reader["FinancialYear"].ToString(), CurrentFlag = Convert.ToBoolean(reader["CurrentFlag"]) });
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

        public dynamic HeadList()
        {
            try
            {
                SqlConnection sqlConnection = null;
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    sqlConnection.Open();
                    string SqlStringTemp = "exec proc_GetHeadList";
                    SqlCommand sqlCommand = new SqlCommand(SqlStringTemp, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    var list = new List<EntryModuleForEmployeeBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new EntryModuleForEmployeeBase() { SecExemID = Convert.ToInt32(reader["SecExemID"]), Head = reader["HeadName"].ToString(), Abbreviation = reader["Abbreviation"].ToString() });
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

                    var list = new List<EntryModuleForEmployeeBase>();

                    try
                    {
                        while (reader.Read())
                        {
                            list.Add(new EntryModuleForEmployeeBase() { BasisId = Convert.ToInt32(reader["BasisId"]), Basis = reader["BasisName"].ToString() });
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

        public DataSet SubmitEmployeeDocuments(EntryModuleForEmployeeBase EMEBaseObj)
        {
            SqlConnection sqlConnection = null;
            DataSet dataSet = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitEmpDocDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@p_EmpDocId", 0);
                        sqlCommand.Parameters.AddWithValue("@p_EmpId", Convert.ToInt32(EMEBaseObj.EmpId));
                        sqlCommand.Parameters.AddWithValue("@p_FinancialYearId", Convert.ToInt32(EMEBaseObj.FY));
                        sqlCommand.Parameters.AddWithValue("@p_HeadName", Convert.ToString(EMEBaseObj.Head));
                        sqlCommand.Parameters.AddWithValue("@p_UserDocName", Convert.ToString(EMEBaseObj.User_Doc_Name));
                        sqlCommand.Parameters.AddWithValue("@p_UserDocPath", Convert.ToString(EMEBaseObj.User_Doc_Path));
                        sqlCommand.Parameters.AddWithValue("@p_ServerDocName", Convert.ToString(EMEBaseObj.Server_Doc_Name));
                        sqlCommand.Parameters.AddWithValue("@p_ServerDocPath", Convert.ToString(EMEBaseObj.Server_Doc_Path));
                        sqlCommand.Parameters.AddWithValue("@p_CreatedBy", Convert.ToString(EMEBaseObj.CreatedBy));
                        sqlCommand.Parameters.AddWithValue("@p_CreatedDate ", Convert.ToDateTime(EMEBaseObj.CreatedDate));
                        sqlCommand.Parameters.AddWithValue("@p_LastAction ", Convert.ToChar(EMEBaseObj.LastAction));

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);
                    }
                    sqlConnection.Close();
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

            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    if (dataSet.Tables[0].Columns.Contains("MessageId"))
                    {
                        if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["MessageId"]) > 0)
                        {
                            using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitEmpSubDocDetails", sqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                //dataSet = new DataSet();
                                sqlCommand.Parameters.Add("@p_EmpSubDocId", SqlDbType.Int);
                                sqlCommand.Parameters.Add("@p_EmpDocId", SqlDbType.Int);
                                sqlCommand.Parameters.Add("@p_InvoiceDate", SqlDbType.DateTime);
                                sqlCommand.Parameters.Add("@p_InvoiceAmount", SqlDbType.Decimal);
                                sqlCommand.Parameters.Add("@p_Remarks", SqlDbType.Text);
                                sqlCommand.Parameters.Add("@p_LastAction", SqlDbType.Char);

                                sqlConnection.Open();

                                for (int iCount = 0; iCount < EMEBaseObj.InvoiceDetails.Count; iCount++)
                                {
                                    //sqlCommand.CommandType = CommandType.StoredProcedure;
                                    sqlCommand.Parameters["@p_EmpSubDocId"].Value =  0;
                                    sqlCommand.Parameters["@p_EmpDocId"].Value = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MessageId"]);
                                    sqlCommand.Parameters["@p_InvoiceDate"].Value = EMEBaseObj.InvoiceDetails[iCount].Invoice_Date;
                                    sqlCommand.Parameters["@p_InvoiceAmount"].Value =  Convert.ToDecimal(EMEBaseObj.InvoiceDetails[iCount].Invoice_Amt);
                                    sqlCommand.Parameters["@p_Remarks"].Value =  Convert.ToString(EMEBaseObj.InvoiceDetails[iCount].Invoice_Remark);
                                    sqlCommand.Parameters["@p_LastAction"].Value =  Convert.ToChar(EMEBaseObj.LastAction);

                                    sqlCommand.ExecuteNonQuery();
                                }
                                sqlConnection.Close();
                            }
                        }
                        else
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

        public List<EntryModuleForEmployeeBase> GetPreviousEmployeeDetails(int EmpId, int FyId)
        {
            SqlConnection sqlConnection = null;
            List<EntryModuleForEmployeeBase> DocumentUploadDetails = new List<EntryModuleForEmployeeBase>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_get_EmpDocDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_empid", EmpId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_fyid", FyId));

                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {

                            //document.InvoiceDetails = new List<InvoiceDetails>();
                            int iCount = 0;
                            while (dataReader.Read())
                            {
                                EntryModuleForEmployeeBase document = new EntryModuleForEmployeeBase();
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpDocId")))
                                {
                                    document.EmpDocId = Convert.ToInt32(dataReader["EmpDocId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpId")))
                                {
                                    document.EmpId = Convert.ToInt32(dataReader["EmpId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("FinancialYearId")))
                                {
                                    document.FYId= Convert.ToInt32(dataReader["FinancialYearId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("FinancialYear")))
                                {
                                    document.FY = Convert.ToString(dataReader["FinancialYear"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("HeadName")))
                                {
                                    document.Head = Convert.ToString(dataReader["HeadName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ServerDocName")))
                                {
                                    document.Server_Doc_Name = Convert.ToString(dataReader["ServerDocName"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("eddModifyBy")))
                                {
                                    document.ModifyBy = Convert.ToString(dataReader["eddModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("eddLastAction")))
                                {
                                    document.LastAction = Convert.ToChar(dataReader["eddLastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("eddCeaseDate")))
                                {
                                    document.CeaseDate = Convert.ToDateTime(dataReader["eddCeaseDate"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpSubDocId")))
                                {
                                    document.EmpSubDocId = Convert.ToInt32(dataReader["EmpSubDocId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("InvoiceDate")))
                                {
                                    //InvoiceDetails id = new InvoiceDetails();
                                    document.Invoice_Date = Convert.ToDateTime(dataReader["InvoiceDate"]);
                                    //document.InvoiceDetails.Add(id);                                    
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("InvoiceAmount")))
                                {
                                    //InvoiceDetails id = new InvoiceDetails();
                                    document.Invoice_Amt = Convert.ToString(dataReader["InvoiceAmount"]);
                                    //document.InvoiceDetails.Add(id);     
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("esdModifyBy")))
                                {
                                    document.ModifyDate = Convert.ToDateTime(dataReader["esdModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ApprovedAmount")))
                                {
                                    document.ApprovedAmount = Convert.ToString(dataReader["ApprovedAmount"]);
                                }
                                DocumentUploadDetails.Add(document);
                                iCount++;
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

        public List<EntryModuleForEmployeeBase> GetPreviousEmployeeSingleDocDetails(int EmpId, int FyId, string Head)
        {
            SqlConnection sqlConnection = null;
            List<EntryModuleForEmployeeBase> DocumentUploadDetails = new List<EntryModuleForEmployeeBase>();
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_get_SingleDocDetails", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@p_empid", EmpId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_fyid", FyId));
                        sqlCommand.Parameters.Add(new SqlParameter("@p_headname", Head));

                        
                        sqlConnection.Open();
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                          
                            //document.InvoiceDetails = new List<InvoiceDetails>();
                            //int iCount = 0;
                            while (dataReader.Read())
                            {
                                EntryModuleForEmployeeBase document = new EntryModuleForEmployeeBase();
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpSubDocId")))
                                {
                                    document.EmpSubDocId = Convert.ToInt32(dataReader["EmpSubDocId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpDocId")))
                                {
                                    document.EmpDocId = Convert.ToInt32(dataReader["EmpDocId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("EmpSubDocId")))
                                {
                                    document.EmpSubDocId = Convert.ToInt32(dataReader["EmpSubDocId"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("InvoiceDate")))
                                {
                                    //InvoiceDetails id = new InvoiceDetails();
                                    //id.Invoice_Date = Convert.ToDateTime(dataReader["InvoiceDate"]);
                                    //document.InvoiceDetails.Add(id);
                                    document.Invoice_Date = Convert.ToDateTime(dataReader["InvoiceDate"]);

                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("InvoiceAmount")))
                                {
                                    //InvoiceDetails id = new InvoiceDetails();
                                    //id.Invoice_Amt = Convert.ToString(dataReader["InvoiceAmount"]);
                                    //document.InvoiceDetails.Add(id);
                                    document.Invoice_Amt = Convert.ToString(dataReader["InvoiceAmount"]);
                                    
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ApprovedAmount")))
                                {
                                    document.ApprovedAmount = Convert.ToString(dataReader["ApprovedAmount"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("esdModifyBy")))
                                {
                                    document.ModifyDate = Convert.ToDateTime(dataReader["esdModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("esdLastAction")))
                                {
                                    document.LastAction = Convert.ToChar(dataReader["esdLastAction"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("esdModifyBy")))
                                {
                                    document.ModifyBy = Convert.ToString(dataReader["esdModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("esdCeaseDate")))
                                {
                                    document.CeaseDate = Convert.ToDateTime(dataReader["esdCeaseDate"]);
                                }

                                DocumentUploadDetails.Add(document);
                                //iCount++;
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


    }
}
