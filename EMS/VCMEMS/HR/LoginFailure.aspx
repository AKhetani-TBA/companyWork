<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginFailure.aspx.cs" Inherits="HR_LoginFailure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<link href="../css/ems.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <div class="EMS_font" style="font-size:20px; background-color: #F0F0F0;">
    &nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/images/icon_warning.png" 
            Height="42px" Width="46px" />
    <div>Dear, <asp:Label ID="empname" runat="server" Text=""></asp:Label>This is 
        Restricted Page for you..&nbsp;
        </div>
    
    </div>
    </form>
</body>
</html>
