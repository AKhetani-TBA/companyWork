<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        Session["search"] = "";
        Session["deptId"] = "";
        Session["id"] = "";
        Session["empId"] = "";
        Session["uname"] = "";
        Session["bankId"] = "";
        Session["op"] = "";
        Session["relId"] = "";
        Session["newEmp"] = "";
        Session["empAttId"] = "";
        Session["packageEmpId"] = "";
        Session["isNewPackage"] = "";
        Session["packageID"] = "";
        Session["empBonusId"] = "";
        Session["empIdForRight"] = "";
        Session["usertype"] = "";
        Session["EmpIdentity"] = "";
        Session["EmpIDD"] = "";
        Session["EmpFullName"] = "";
      
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
