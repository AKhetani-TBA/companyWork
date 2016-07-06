using System;
using Office = Microsoft.Office.Core;

namespace Beast_Barclay_Addin
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
       
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
       
        }
    
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }
       
        #region COMADDIN ACCESS UTIL OBJECT
        private BarclayAddinUtil addinBarclayAddinUtil;
        protected override object RequestComAddInAutomationService()
        {
            if (addinBarclayAddinUtil == null)
            {
                addinBarclayAddinUtil = new BarclayAddinUtil();
            }
            return addinBarclayAddinUtil;
        }
        #endregion
        #endregion
    }
}
