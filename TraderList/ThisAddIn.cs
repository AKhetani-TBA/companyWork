﻿using System;
using Office = Microsoft.Office.Core;

namespace TraderList
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            Globals.ThisAddIn.Application.DisplayStatusBar = true;
            Globals.ThisAddIn.Application.StatusBar = false;
            //Globals.ThisAddIn.Application.
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
        private TraderListAddinUtil traderListAddinUtil;
        protected override object RequestComAddInAutomationService()
        {
            if (traderListAddinUtil == null)
            {
                traderListAddinUtil = new TraderListAddinUtil();
            }
            return traderListAddinUtil;
        }
        #endregion
        #endregion
    }
}
