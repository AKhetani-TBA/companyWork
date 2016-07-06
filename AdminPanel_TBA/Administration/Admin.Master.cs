using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class Admin : System.Web.UI.MasterPage
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
                Session.Clear();
                Session.Abandon();
                Response.Redirect("sto.aspx?RefNo=999991", false);
                return;
            }

            if (!IsPostBack)
            {
                try
                {
                    hdnUserID.Value = Convert.ToString(CurrentSession.User.UserID);
                    if(Convert.ToString(HttpContext.Current.Session["VendorId"]) == "")
                    {
                        company.Visible = true;
                        group.Visible = true;
                        user.Visible = true;
                        AutoURl.Visible = true;
                        //RapidrvAdmin.Visible = false;
                        //RapidrvMarketing.Visible = false;
                        auditTrail.Visible = false;
                    }
                    //else if (Convert.ToInt32(HttpContext.Current.Session["VendorId"]) == (int)BASE.Enums.Vendors.rapidRV)
                    //{
                    //    company.Visible = false;
                    //    group.Visible = false;
                    //    user.Visible = false;
                    //    AutoURl.Visible = false;
                    //    //RapidrvAdmin.Visible = true;
                    //    //RapidrvMarketing.Visible = true;
                    //    auditTrail.Visible = true;
                    //}               
                }
                catch (Exception ex)
                {
                    LogUtility.Error("Admin.Master.cs", "Page_Load", ex.Message, ex);
                }

            }
        }
    }
}