using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Administration
{
    public partial class Signout : System.Web.UI.Page
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


            if (CurrentSession.User != null)
            {

                BLL.Domain.SubmitWebActiveLogins(Convert.ToString(CurrentSession.User.UserID), CurrentSession.User.SessionID, "", "", "");
            }



            Session.Clear();
            Response.Redirect("Login.aspx", false);
        }
    }
}