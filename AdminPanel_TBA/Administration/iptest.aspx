<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iptest.aspx.cs" Inherits="Administration.iptest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iptest</title>
    <script src="js/jquery-1.9.1.js"></script>
    <style type="text/css">
        table {
            border-collapse: collapse;
            font-family: Calibri;
            font-size: 12px;
        }

        td {
            border: 1px solid black;
            padding: 5px;
        }

        .title {
            background-color: #7D7F94;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblInfotbl" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
