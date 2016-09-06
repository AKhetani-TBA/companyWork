<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sample.aspx.cs" Inherits="HR_sample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>

    <script src="../js/jquery.js" type="text/javascript"></script>
<script type="text/javascript">
$(function() {
$("#wrapper").mousemove(function(evt) {
    $("#message").css({
      top: evt.pageY + 5,
      left: evt.pageX + 5
  }).show();
  });
});

</script>
<style type="text/css">
html, body, div { margin: 0; padding: 0; border: 0 none; }
#wrapper { margin: 0 auto; width: 600px; }
#equipment { color: green; }
#message { display: none; position: absolute; text-align: center; padding: 10px; width: 120px; background: red; color: white; font-weight: bold; }
</style>
</head>
<body>
    <form id="form1" runat="server">
<div id="wrapper">
<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor <span id="equipment">incididunt ut labore et dolore</span> magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
<div id="message">This is a message</div>
</div>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    </form>
</body>
</html>
