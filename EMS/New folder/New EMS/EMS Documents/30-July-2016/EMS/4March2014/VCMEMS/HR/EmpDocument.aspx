<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpDocument.aspx.cs" Inherits="HR_EmpDocument"
    EnableEventValidation="false" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/ajax.js" type="text/javascript"></script>
<%--    <script src="../js/jquery-1.6.4.js" type="text/javascript"></script>
--%>    <script src="../js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        function chkForFileUpload()
        {
            var file = $( '#fup' );
            var fileName = file[0].value;
            //Checking for file browsed or not 
            if ( fileName == '' )
            {
                alert( "Please select a file to upload!!!" );
                file.focus();
                return false;
            }
            else
            {
                return true;
            }
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <table id="Doc" runat="server">
                    <tr>
                        <td>
                            Department :
                        </td>
                        <td>
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" Width="140px"
                                OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Employee :
                        </td>
                        <td>
                            <asp:DropDownList ID="showEmployees" runat="server" Width="150px" AutoPostBack="true"
                                CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Document Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldocType" runat="server" Width="150px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnsearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                OnClick="btnsearch_Click" CausesValidation="False" Style="height: 23px" />
                        </td>
                        <td>
                            <asp:Button ID="AddDoc" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                Font-Size="18px" Height="28px" ForeColor="#333333" Text="Add Document" CausesValidation="False"
                                OnClick="addDoc_Click" />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"
                                OnClick="btnexcel_Click" Style="height: 21px" /><br />
                        </td>
                    </tr>
                </table>
                <div id="adddocumentdiv" runat="server">
                    <fieldset style="padding: 5px; width: 750px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Document Details
                        </legend>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%">
                                    Employee<span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 15%">
                                    Document Name <span style="color: #FF0000">&nbsp;*</span>
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="showDocuments" runat="server" Width="200px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvdoc" runat="server" ErrorMessage="Please select document."
                                        InitialValue="0" ControlToValidate="showDocuments" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Select Document <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fup" runat="server" Font-Bold="False" Height="22px" class="button" />
                                    <%--  <asp:RequiredFieldValidator ID="rfvupl" runat="server" ControlToValidate="fup" ErrorMessage="Please select a file for upload"
                                Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    Date <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="dob" runat="server" Width="150px"></asp:TextBox>
                                            <asp:CalendarExtender ID="dob_CalendarExtender" runat="server" TargetControlID="dob"
                                                Format="dd MMMM yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--<asp:TextBox ID="txtStart" runat="server" Width="54%" onkeypress="NumericValue(this)"></asp:TextBox>--%>
                                    <asp:RegularExpressionValidator ID="revDate" runat="server" Display="None" ControlToValidate="dob"
                                        ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="dob" ErrorMessage="Please enter date "
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" style="vertical-align: middle">
                                    <%--<asp:Button ID="up" runat="server" Text="Upload" OnClick="up_Click" Font-Bold="False"
                                Font-Names="Tw Cen MT Condensed" Height="28px" Font-Size="18px" ForeColor="#333333" Visible="false" />--%>
                                    <asp:Button ID="upd" runat="server" OnClick="upd_Click" Text="Save" Font-Bold="False"
                                        Font-Names="Tw Cen MT Condensed" Height="28px" Font-Size="18px" ForeColor="#333333"
                                        OnClientClick="return chkForFileUpload();" />
                                    &nbsp;<asp:Button ID="cncl" runat="server" OnClick="cncl_Click" Text="Cancel" Font-Names="Tw Cen MT Condensed"
                                        Height="28px" ForeColor="#333333" Font-Size="18px" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" DisplayMode="List" />
                </div>
                <div id="DivGrd" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 600px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AutoGenerateColumns="False" OnRowDataBound="displayAll_RowDataBound"
                                OnSelectedIndexChanged="displayAll_SelectedIndexChanged" Width="100%" AllowPaging="false"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                EnableTheming="False">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                    ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee Name</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="deptName"
                                                CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="docId" HeaderText="docId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="docURL" HeaderText="docurl">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Document Title">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="docTitle"
                                                CommandName="sort" CssClass="gridlink">Document</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("DocumentName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton21" runat="server" CausesValidation="False" CommandArgument="Doc_Date"
                                                CommandName="sort" CssClass="gridlink">Date</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label55" runat="server" Text='<%# Bind("Doc_Date") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View/Download">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="hid" runat="server" Text='<%# Eval("docURL") %>' Visible="False"></asp:Label>
                                            <asp:LinkButton ID="view" runat="server" Style="font-family: 'Tw Cen MT Condensed';
                                                color: #000000;" Text="View" CausesValidation="False" Font-Size="18px"></asp:LinkButton>
                                            &nbsp;/
                                            <asp:LinkButton ID="dwn" runat="server" Style="color: #000000" CausesValidation="False"
                                                Font-Names="Tw Cen MT Condensed" Font-Size="18px">Download</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="delImg" runat="server" ImageUrl="~/images/delete.ico" Width="16px"
                                                CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the document?');" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="True">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="empId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Documents added...
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <br />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="upd" />
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
