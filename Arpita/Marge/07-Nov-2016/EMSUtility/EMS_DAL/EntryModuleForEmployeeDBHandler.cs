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
                            list.Add(new EntryModuleForEmployeeBase() { SecExemID = Convert.ToInt32(reader["SecExemID"]), Head = reader["HeadName"].ToString(), Abbreviation = reader["Abbreviation"].ToString()});
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
                            list.Add(new EntryModuleForEmployeeBase() { BasisId= Convert.ToInt32(reader["BasisId"]), Basis = reader["BasisName"].ToString() });
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
            DataSet dataset = null;
            try
            {
                using (sqlConnection = new SqlConnection(this.config.EmsConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_SubmitSectionExemptionDetails", sqlConnection))
                    {
                        //EmployeeBasicDetails EmpBD;
                        //sqlCommand.CommandType = CommandType.StoredProcedure;
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_EmpId", Convert.ToInt32(EmpBD.EmployeeId)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_FinancialYearId", Convert.ToInt32(EMEBaseObj.FY)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_HeadName", Convert.ToString(EMEBaseObj.Head)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_UserDocName", Convert.ToInt32(EMEBaseObj.Basis)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_UserDocPath", Convert.ToInt32(EMEBaseObj)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_BasisId", Convert.ToInt32(DUA.Basis)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_MaximumAmt", Convert.ToDecimal(DUA.Maximum)));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyBy", DUA.ModifyBy));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_ModifyDate", DUA.ModifyDate));
                        //sqlCommand.Parameters.Add(new SqlParameter("@p_LastAction", DUA.LastAction));
                        //if (DUA.LastAction == 'D')
                        //{
                        //    sqlCommand.Parameters.Add(new SqlParameter("@p_CeaseDate", DateTime.ParseExact(DUA.CeaseDate, "dd/MM/yyyy", null)));
                        //}
                        //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        //dataset = new DataSet();
                        //sqlDataAdapter.Fill(dataset);
                        //sqlConnection.Close();
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

        public string GetEmployeeDocumentsdetails(int empId, string connStr)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return "successfully saved";
        }
    }
}
