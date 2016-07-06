using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Administration
{

    public partial class AuditTrail : System.Web.UI.Page
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
            DataSet ds = new DataSet();

            ds = BLL.Domain.GetVendorID(Convert.ToString(CurrentSession.User.UserID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                VendorID.Value = Convert.ToString(ds.Tables[0].Rows[0]["UserGroupId"]);
            }
            else
            {
                Response.Redirect("Signout.aspx", false);
                return;
            }

            Int32 userid = Convert.ToInt32(CurrentSession.User.UserID);
            Int32 vendorid = 0;
            DataSet dt = new DataSet();
            if (Convert.ToString(HttpContext.Current.Session["VendorId"]) != "")
            {
                vendorid = Convert.ToInt32(HttpContext.Current.Session["VendorId"]);


                dt = BLL.Domain.getVendorDetails(vendorid);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    string name = Convert.ToString(dt.Tables[0].Rows[0]["CompanyName"].ToString());
                    byte[] logo = (byte[])dt.Tables[0].Rows[0]["Companylogo"];
                    string fromEmail = Convert.ToString(dt.Tables[0].Rows[0]["FromEmail"]);
                    string ccEmail = Convert.ToString(dt.Tables[0].Rows[0]["CCEmail"].ToString());
                    string signature = Convert.ToString(dt.Tables[0].Rows[0]["Signature"].ToString());

                    imgCompanyLogo.Src = "data:image/png;base64," + Convert.ToBase64String(logo);

                    lblCompanyTitle.Text = name + " Audit Trail";
                }
            }
        }
    }
}