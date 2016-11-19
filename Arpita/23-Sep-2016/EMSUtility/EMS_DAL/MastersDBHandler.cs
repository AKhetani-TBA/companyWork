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
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader() ){

                            while (dataReader.Read())
                            {
                                Role role = new Role();
                                role.RoleId = Convert.ToInt32( dataReader[0]);
                                role.RoleName = Convert.ToString( dataReader["RoleName"]);
                                role.CreatedBy = Convert.ToString( dataReader["CreatedBy"] );
                                role.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                               
                                if ( !dataReader.IsDBNull(dataReader.GetOrdinal("ModifyBy")) )
                                {
                                    role.ModifyBy = Convert.ToString(dataReader["ModifyBy"]);
                                }
                                if (!dataReader.IsDBNull(Convert.ToInt32( dataReader.GetOrdinal("ModifyDate") )))
                                {
                                    role.ModifyDate = Convert.ToDateTime(dataReader["ModifyDate"]);
                                }
                                role.LastAction = Convert.ToChar( dataReader["LastAction"] );

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
    }
}
