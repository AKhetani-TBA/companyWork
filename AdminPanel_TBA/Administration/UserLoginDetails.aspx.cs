using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.Common.Log;

namespace Administration
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public partial class UserLoginDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Gets the Trumid User Login Details
        /// </summary>
        /// <param name="ServerId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetUserLoginDetails(string ServerId, string FromDate, string ToDate)
        {
            try
            {
                DataTable table = new DataTable();

                Domain domain = new Domain();
                DataSet ds = domain.GetUserLoginDetails(ServerId, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));

                table = ds.Tables[0];

                return ConvertDataTabletoString(table);

            }
            catch (Exception e)
            {
                LogUtility.Error("UserLoginDetails.cs", "GetUserLoginDetails()", e.Message, e);
                return null;
            }
        }

        /// <summary>
        /// Converts Datatable to String
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDataTabletoString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);


        }
    }
}