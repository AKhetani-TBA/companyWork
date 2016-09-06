<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveCOffDetails.aspx.cs"
    Inherits="HR_LeaveCOffDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    
     <script language="javascript" type="text/javascript">
         function showlogdiv() 
        {
            var dv = document.getElementById("in_out_logs");
            dv.style.pixelLeft = event.x - 380;
            dv.style.pixelTop = event.y + 15;
        }
        function OpenPopup(strURL) 
        {
            window.open(strURL, "List", "scrollbars=yes,resizable=no,width=600,height=450,top=175,left=475");
            return false;
        }
        </script>
   
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
  <div class="EMS_font" id="mainDiv">
  <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
                <div id="searchPane" runat="server">
                <table width="100%">
                    <tr>  
              <td style="width:50%"> 
                        <fieldset>
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search  Range
                        </legend> 
                                    From Date:
                                    <asp:TextBox ID="txtFormDate" runat="server" AutoPostBack="true" 
                                        Font-Bold="True" ontextchanged="txtFormDate_TextChanged" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendaceDate" runat="server" Format="dd-MMM-yyyy" 
                                        TargetControlID="txtFormDate">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp; To Date :
                                    <asp:TextBox ID="txtTodate" runat="server" Font-Bold="True" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendancetodate" runat="server" Format="dd-MMM-yyyy" 
                                       TargetControlID="txtTodate">
                                    </asp:CalendarExtender>                                    
                                    <br />
                                    <asp:LinkButton ID="CY" runat="server" onclick="CY_Click"> Single Day </asp:LinkButton> 
                                      &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;  &nbsp;&nbsp;     &nbsp;&nbsp;    &nbsp;&nbsp; 
                                     &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp; 
                                    <asp:LinkButton ID="CM" runat="server" onclick="CM_Click"> Single Month </asp:LinkButton>
                             </fieldset>
                    </td>  
              <td style="width:50%">
                    <fieldset>
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Department / Employee / Product (Project)  wise Search
                        </legend>
                      Dept: 
                                   <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True"
                                      OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                                    </asp:DropDownList>
                    Emp:
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="false" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"   >
                                    </asp:DropDownList>
                              &nbsp;&nbsp;&nbsp;   <asp:ImageButton ID="btnSearch" runat="server" 
                                          ImageUrl="~/images/searchbtn.png" OnClick="btnSearch_Click" /> <br />
                                          <br />
                                           </fieldset>
                                           
                            </td>    
                </tr> 
                    
                    
          </table>
                </div>
                <div id="searchResults" runat="server">
                    <tr>
                        <td colspan="6" valign="top">
                        <div style="height: 1050px;width: 100%; overflow-y: auto; overflow-x: auto;" > 
                                 <asp:GridView ID="gvleave" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                HorizontalAlign="Justify" OnRowCommand="gvleave_RowCommand" OnRowDataBound="gvleave_RowDataBound"
                                Width="100%">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle CssClass="gridheader"/>
                                <Columns>
                                <asp:TemplateField HeaderText="Coff Id" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblCId" runat="server" Text='<%# Bind("CId") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect"   Width="2%"/>
                                </asp:TemplateField>     
                                
                                   <asp:BoundField DataField="COffDate" HeaderText="COff Date" DataFormatString="{0:dd/MM/yyyy}"
                                        ItemStyle-Width="6%" /> 
                                     <asp:BoundField DataField="COffDate" HeaderText="Day" DataFormatString=  "{0:dddd}"
                                        ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="EmpName" HeaderText="Employee " ItemStyle-Width="15%" />
                                   <asp:TemplateField HeaderText="Dept." >
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeptname" runat="server"   ForeColor="Black" ></asp:Label>
                                    </ItemTemplate>                                    
                                    <ItemStyle CssClass="gridlink "   Width="9%"/>
                                 </asp:TemplateField>                    
                                  <asp:BoundField DataField="DayType" HeaderText="Day " ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                                  
                                
                                     
                                  <%--       <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-Width="25%" />
                                   <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}"
                                        ItemStyle-Width="10%" />    --%>

                           <%--         <asp:TemplateField HeaderText="Intime" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblIntime" runat="server"   ForeColor="Black" ></asp:Label>
                                    </ItemTemplate>       
                                      <ControlStyle CssClass="gridlink" />
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle CssClass="gridlink "   Width="7%"/>
                                </asp:TemplateField>         --%>                                      
                              <%--    <asp:TemplateField HeaderText="Outtime" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblOuttime" runat="server"   ForeColor="Black" ></asp:Label>
                                    </ItemTemplate>           
                                     <ControlStyle CssClass="gridlink" />
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle CssClass="gridlink "   Width="7%"/>
                                </asp:TemplateField>       --%>
                                         <asp:TemplateField HeaderText="Gross Hours"  >
                                    <ItemTemplate >
                                        <asp:Label ID="lblGrossTime" runat="server" ForeColor="Black"  ></asp:Label>
                                    </ItemTemplate>          
                                      <ControlStyle CssClass="gridlink" />
                                    <HeaderStyle CssClass="gridheader" />                                                              
                                    <ItemStyle CssClass="gridlink "  HorizontalAlign="Center"  Width="5%"/>
                                </asp:TemplateField>       
                                      <asp:TemplateField HeaderText="NetOut" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblDurationOut" runat="server"   ForeColor="Black" ></asp:Label>
                                    </ItemTemplate>                            
                                       <ControlStyle CssClass="gridlink" />
                                    <HeaderStyle CssClass="gridheader" />                                           
                                    <ItemStyle CssClass="gridlink "   HorizontalAlign="Center"  Width="5%"/>
                                </asp:TemplateField>       
                                  
                               <asp:TemplateField HeaderText="NetIn" >
                                    <ItemStyle CssClass="gridlink "   Width="5%"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDurationIn" runat="server"  ForeColor="Black"  ></asp:Label>
                                    </ItemTemplate>                              
                                      <ControlStyle CssClass="gridlink" />
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle CssClass="gridlink "  HorizontalAlign="Center"  Width="5%"/>                                          
                                </asp:TemplateField>                                               
                                     <asp:BoundField DataField="Approved" HeaderText="Status" ItemStyle-Width="5%"  />
                                     <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-Width="5%"/>       
                                   
                                   <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:ImageButton ID="statusImageButton" runat="server" CausesValidation="False" Height="20px"  Width="20px" 
                                            ImageUrl="~/images/edit_btn.png" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClick="statusImageButton_Click " onmouseover="setstatusdivExtra();" />
                                        </div>
                                    </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("CId") %>'
                                                CommandName="Deleteleave"  ImageUrl="~/images/Deletings.PNG"  OnClientClick="javascript:if(!confirm('Are you sure ,you want to Delete Record?')){return false;}"  />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="2%" />
                                    </asp:TemplateField>
                                    
                                       <asp:TemplateField HeaderText="Emp Id" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("empId") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect"   Width="2%"/>
                                </asp:TemplateField>
                                
                         <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="logsImageButton" runat="server" CommandArgument='<%# Eval("empId") + "," + Eval("empName") %>'
                                                ImageUrl="~/images/clock.ico" OnClick="logsImageButton_Click" onmouseout="hidelogdiv()"
                                                onmouseover="showlogdiv();" Style="width: 18px;" Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
          
                                    <asp:TemplateField HeaderText="Logs">
                                        <ItemStyle Width="2%" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="logsImage" runat="server" ImageUrl="~/images/doc.png" Width="30px"
                                                Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                  
                                </Columns>
                                <EmptyDataTemplate>
                                    No Leave Record Found
                                </EmptyDataTemplate>
                            </asp:GridView>
       
                            <input id="hidleaveID" runat="server" type="hidden" />
                            </div>
                        </td>
                    </tr>
                </div>
                <div id="statusDivExtra" class="status_div" style="border-left: solid 3px #999999;
                            border-top: solid 1px #C0C0C0; border-right: solid 1px #C0C0C0; border-bottom: solid 3px #999999;
                            position: relative; width: 267px; height: 250px;">
                            <fieldset id="Fieldset2" style="margin-right: 5px; margin-left: 5px; margin-bottom: 5px;
                                margin-top: 5px; padding-bottom: 2px; padding-left: 2px; padding-top: 2px; padding-right: 2px;
                                width: 250px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update
                            Leave&nbsp; </legend>
                            <div>
                                    <table>
                            <tr>
                                <td>
                                    Employee:
                                </td>
                                <td>
                                       <b><asp:Label ID="lblemp" runat="server" Text="Label"></asp:Label></b>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    COff Date
                                </td>
                              <%--  <td>
                                    <asp:TextBox ID="txtCDate" runat="server" Font-Bold="True" Width="95px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCDate"
                                        Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>--%>
                                <td>
                                    <asp:Label ID="lblCDate" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Day Type
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbDayType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Full Day" Value="FD" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Half Day" Value="HD"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Comments
                                </td>
                                <td rowspan="2">
                                    <asp:TextBox ID="txtComments" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblstatus" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Approved" Value="A" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                </tr>
                                </table>
                            </div>
                           <div style="text-align: center;">
                                    <asp:Button ID="btnStatusSubmit" runat="server" Text="Save" CssClass="button" OnClick="btnStatusSubmit_Click"  />
                                    <asp:Button ID="btnClosed" runat="server" Text="Close" CssClass="button" OnClientClick="self.close();" />
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" />
                            </fieldset>
                </div>
                <div id="in_out_logs" class="logs_div">
                        <div>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblname" runat="server" Text="" ForeColor="Black"></asp:Label>
                            <asp:Label ID="logdate" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                        <fieldset id="in_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                            padding-bottom: 5px; padding-left: 5px; float: left; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Inside Details
                            </legend>
                            <asp:Label ID="lblDetailLogs" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                        <fieldset id="out_logs" style="margin-right: 5px; margin-bottom: 5px; padding-bottom: 5px;
                            padding-left: 5px; float: right; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Outside Details
                            </legend>
                            <asp:Label ID="lblOutsideDetails" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                    </div>
            </div> 
          </ContentTemplate>
</asp:UpdatePanel>
    </div> 
    </form>
</body>
</html>
