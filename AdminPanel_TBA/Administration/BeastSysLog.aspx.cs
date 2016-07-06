using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Administration
{

    public partial class BeastSysLog : System.Web.UI.Page
    {
        private SessionInfo _session;
        public SessionInfo CurrentSession
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionInfo(HttpContext.Current.Session);
                }
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentSession.User == null)
            {
                Response.Redirect("Signout.aspx", false);
                return;
            }           
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string BeastMostUsedUsers(string FromDate, string ToDate)
        {
            try
            {
                clsDAL dbObjAutoTest = new clsDAL(false);
                DataSet tempDS = new DataSet();
                tempDS = dbObjAutoTest.BeastMostUsedUsers(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)); /* MySQL*/
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr in tempDS.Tables[0].Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in tempDS.Tables[0].Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                string json = serializer.Serialize(rows);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string getBeastUserAllActivity(string Userid, string LastSeen)
        {
            try
            {
                clsDAL dbObjAutoTest = new clsDAL(false);
                DataSet tempDS = new DataSet();
                LastSeen = LastSeen.Replace("|", " ");
                tempDS = dbObjAutoTest.GetBeastUserAllActivityFromMySql(Userid, LastSeen); /* MySQL*/
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr in tempDS.Tables[0].Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in tempDS.Tables[0].Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                string json = serializer.Serialize(rows);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetBeastSessionList(string UserID, string FromDate, string ToDate)
        {
            try
            {
                clsDAL dbObjAutoTest = new clsDAL(false);
                DataSet tempDS = new DataSet();
                tempDS = dbObjAutoTest.GetBeastSessionList(UserID, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)); /* MySQL*/
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;
                foreach (DataRow dr in tempDS.Tables[0].Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in tempDS.Tables[0].Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                string json = serializer.Serialize(rows);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}