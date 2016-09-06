<%@ Page Title="" Language="C#" MasterPageFile="~/HR/Main.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="HR_Default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        $(document).ready(function() {
            $("#updown").click(function() {

            $("#ctl00_ContentPlaceHolder1_maxVersion").slideToggle("fast");           
            });
            $("#updown1").click(function() {

                $("#maxVersion1").slideToggle("fast");
            });
            $("#updown2").click(function() {

                $("#maxVersion2").slideToggle("fast");
            });
        });


        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                $("#updown").click(function() {

                $("#ctl00_ContentPlaceHolder1_maxVersion").slideToggle("fast");
                });
                $("#updown1").click(function() {

                    $("#maxVersion1").slideToggle("fast");
                });
                $("#updown2").click(function() {

                    $("#maxVersion2").slideToggle("fast");
                });
            }
        }

    </script>
 <script language="javascript" type="text/javascript">
     function showlogdiv() {
         var dv = document.getElementById("in_out_logs");
         var winW, winH;
         winW = document.body.offsetWidth / 2;
         winH = document.body.offsetHeight / 2;
         dv.style.pixelLeft = winW - 100;
         dv.style.pixelTop = winH - 50;
     }
     function Colseddiv() {
          document.getElementById('maxVersion').style.display = 'none';         
     }
      </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div style="font-size: 30px; color: orange;">
                Welcome to <span style="color: #003366">V Y A P A R</span></div>
            <div style="color: orange; font-size: medium;">
                business, trade, commerce, dealing, transaction.
            </div>
            <br />
            <div style="text-align: left; padding-left: 30px; padding-right: 30px;">
                Vyapar Capital Market Partners LLC<sup>®</sup> is a brokerage firm specializing
                in over-the-counter (OTC) derivatives products requiring high levels of analytic
                knowledge, technical support, and overall market expertise.
                <br />
                <br />
                We were founded in 2005 by a team of highly-experienced financial professionals
                who saw a need for a firm with the following characteristics:<br />
                *&nbsp; Able to move quickly into new markets;
                <br />
                *&nbsp; Able to act flexibly when existing markets undergo periods of rapid evolution
                <br />
                *&nbsp; Not limited or constrained by the hierarchical structure of traditional
                companies, and
                <br />
                *&nbsp; Offering electronic and hybrid brokerage services
                <br />
                <br />
                VCM’s professionals are uniquely committed to their clients and the firm due to
                our being a partnership; and our flat organizational structure allows us to provide
                the best use of our firm’s intellectual resources in devising customized solutions
                for our clients.
                
                <br />
                   
                <br />
                 <div id="in_out_logs" class="logs_div" style="position: fixed; bottom: 40px; right:5px; z-index: 4000;">
                            <div>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblname" runat="server" Text="" ForeColor="Black"></asp:Label>
                                <asp:Label ID="logdate" runat="server" Text=""></asp:Label>
                                <span style="float:right; padding-left:1px; height:13px; width:10px" onmouseover="this.style.border = 'solid 1px Gray'; this.style.cursor='hand'" onclick="document.getElementById('in_out_logs').style.display = 'none'" onmouseout="this.style.border='0'">X</span>
                            </div>
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
                <div id="adminpopup" runat="server" class="EMS_font" style="padding-bottom: 5px;
                    padding-top: 5px; width: 680px;  text-align: left; right: 350px;
                    position: fixed; bottom: 0px; z-index: 3500; background-color: #353535;" visible="false">
                    <div id="updown" style="display: block; height: 20px; text-align: center" onmouseover="this.style.cursor='pointer'">
                        &nbsp;&nbsp;
                        <asp:Label ID="notify" runat="server" Text="" Style="color: yellow;"></asp:Label>
                    </div>
                    <div id="maxVersion" runat="server" style="margin-left: 5px; width: 670px;  height:500px;display: none;"> <%--display: none--%>
                           <table style="background-color: Gray; width: 100%; height: 20px; border-left: solid 1px #353535; border-right: solid 1px #353535; ">
                                    <tr style="height: 20px">
                                        <td style="width: 130px; text-align: left">
                                            Employee
                                        </td>
                                        <td style="width: 70px; text-align: center">
                                            Date
                                        </td>
                                        <td style="width: 70px; text-align: center">
                                            Original
                                        </td>
                                        <td style="width: 70px; text-align: center">
                                            Modified
                                        </td>
                                        <td style="text-align: left">
                                           Modified by
                                        </td>
                                        <td style="text-align: left; width: 120px;">
                                           Comments
                                        </td>
                                        <td style="width: 50px; text-align: center; vertical-align: middle">
                                           
                                        </td>
                                         <td style="width: 20px; text-align: center; vertical-align: middle">                                           
                                        </td>
                                    </tr>
                                </table>
                        <asp:DataList ID="showNotifications" runat="server" DataSourceID="SqlDataSource1"
                            OnItemDataBound="showNotifications_ItemDataBound" OnItemCommand="showNotifications_ItemCommand"
                            Width="100%">
                            <ItemTemplate>
                                <table id="notificationtable" style="background-color: white; width: 100%; height: 20px">
                                    <tr style="height: 18px">
                                        <td style="border-right: solid 1px gray; width: 130px; text-align: left">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("empName") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray; width: 70px; text-align: center">
                                            <asp:Label ID="dateOfRecord" runat="server" Text='<%# Eval("dateOfRecord") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray;  width: 70px; text-align: center">
                                            <asp:Label ID="cat" runat="server" Text='<%# Eval("workDayCategory") %>'></asp:Label>
                                        </td>
                                        <td style=" border-right: solid 1px gray;width: 70px; text-align: center">
                                            <asp:Label ID="newcat" runat="server" Text='<%# Eval("newCategory") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray; text-align: left">
                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("modifyBy") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray; text-align: left; width: 120px;">
                                            <asp:Label ID="comments" runat="server" Text='<%# Eval("comments") %>'></asp:Label>
                                        </td>
                                        <td style=" width: 50px; text-align: center; vertical-align: middle; height:20px;">
                                            <asp:ImageButton ID="confirmBtn1"  CommandName="confirmBtn" Style="border: 1px solid white;
                                                margin-top: 2px" onmouseover="this.style.border = '1px solid gray';" onmouseout="this.style.border = '1px solid white';"
                                                Text="Confirm" runat="server" ImageUrl="../images/confirm.png" />
                                            <asp:ImageButton ID="cancelBtn1" CommandName="cancelBtn" Style="border: 1px solid white;
                                                margin-top: 2px" onmouseover="this.style.border = '1px solid gray';" onmouseout="this.style.border = '1px solid white';"
                                                Text="Cancel" runat="server" ImageUrl="../images/delete1.png" />
                                            <asp:Label ID="empId" runat="server" CssClass="hideselect" Text='<%# Eval("empId") %>'></asp:Label>
                                        </td>
                                         <td style="border-right: solid 1px gray; text-align: left; width: 20px;">
                                             <asp:ImageButton ID="logsImageButton" runat="server" 
                                            CommandArgument='<%# Eval("empId") + "," + Eval("empName")+"," + Eval("dateOfRecord") %>' 
                                           ImageUrl="~/images/clock.ico" OnClick="logsImageButton_Click" 
                                            Style="width: 18px;" 
                                            Width="18px" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EMS %>"
                            SelectCommand="NotificationsGetAll" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                   <%-- <asp:Button ID="btnClosed" runat="server" Text="Closed" OnClientClick="Colseddiv();" />--%>
                </div>
                <div id="birthdaypopup" runat="server" class="EMS_font" style="padding-bottom: 5px;
                    padding-top: 5px; width: 260px; text-align: left;
                    right: 85px; position: fixed; bottom: 0px; z-index: 3500; background-color: #353535;"
                    visible="false">
                    <div id="updown1" style="display: block; height: 20px; text-align: center" onmouseover="this.style.cursor='pointer'">
                        &nbsp;&nbsp;
                        <asp:Label ID="notify1" runat="server" Text="Upcoming Birthdays" Style="color: yellow;"></asp:Label>
                    </div>
                    <div id="maxVersion1" style="margin-left: 5px; width: 250px; display: none">
                    <table style="background-color: Gray; width: 100%; height: 20px; border-left: solid 1px #353535; border-right: solid 1px #353535; ">
                                    <tr style="height: 20px">
                                        <td style="width: 120px; text-align: left">
                                            Employee
                                        </td>
                                        <td style="width: 80px; text-align: center">
                                           Birth Date
                                        </td>
                                        <td style="width: 50px; text-align: center">
                                           Age
                                        </td>
                                    </tr>
                                </table>
                      <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource2"  
                            Width="100%" onitemdatabound="DataList1_ItemDataBound">
                            <ItemTemplate>
                                <table style="background-color: white; width: 100%; height: 20px">
                                    <tr style="height: 18px">
                                        <td style="border-right: solid 1px gray; width: 120px; text-align: left">
                                            <asp:Label ID="lblempp" runat="server" Text='<%# Eval("empName") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray; width: 80px; text-align: center">
                                            <asp:Label ID="lblbday" runat="server" Text='<%# Eval("empDOB") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray;  width: 50px; text-align: center">
                                            <asp:Label ID="lblage" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>                        
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EMS %>"
                            SelectCommand="Emp_GetUpComingBday" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                </div>
                <div id="anniversaryPopup" runat="server" class="EMS_font" style="padding-bottom: 5px;
                    padding-top: 5px; width: 260px;  text-align: left;
                    right: 1035px; position: fixed; bottom: 0px; z-index: 3500; background-color: #353535;"
                    visible="false">
                    <div id="updown2" style="display: block; height: 20px; text-align: center" onmouseover="this.style.cursor='pointer'">
                        &nbsp;&nbsp;
                        <asp:Label ID="notify2" runat="server" Text="Upcoming Anniversary" Style="color: yellow;"></asp:Label>
                    </div>
                    <div id="maxVersion2" style="margin-left: 5px; width: 250px; display: none">
                        <table style="background-color: Gray; width: 100%; height: 20px; border-left: solid 1px #353535; border-right: solid 1px #353535; ">
                                    <tr style="height: 20px">
                                        <td style="width: 120px; text-align: left">
                                            Employee
                                        </td>
                                        <td style="width: 80px; text-align: center">
                                           Hire Date
                                        </td>
                                        <td style="width: 50px; text-align: center">
                                           Year
                                        </td>
                                    </tr>
                                </table>
                      <asp:DataList ID="DataList2" runat="server" DataSourceID="SqlDataSource3" Width="100%" onitemdatabound="DataList2_ItemDataBound">
                            <ItemTemplate>
                                <table style="background-color: white; width: 100%; height: 20px">
                                    <tr style="height: 18px">
                                        <td style="border-right: solid 1px gray;width: 120px; text-align: left">
                                            <asp:Label ID="lblemppp" runat="server" Text='<%# Eval("empName") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray; width: 80px; text-align: center">
                                            <asp:Label ID="emphiredate" runat="server" Text='<%# Eval("empHireDate") %>'></asp:Label>
                                        </td>
                                        <td style="border-right: solid 1px gray;  width: 50px; text-align: center">
                                            <asp:Label ID="lblyearr" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EMS %>"
                            SelectCommand="Emp_GetUpComingAnniversary" SelectCommandType="StoredProcedure">
                        </asp:SqlDataSource>
                    </div>
                </div>             
            </div>
            &nbsp;           
        </ContentTemplate>          
    </asp:UpdatePanel>
</asp:Content>
