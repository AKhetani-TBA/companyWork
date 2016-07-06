<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="True" CodeBehind="AssignGroup.aspx.cs" Inherits="Administration.AssignGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .expanded-group
        {
            background: url("plugins/datatable/images/minus.jpg") no-repeat scroll left center transparent;
            padding-left: 15px !important;
        }

        .collapsed-group
        {
            background: url("plugins/datatable/images/plus.jpg") no-repeat scroll left center;
            padding-left: 15px !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        Sys.Application.add_load(BindFunctions);
        function BindFunctions() {
            $('#tblGroup').dataTable({
                "pagingType": "simple",
                "aaSorting": [],
                "initComplete": function (settings, json) {
                    $("#tblGroup").show();
                }
            });
            //$('#src').dataTable({
            //    "sPaginationType": "two_button"
            //});
            $('#tblAllImgs').dataTable({ "bLengthChange": false, "bPaginate": false }).rowGrouping({
                bExpandableGrouping: true,
                asExpandedGroups: [],
            });
            $('#tblGrpImgs').dataTable({ "bLengthChange": true, "bPaginate": false }).rowGrouping({
                bExpandableGrouping: true,
                asExpandedGroups: [],
            });

            $('#<%= usrInGrpList.ClientID%>').listbox({ 'searchbar': true });
            $('#<%= AllUserLst.ClientID%>').listbox({ 'searchbar': true });
        }

        function ResetFunc() {
            $('#ContentPlaceHolder1_lblMsgCreateGrp').text("");
            $('#ContentPlaceHolder1_lbldltFrmgrp').text("");
            $('#ContentPlaceHolder1_lblAddInGrp').text("");
            $('#ContentPlaceHolder1_lblRmvImg').text("");
            $('#ContentPlaceHolder1_lblAddImg').text("");

        }
    </script>

    <div class="row-fluid">
        <div class="span12">
            <div class="row-fluid">
                <div class="span6">
                    <div class="section-outer">
                        <div class="section-title">
                        </div>
                        <div class="section-detail">
                        </div>
                    </div>
                </div>
                <div class="span6"></div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="upCreateGroup" runat="server">
        <ContentTemplate>
            <div class="row-fluid">
                <div class="span12">
                    <div class="span5">
                        <div style="margin-left: 10px"></div>
                        <div class="span12 control-group createGrp">
                            <div class="controls">

                                <div class="heading">
                                    Create Group
                                </div>

                                <div class="span12" style="margin-left: 0px;border: 2px solid navy;">
                                    <table>
                                        <tr>
                                            <td style="width: 100%; padding: 5px!important">

                                                <asp:TextBox ID="txtGroupName" runat="server" ValidationGroup="valgroupNewsLetterCreateGroup" CssClass="chgpwd" placeHolder="Enter Group Name" />
                                                <asp:RequiredFieldValidator ID="vfieldGroupName" runat="server" ValidationGroup="valgroupNewsLetterCreateGroup" ControlToValidate="txtGroupName" ErrorMessage="Please enter Group Name" ForeColor="#FF3300">*</asp:RequiredFieldValidator>

                                                <asp:RegularExpressionValidator ID="rfieldGroupName" ValidationGroup="valgroupNewsLetterCreateGroup" runat="server" ControlToValidate="txtGroupName" ToolTip="Enter Valid Email Id"
                                                    ValidationExpression="[^\s]{1,30}$" ForeColor="#FF3300">Please enter valid Group Name</asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 100%; padding: 5px!important">
                                                <asp:TextBox ID="txtGroupDesxtiption" runat="server" ValidationGroup="valgroupNewsLetterCreateGroup" CssClass="chgpwd" placeHolder="Enter Description" />
                                                <asp:RequiredFieldValidator ID="vFiledGroupDesc" runat="server" ValidationGroup="valgroupNewsLetterCreateGroup" ControlToValidate="txtGroupDesxtiption" ForeColor="#FF3300" ErrorMessage="Please Enter Description" SetFocusOnError="True">*</asp:RequiredFieldValidator>

                                                <asp:RegularExpressionValidator ID="rFiledGroupDesc" ControlToValidate="txtGroupDesxtiption" Text="Invalid Description" runat="server" ForeColor="#FF3300" ValidationExpression="^[a-zA-z0-9-'/\s]{1,50}$" ValidationGroup="valgroupNewsLetterCreateGroup" />
                                            </td>


                                        </tr>
                                        <tr>
                                            <td style="padding: 5px!important">
                                                <asp:Button ID="btnCreateGrp" runat="server" Text="Create Group" ToolTip="Create Group" CssClass="btnClass" ValidationGroup="valgroupNewsLetterCreateGroup" OnClick="btnCreateGrp_Click" OnClientClick="ResetFunc()" />
                                                <asp:ValidationSummary ID="vfieldSmry" runat="server" ValidationGroup="valgroupNewsLetterCreateGroup" ShowMessageBox="false" ShowSummary="false" />
                                                <br />
                                                <asp:Label ID="lblMsgCreateGrp" runat="server"></asp:Label>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="span12  allGroups">

                            <div class="heading">
                                All Groups
                            </div>

                            <div class="pnl">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptrGroup" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblGroup" style="border-spacing: 0; display: none">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th style="padding-right: 5px;">Group Id
                                                                </th>
                                                                <th>Group Name
                                                                </th>
                                                                <th>Description
                                                                </th>
                                                                <th style="padding-right: 5px;"></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="margin-left: 1px; width: 10%;">
                                                            <%#Eval("GroupID")%>
                                                        </td>
                                                        <td style="padding-right: 5px; width: 30%;">
                                                            <%#Eval("NAME")%>
                                                        </td>
                                                        <td style="width: 50%;">
                                                            <%#Eval("Description")%>
                                                        </td>
                                                        <td style="padding-right: 5px;">
                                                            <asp:ImageButton ID="getGroupDetails" CommandArgument='<%#Eval("GroupID")+","+ Eval("NAME")%>' ToolTip="Get User Details" OnClick="getGropDetails" src="images/getDetails.png" runat="server" OnClientClick="ResetFunc()" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> </table>
                                                </FooterTemplate>
                                            </asp:Repeater>

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="span7 mainDiv" id="discr" runat="server">

                        <div id="grpName" class="span12">
                            <div class="heading">
                                <asp:Label ID="lblGrpName" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="span12" style="margin-top:10px">
                            <div class="UserInGrp span5 panel">



                                <div class="heading">
                                    Users In Group
                                  
                                </div>

                                <div class="offset3">
                                    <asp:ListBox ID="usrInGrpList" CssClass="userlist" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </div>
                            <div class="span1" style="text-align: center; margin-top: 18%; margin-right: 0px;">
                                <div style="margin: 5px;">
                                    <asp:Button ID="btnDltFrmGrp" runat="server" Text=">>" ToolTip="Delete User From Froup" CssClass="btnClass" Font-Size="15px" OnClick="btnDltFrmGrp_Click" OnClientClick="ResetFunc()" />
                                    <asp:Label ID="lbldltFrmgrp" runat="server" ForeColor="#FF3300"></asp:Label>

                                </div>
                                <div style="margin: 5px;">
                                    <asp:Button ID="btnAddToGrp" runat="server" Text="<<" CssClass="btnClass" ToolTip="Add User to Group" Font-Size="15px" OnClick="btnAddToGrp_Click" OnClientClick="ResetFunc()" />
                                    <asp:HiddenField ID="hdn_grp" Value="" runat="server" />
                                    <asp:Label ID="lblAddInGrp" runat="server" ForeColor="#FF3300"></asp:Label>
                                </div>
                            </div>
                            <div class="span5">

                                <div class="heading">
                                    All Users 
                                </div>

                                <div>
                                    <asp:ListBox ID="AllUserLst" CssClass="userlist" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </div>
                        </div>
                        <div class="span12">
                            <div class="span5 offset  ImagesInGrp">

                                <div class="heading">
                                    Images In Group
                                </div>

                                <div class="">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Repeater ID="rptrGrpImgs" runat="server" OnItemCommand="rptrGrpImgs_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table id="tblGrpImgs">
                                                            <thead>
                                                                <tr class="tblHdr">
                                                                    <th></th>
                                                                    <th></th>
                                                                    <th></th>

                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr id="<%#Eval("ObjectId")%>">
                                                            <td colspan="1">
                                                                <%#Eval("CategoryName")%>  
                                                            </td>
                                                            <td style="margin-left: 1px;">

                                                                <input type="checkbox" id="ImgInGrp" runat="server" value='<%#Eval("ObjectId") + " ," + Eval("CategoryName") + "," + Eval("AppName")%>' />
                                                            </td>
                                                            <td>
                                                                <%#Eval("AppName")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="span1" style="text-align: center; margin-top: 20%; margin-right: 0px;">
                                <div style="margin: 5px;">
                                    <asp:Button ID="btnRmveImgFrmGrp" runat="server" Text=">>" ToolTip="Delete User From Froup" CssClass="btnClass" Font-Size="15px" OnClick="btnRmveImgFrmGrp_Click" OnClientClick="ResetFunc()" />
                                    <asp:Label ID="lblRmvImg" runat="server" ForeColor="#FF3300"></asp:Label>

                                </div>
                                <div style="margin: 5px;">
                                    <asp:Button ID="btnAddImgToGrp" runat="server" Text="<<" CssClass="btnClass" ToolTip="Add User to Group" Font-Size="15px" OnClick="btnAddImgToGrp_Click" OnClientClick="ResetFunc()" />
                                    <asp:HiddenField ID="HiddenField1" Value="" runat="server" />
                                    <asp:Label ID="lblAddImg" runat="server" ForeColor="#FF3300"></asp:Label>
                                </div>
                            </div>

                            <div class="span5 allImages">

                                <div class="heading">
                                    AllImages In Group
                                </div>

                                <div class="">
                                    <table runat="server">
                                        <tr>
                                            <td>
                                                <asp:Repeater ID="rptrAllImgs" runat="server" OnItemCommand="rptrAllImgs_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table id="tblAllImgs">
                                                            <thead>
                                                                <tr class="tblHdr">
                                                                    <th></th>
                                                                    <th></th>
                                                                    <th></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr id='<%#Eval("sid")%>'>
                                                            <td colspan="4">

                                                                <%#Eval("CategoryName")%>
                                                            </td>

                                                            <td style="margin-left: 1px;">
                                                                <input type="checkbox" id="cb1" runat="server" value='<%#Eval("SID_GUID") + " ," + Eval("CategoryName") + "," + Eval("ApplicationName")%>' />
                                                            </td>
                                                            <td>
                                                                <%#Eval("ApplicationName")%>
                                                            </td>
                                                        </tr>

                                                    </ItemTemplate>

                                                    <FooterTemplate>
                                                        <%--<tr>
                                                                <asp:Label ID="lblFooter" runat="server" Text="Label"></asp:Label>
                                                            </tr>--%>
                                                            </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
