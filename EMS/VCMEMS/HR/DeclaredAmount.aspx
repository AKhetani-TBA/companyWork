<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeclaredAmount.aspx.cs"
    Inherits="DeclaredAmount" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 25px;
        }
        .style2
        {
            width: 110px;
        }
        .style3
        {
            width: 91px;
        }
        .style4
        {
            width: 149px;
        }
        .style5
        {
            width: 129px;
        }
    </style>

    </head>
<body onload="document.getElementById('dynamicBox').style.display=document.getElementById('txt').value; ">
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="fireEvent" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <div>
                        <div>
                            <%-- <fieldset style="padding: 5px;">
                    <legend style="margin-bottom: 2px; font-weight: normal; color: #808080;">Search </legend>--%>
                          
                                
                                    <table>
                                        <tr>
                                        <td>
                                            Department : </td>
                                        <td>
                                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" 
                                                CssClass="ddl" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                                Width="100px">
                                                <asp:ListItem>All</asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Employee : 
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" 
                                                CssClass="ddl" Width="171px" 
                                                onselectedindexchanged="showEmployees_SelectedIndexChanged">
                                                <asp:ListItem>All</asp:ListItem>
                                                
                                            </asp:DropDownList>
                                        </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                Previous Declaration Dates</td>
                                            <td>
                                                <asp:DropDownList ID="ddlPreviousDeclarationDates" runat="server" 
                                                     
                                                    onselectedindexchanged="ddlPreviousDeclarationDates_SelectedIndexChanged" 
                                                    AutoPostBack="True">
                                                    <asp:ListItem>No Date</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                
                           
                            <div id="div2">
                               <fieldset style="padding: 5px; padding-left: 6px;">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add Investment Declaration
                                    </legend>
                                     With Effect From :
                                    <asp:TextBox ID="showYear" runat="server" AutoPostBack="True" 
                                        ontextchanged="showYear_TextChanged" Width="120px"></asp:TextBox>
                                    <asp:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
                                        Enabled="True" Format="dd MMMM yyyy" TargetControlID="showYear">
                                    </asp:CalendarExtender>
                                    </fieldset> 
                                <fieldset style="padding: 5px; padding-left: 6px;">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">INCOME TAX 
                                        INVESTMENT SELF DECLARATION FORM FOR THE FINANCIAL YEAR
                                        <asp:Label ID="yearLabel" runat="server" Text="Label"></asp:Label>
                                    </legend>
                                    <%--    <fieldset style="padding: 5px; width :340px;float:left;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;float: left">ESIC</legend>--%>
                        <div id="empDetails">
                        <table  style="width: 564px;">
                        <tr>
                        <td class="style2">Declaration Month</td>
                        <td class="style1" >
                            <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                            </td>
                        <td class="style3">Declaration No.</td>
                        <td class="style4">
                            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="2">Full Name</td>
                        <td colspan="2">
                            <asp:Label ID="empName" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                        <tr>
                        <td colspan="2">Designation</td>
                        <td colspan="2">
                            <asp:Label ID="empDesignation" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                        <tr>
                        <td colspan="2">Pan No.</td>
                        <td colspan="2">
                            <asp:Label ID="empPan" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                        <tr>
                        <td colspan="2">Mobile No.</td>
                        <td colspan="2">
                            <asp:Label ID="empMobile" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                        <tr>
                        <td colspan="2">Email</td>
                        <td colspan="2">
                            <asp:Label ID="empEmail" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                        <tr>
                        <td colspan="2">Residential Address</td>
                        <td colspan="2">
                            <asp:Label ID="empAddress" runat="server" Text=""></asp:Label>
                            </td>
                       
                        </tr>
                       
                            
                        </table>
                        </div>
                                    <div style="text-align: left; vertical-align: top; width: 650px; height: 100%;float:left;">
                                        <asp:DataList ID="showSections" runat="server" DataKeyField="sectionId" OnItemDataBound="showSections_ItemDataBound"
                                            RepeatColumns="1" width="703px"
                                            OnItemCommand="showSections_ItemCommand" 
                                            onselectedindexchanged="showSections_SelectedIndexChanged">
                                            <ItemTemplate>
                                                <fieldset style="padding: 5px; margin-right: 5px; width:auto; height: auto; vertical-align: top;
                                                    float: left; margin-left: 0px;">
                                                    <legend runat="server" id="genericLegend" style="margin-bottom: 4px; padding:5px; font-weight: bolder;
                                                        color: #808080;">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("sectionName") %>'></asp:Label>
                                                    </legend>
                                                    <%--<asp:Label ID="Label2" runat="server"  Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                                    <table style="width:600px" >
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:GridView ID="showRules" runat="server" AutoGenerateColumns="False" 
                                                                    DataKeyNames="sectionDetailId" OnRowCommand="showRules_RowCommand" 
                                                                    OnRowDataBound="showRules_RowDataBound" 
                                                                    OnSelectedIndexChanged="showRules_SelectedIndexChanged" PageSize="20" 
                                                                    width="600px">
                                                                    <RowStyle BorderColor="#333333" />
                                                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                                    <Columns>
                                                                    
                                                                        <asp:BoundField ControlStyle-CssClass="hideselect" DataField="sectionId" 
                                                                            HeaderText="SectionId">
                                                                            <ControlStyle CssClass="hideselect" />
                                                                            <FooterStyle CssClass="hideselect" />
                                                                            <HeaderStyle CssClass="hideselect" />
                                                                            <ItemStyle CssClass="hideselect" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField ControlStyle-CssClass="hideselect" DataField="sectionDetailId" 
                                                                            HeaderText="SectionDetail">
                                                                            <ControlStyle CssClass="hideselect" />
                                                                            <FooterStyle CssClass="hideselect" />
                                                                            <HeaderStyle CssClass="hideselect" />
                                                                            <ItemStyle CssClass="hideselect" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="subSectionName" HeaderText="Section Name" />
                                                                        <asp:CommandField ShowSelectButton="True">
                                                                            <ControlStyle CssClass="hideselect" />
                                                                            <FooterStyle CssClass="hideselect" />
                                                                            <HeaderStyle CssClass="hideselect" />
                                                                            <ItemStyle CssClass="hideselect" />
                                                                        </asp:CommandField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%--<label id="i" >hi</label>--%>
                                                                                <div onclick="//showText(this.childNodes[0]);//showText(this.childNodes[0]);" 
                                                                                    style="width:140px;background-color:white;height:20px;">
                                                                                    <%-- <asp:Label ID="amountLabel" onclick="showText(this);" ClientIDMode="Static" runat="server" width="120px" Text="" BackColor="AliceBlue"></asp:Label>--%>
                                                                                    <asp:TextBox ID="amountText"  runat="server" ClientIDMode="Static" onKeyUp="updateSum(this);"
                                                                                        onBlur="update(this);showLabel(this);updateSum(this);" onclick="showText(this);updateSum(this);" 
                                                                                        style="border:none;float:left;padding-left:6px;display:block;" tyle="float:right;" width="134px"></asp:TextBox>
                                                                                    <%-- <img runat="server" id="img" style="height:16px;width:16px;float:left;display:none" onclick="showLabel(this);" src="..\images\delete.ico" />--%>
                                                                                    <%-- <asp:Label ID="hideEmpId" Text='<%# Session["sectionId"] %>' ClientIDMode="Static" runat="server" width="120px" ></asp:Label>--%>
                                                                                    <asp:HiddenField ID="hideSectionDetailId" runat="server" 
                                                                                        Value='<%# Bind("sectionDetailId") %>' />
                                                                                    <asp:HiddenField ID="hidEmpId" runat="server" 
                                                                                        Value='<%# Session["tempEmpId"] %>' />
                                                                                    <asp:HiddenField ID="year" runat="server" 
                                                                                        Value='<%# Session["wef"] %>' />
                                                                                    <asp:HiddenField ID="down" runat="server" Value='<%# Bind("downLimit") %>' />
                                                                                     <asp:HiddenField ID="up" runat="server" Value='<%# Bind("upLimit") %>' />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="16px" />
                                                                        </asp:TemplateField>
                                                                       <%-- <asp:BoundField  DataField="downLimit" 
                                                                            HeaderText="down"/>
                                                                         <asp:BoundField  DataField="upLimit" 
                                                                            HeaderText="down"/>--%>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        Details not Added...
                                                                    </EmptyDataTemplate>
                                                                    <HeaderStyle BackColor="#959595" CssClass="hideselect" 
                                                                        Font-Names="Tw Cen MT Condensed" Font-Size="17px" ForeColor="White" 
                                                                        Height="19px" HorizontalAlign="Left" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:190px;border-right:1px solid silver;font-weight:bold">
                                                                Max Amount Applicable
                                                                </td>
                                                            <td style="width:130px;border-right:1px solid silver">
                                                            <asp:Label ID="Label5" runat="server" style="font-weight:bold" Text='<%# Bind("sectionLimit") %>'></asp:Label>
                                                            
                                                               
                                                                
                                                            </td>
                                                            <td style="width:136px;border-right:1px solid silver;font-weight:bold">
                                                                Total</td>
                                                            <td style="width:144px;">
                                                                <asp:Label ID="tempSum" runat="server" style="font-weight:bold" Text=""></asp:Label>
                                                                 
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                
                                            </ItemTemplate>
                                            
                                        </asp:DataList>
                                    </div>
                                    <div id="errMsg" style="margin-top:15px;background-color:#FAFAD2;width:auto;height:auto;float:left;display:none;border:1px solid 	#E9967A;color:#2F4F4F;padding:3px 3px 3px 3px">Amount Not elegible</div>
                                    <%--  </fieldset> --%>
                                </fieldset>
                                <div id="dynamicBox" style="position:relative;display:none;">
                                <input type="button" value="ok" onClick="document.getElementById('dynamicBox').style.display='none';"/>
                                <input type="button" value="close" onClick="document.getElementById('dynamicBox').style.display='none';"/>
                                <asp:TextBox ID="dynamicText" ClientIDMode="Static" runat="server" ></asp:TextBox>
                                </div>
                            </div>
                            <div id="div1">
                                <asp:Button ID="fireEvent" Style="display: none" OnClick="EvenHandler" runat="server"
                                    Text="Button" />
                                   <asp:HiddenField ID="hdfSlabDetId" runat="server" />
                                     <asp:HiddenField ID="txt" runat="server" Value="none"/>
                                <script language="javascript">
                                    function HandleEvent(slabId) {
                                        document.getElementById("txt").value = "block";
                                        var t = document.getElementById('<%= fireEvent.ClientID %>');
                                        t.value = slabId;
                                        document.getElementById('<%= hdfSlabDetId.ClientID %>').value = slabId;
                                        
                                        t.click();
                                        //alert(t.value);
                                    }
    
                                </script>

                            </div>
                           
                            &nbsp;</div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
