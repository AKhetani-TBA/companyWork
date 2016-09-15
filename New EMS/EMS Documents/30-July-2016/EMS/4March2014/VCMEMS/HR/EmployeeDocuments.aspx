<%@ Page Title="" Language="C#" MasterPageFile="~/HR/Main.master" AutoEventWireup="true"
    CodeFile="EmployeeDocuments.aspx.cs" Inherits="HR_EmployeeDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
    <script language ="javascript" >
        function setheight() {
            
            document.getElementById('myframe').style.height = document.getElementById('myframe').document.body.scrollHeight - 300  + "px";

        }
    </script>
    <iframe onload ="setheight();" id="myframe" src="EmpDocument.aspx" frameborder="0" scrolling='no' filter: 
                chroma(color="#FFFFFF"); allowtransparency="true" style="width:100%;">
    </iframe>
   
    </div>
</asp:Content>
