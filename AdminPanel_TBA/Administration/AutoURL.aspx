<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/Admin.Master" CodeBehind="AutoURL.aspx.cs" Inherits="Administration.AutoURL" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .backg
        {
            background-color: lightblue;
        }

        .bgcolor
        {
            /*background: linear-gradient(to bottom, rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%);*/
            background: #36648B !important;
            color: White;
        }

        .hover
        {
            /* background: linear-gradient(to bottom, rgba(225,255,255,1) 0%,rgba(225,255,255,1) 7%,rgba(225,255,255,1) 12%,rgba(253,255,255,1) 12%,rgba(230,248,253,1) 30%,rgba(200,238,251,1) 54%,rgba(190,228,248,1) 75%,rgba(177,216,245,1) 100%); /* W3C */
            background-color: #3299CC;
            color: #fff;
            cursor: pointer;
        }


        .login-mid
        {
            background: linear-gradient(to bottom, rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%);
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin: 0;
        }

        .unselected
        {
            /* background-image: url('Images/AppSelection.png'); */
            background-repeat: no-repeat;
            background-position: bottom;
            margin: 0px;
            padding: 0px;
            height: 14px;
            width: 14px;
            overflow: hidden;
        }

        .selected
        {
            background-repeat: no-repeat;
            background-position: top;
            margin: 0px;
            padding: 0px;
            height: 18px;
            width: 18px;
            overflow: hidden;
        }

        ul li
        {
            list-style: none !important;
            margin-bottom: 4px;
        }

        ul
        {
            margin-left: 1px !important;
        }

        #tblCategory tr
        {
            margin-bottom: 4px !important;
            padding-bottom: 4px !important;
        }
    </style>

    <script type="text/javascript">
        function BindFunctions() {
            var sampleData = new Array();
            $("input[type='checkbox']").change(function () {

                if (this.className == "CheckAll") {
                    if (this.checked) {
                        var vdata = $(this).val();
                        sampleData.push(vdata);
                        var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                        txtMailId.value = sampleData;
                        var data = vdata.split(',')[0].trim();
                        document.getElementById(data).className = "backg";
                    }
                    else {
                        var vdata = $(this).val();
                        var ipop = sampleData.indexOf(vdata);
                        var newData1 = sampleData.slice(0, ipop);
                        var ipopNew = ipop + 1;
                        var newData2 = sampleData.slice(ipopNew, sampleData.length);
                        sampleData = newData1.concat(newData2);
                        var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                        txtMailId.value = sampleData;
                        $(this).parent().parent().removeClass("backg");
                    }
                }
            });
            $('#ChkAllCheck').click(function () {

                if ($(this).is(":checked")) {
                    $('input[type=checkbox]').each(function () {
                        if (this.className == "CheckAll") {
                            if (!this.checked) {

                                $(this).prop('checked', true);
                                var vdata = $(this).val();
                                sampleData.push(vdata);
                            }
                        }
                    });
                    var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                    txtMailId.value = sampleData;
                }
                else {
                    $('input[type=checkbox]').each(function () {
                        if (this.className == "CheckAll") {
                            $(this).prop('checked', false);
                            var vdata = $(this).val();
                            var ipop = sampleData.indexOf(vdata);
                            var newData1 = sampleData.slice(0, ipop);
                            var ipopNew = ipop + 1;
                            var newData2 = sampleData.slice(ipopNew, sampleData.length);
                            sampleData = newData1.concat(newData2);

                        }
                    });
                    var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                    txtMailId.value = sampleData;
                }
            });
            $('#loading').show();
            $('#tblAutoURL').dataTable({
                "pagingType": "simple",
                "iDisplayLength": 25,
                "aaSorting": [],
                "initComplete": function (settings, json) {
                    $("#tblAutoURL").show();
                    $('#loading').hide();
                },
                "fnDrawCallback": function () {
                    $(".paginate_enabled_previous").bind('click', pageChanged);
                    $(".paginate_enabled_next").bind('click', pageChanged);
                }

            });

            function pageChanged() {
                $('input[type=checkbox]').each(function () {
                    if (this.className == "CheckAll") {
                        if (this.checked) {
                        }
                        else {
                            $('#ChkAllCheck').prop("checked", false);
                            return;
                        }
                        $('#ChkAllCheck').prop("checked", true);
                    }
                });
                //   $('#ctl00_ContentPlaceHolder1_rptrUsers_ctl00_ChkAllCheck').prop("checked", false);            
            }
            //        pageChanged(); 

            var GlobalID = 0;
            function GetCalcList() {

                var calcList = "";
                if (GlobalID == 0) {
                    $('#Calclist ul').each(function () {
                        $(this).find('li').each(function () {
                            if ($(this).find('img').hasClass("selected")) {
                                //                                $(this).find('img').removeClass("selected");
                                //                                $(this).find('img').addClass("unselected");
                                calcList = calcList + $(this).find('img').attr('id') + ',';
                            }
                        });
                    });
                }
                else {
                    var flag = false;
                    $('#tblGroup').each(function () {
                        $(this).find('li').each(function () {
                            if ($(this).find('img').hasClass("selected")) {
                                flag = true;
                            }
                        });
                    });
                    if (flag == true) {
                        calcList = "";
                        $('#grpdivlst ul').each(function () {
                            $(this).find('li:visible').each(function () {
                                calcList = calcList + $(this).attr('id') + ',';
                            });
                        });
                    }
                }
                document.getElementById('<%= CalcList.ClientID %>').value = calcList;
            }

            $('.CalcList').click(function () {
                GetCalcList();

                return true;
            });

            $('#<%=btnExistingGroup.ClientID%>').on("click", function () {
                $('#tblCategory tbody').each(function () {
                    $(this).find('td').each(function () {
                        if ($(this).hasClass("bgcolor")) {

                            $(this).removeClass("bgcolor");
                            $(this).find('img').removeClass("selected");
                            $(this).find('img').addClass("unselected");
                            $(this).find("#imgSelc").attr('src', 'images/UnSelected.png');
                            $("#Calclist").empty();
                        }
                    });
                });
                GetUserAppGroup();
                return false;
            });

            $('#<%=btnSaveGroup.ClientID%>').on("click", function () {
                $('#tblGroup').each(function () {
                    $(this).find('li').each(function () {
                        if ($(this).hasClass("bgcolor")) {
                            $(this).removeClass("bgcolor");
                            $(this).find('img').removeClass("selected");
                            $(this).find('img').addClass("unselected");
                            $(this).find("img").attr('src', 'images/UnSelected.png');

                            $("#grpdivlst").empty();
                        }

                    });
                });
                SubmiUserAppGroup();
                return false;
            });


            function GetUserAppGroup() {
                GlobalID = 1;
                $('#GroupTr').show();
                $('#NewCategory').hide();
                return false;
            }

            function SubmiUserAppGroup() {

                $('#GroupTr').hide();
                $('#NewCategory').show();
                if (GlobalID == 1) {
                    GlobalID = 0;
                    return false;
                }
                GlobalID = 0;
                var groupName = document.getElementById('<%= txtGrpName.ClientID %>').value;
                if (groupName == "") {
                    alert("Please Enter Group Name !");
                    return false;
                }

                if (/[^A-Za-z0-9]/.test(groupName)) {
                    alert('Group Name should not contain special charaters and spaces !');
                    return false;
                }
                var CalcID = "";

                $('#Calclist ul').each(function () {
                    $(this).find('li').each(function () {
                        if ($(this).find('img').hasClass('selected')) {
                            CalcID = CalcID + $(this).find('img').attr('id') + ',';
                        }
                    });

                });
                if (CalcID == "") {
                    alert("Please Select Calculator !");
                    return false;
                }
                var param = '{"GroupName":"' + document.getElementById('<%= txtGrpName.ClientID %>').value + '" ,"UserId":"' + document.getElementById('<%= hdnUserID.ClientID %>').value + '" ,"CalcID":"' + CalcID + '"}';
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: param,
                    url: "AutoURL.aspx/SubmiUserAppGroup",
                    success: function (data) {

                        alert(data.d.split(',')[0]);

                        if (data.d.split(',')[0] == 'Data Save Successfully !!!') {
                            if ($('#tblGroup').length > 0) {
                                var html = "<li  class=\"Pointer\" onclick=\"changeImageGroup(this);\"> <img id=\"imgSelc\"  style=\"height: 14px !important; width: 14px !important;\" class='" + data.d.split(',')[1] + " unselected' name='GrpName'  src=\"images/UnSelected.png\" />";
                                html = html + "<label ID=\"lblGroupName\" class=\"name\" style=\"display: inline-block;\">" + document.getElementById('<%= txtGrpName.ClientID %>').value + "</li>";
                                $('#tblGroup li:last').after(html);
                            }
                            else {
                                location.reload(true);
                            }
                            document.getElementById('<%= txtGrpName.ClientID %>').value = "";
                            var desc = false;
                            sortUnorderedList("tblGroup", desc);
                            $('#Calclist ul').each(function () {
                                $(this).find('li').each(function () {
                                    if ($(this).find('img').hasClass("selected")) {
                                        $(this).find('img').removeClass("selected");
                                        $(this).find('img').addClass("unselected");
                                        $(this).find("img").attr('src', 'images/UnSelected.png');
                                    }
                                });
                            });
                        }
                    },
                    error: function (request, status, error) {
                    }
                });
                return true;
            }


            $('#tblCategory tbody tr ').hover(
                function () {
                    var html = "<img style='cursor: pointer;' id='test' src='images/ExpandCollapse.png'/></td>";
                    $(this).find('td').eq(1).append(html);
                    $(this).addClass('hover');

                },
               function () {
                   $(this).find('td').eq(1).find('img').remove();
                   $(this).removeClass('hover');
                   //   $(this).removeClass('hover');
               }
            );

            $('#Calclist').on("mouseenter", "li", function () {
                $(this).addClass('hover');

            });

            $('#Calclist').on("mouseleave", "li", function () {

                $(this).removeClass('hover');
            })

            /******************************************************************************************/
            $('#tblCategory tbody tr').on("click", function () {

                $('#tblCategory tbody tr ').each(function () {

                    $(this).find('td').removeClass('bgcolor');
                });
                var CategoryID = $(this).find('td').eq(1).attr('class');
                var param = '{"CategoryId":"' + CategoryID + '" ,"UserId":"' + document.getElementById('<%= hdnUserID.ClientID %>').value + '"}';
                var id = "Category_" + CategoryID;
                $(this).find('td').addClass('bgcolor');
                $("#Calclist ul").each(function () {
                    $(this).hide();
                });
                if ($('#' + id).length > 0) {
                    $('#' + id).show();
                    // element exists...
                }
                else {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: param,
                        url: "AutoURL.aspx/GetSubMenuCategory",
                        success: function (data) {
                            var listHtml = "";
                            listHtml = " <ul id=" + id + ">";
                            data = eval("(" + data.d + ")");
                            $.each(data.Table, function (key, value) {
                                listHtml += " <li  onClick=\"changeImage(this)\" class=\"Pointer\">";
                                listHtml += "<img style=\"height: 18px !important; width: 18px !important;\" class=\"unselected\" id=" + value.AppId + " name=\"image1\" src=\"images/UnSelected.png\" />";
                                listHtml += "&nbsp;&nbsp;" + value.AppTitle;
                                //listHtml += "<input style='margin:4px 0 5px' class='chk' type='checkbox' id=" + value.AppId + ">&nbsp" + value.AppTitle;
                                listHtml += "</li>";
                            });

                            listHtml += " </ul>";
                            $("#Calclist").append(listHtml);

                            //                            var asd = document.getElementsByName("image1");

                            //                            for (var i = 0; i < asd.length; i++) {
                            //                                asd[i].src = "images/UnSelected.png";
                            //                            }
                        },
                        error: function (request, status, error) {
                        }
                    });
                }
            });

            /****************************************************************************************/


            $('#tblGroup').on("mouseenter", "li", function () {
                var html = "<img style='cursor: pointer; float:right; padding-top:4px' id='test' src='images/ExpandCollapse.png'/></td>";
                $(this).append(html);
                $(this).addClass('hover');
            });

            $('#tblGroup').on("mouseleave", "li", function () {
                $(this).find('#test').remove();
                $(this).removeClass('hover');
            })
            $('#tblGroup').on("click", "li", function () {

                $('#tblGroup').each(function () {

                    // $(this).find('td').eq(1).find('img').remove();
                    $(this).find('li').removeClass('bgcolor');
                    $(this).find('li').find("#imgSelc").attr('src', 'images/UnSelected.png');
                });
                var grpid1 = $(this).find('#lblGroupName').text();
                var grpid = grpid1.trim();

                var param = '{"GroupName":"' + grpid + '" ,"UserId":"' + document.getElementById('<%= hdnUserID.ClientID %>').value + '"}';

                $(this).addClass('bgcolor');
                $(this).find("#imgSelc").attr('src', 'images/Selected.png');
                $("#grpdivlst ul").each(function () {
                    $(this).hide();
                });
                if ($('#' + grpid).length > 0) {
                    $('#' + grpid).show();
                    // element exists...
                }
                else {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: param,
                        url: "AutoURL.aspx/GetAppGroup",
                        success: function (data) {

                            var listHtml = "";
                            listHtml = " <ul id=" + grpid + ">";
                            data = eval("(" + data.d + ")");
                            $.each(data.Table, function (key, value) {
                                listHtml += " <li id=" + value.AppId + ">";
                                listHtml += value.AppTitle;
                                listHtml += "</li>";
                            });
                            listHtml += " </ul>";
                            $("#grpdivlst").add("");
                            $("#grpdivlst").append(listHtml);


                        },
                        error: function (request, status, error) {
                        }
                    });
                }

            });
            $('#ContentPlaceHolder1_btnSaveGroup').click(function () {
                if ($('#<%=txtGrpName%>').val() == '') {
                    alert('Please Enter Group Name');
                    return false;
                }

            });
        }
        function changeImage(d) {
            if ($(d).find("img").hasClass("unselected")) {
                $(d).find("img").removeClass("unselected");
                $(d).find("img").attr('src', 'images/Selected.png');
                $(d).find("img").addClass("selected");
            }
            else {
                $(d).find("img").removeClass("selected");
                $(d).find("img").addClass("unselected");
                $(d).find("img").attr('src', 'images/UnSelected.png');
            }
        }

        function changeImageGroup(d) {
            $('#tblGroup').each(function () {
                $(this).find('li').each(function () {
                    if ($(this).find('img').hasClass("selected")) {
                        $(this).find('img').removeClass("selected");
                        $(this).find('img').addClass("unselected");
                        $(this).find("#imgSelc").attr('src', 'images/Selected.png');
                    }
                });
            });

            $(d).find("#imgSelc").removeClass("unselected");
            $(d).find("#imgSelc").addClass("selected");
            $(d).find("#imgSelc").attr('src', 'images/UnSelected.png');
        }

        function sortUnorderedList(ul, sortDescending) {
            var ul = $('ul#tblGroup'),
                    li = ul.children('li');
            li.sort(function (a, b) {
                return alphabetical($(a).find("#lblGroupName").text(), $(b).find("#lblGroupName").text());
            });
            ul.append(li);
        }
        function alphabetical(a, b) {

            a = a.toLowerCase();
            b = b.toLowerCase();
            var reA = /[^a-zA-Z0-9]/g;
            var reN = /[^0-9]/g;
            var aA = a.replace(reA, "");
            var bA = b.replace(reA, "");
            return aA > bA ? 1 : -1;

        }
        function CheckboxSelection() {

            var txtMailId = document.getElementById('<%=hdnMailId.ClientID%>');
            console.log("e1");
            if (txtMailId.value == "") {

                document.getElementById('<%=lblMessage.ClientID %>').innerText = "select atleast one user to send AutoURL.";
                //                document.getElementById('<%=lblMessage.ClientID %>').style.display = "block";

                return false;
            }
        }
        function SelectionChange() {
            var vDDL = $('#<%=drpType.ClientID%> option:selected');
            var dpvalue = vDDL.text();


            if (dpvalue == "Excel") {
                vDDL.parent().parent().parent().css('background-color', '#CAE1FF');
            }

            else if (dpvalue == "Web") {
                vDDL.parent().parent().parent().css('background-color', '#FFF8DC');
            }
            else if (dpvalue == "Beast") {
                vDDL.parent().parent().parent().css('background-color', '#DFFFA5');
            }
            else
                vDDL.parent().parent().parent().css('background-color', 'rgb(230, 235, 223)');
        }
        function ChangeGroup() {

            var x = $('#<%=hdnDPStats.ClientID %>').val();
            if (x == "Disabled") {
                var validationResults = confirm("You want to change the group. It will assign/change permission of selected user(s) with selected value due you want to proceed");

                if (validationResults == false) {
                    return false;
                }
                else {
                    $('#<%=ddGroup.ClientID %>').attr("disabled", false);
                    $('#<%=hdnDPStats.ClientID %>').val("Enabled");
                }
            } else {
                $('#<%=ddGroup.ClientID %>').attr("disabled", true);
                $('#<%=hdnDPStats.ClientID %>').val("Disabled");
            }
        }

        function AutoUrlSelectionChange() {

            var vDDL = $('#<%=drpAutoURL.ClientID%>');

            var dpvalue = vDDL.val();

            if (dpvalue == "Excel") {
                document.getElementById('<%=DrpPackage.ClientID %>').style.display = "inline-block";
                document.getElementById('<%=lblPackage.ClientID %>').style.display = "inline-block";
                vDDL.parent().parent().css('background-color', '#CAE1FF');
            }
            else {
                document.getElementById('<%=DrpPackage.ClientID %>').style.display = "none";
                document.getElementById('<%=lblPackage.ClientID %>').style.display = "none";

                if (dpvalue == "Web") {
                    vDDL.parent().parent().css('background-color', '#FFF8DC');
                }
                else if (dpvalue == "Beast") {
                    vDDL.parent().parent().css('background-color', '#DFFFA5');
                }
            }
        }

        function CheckboxSelection() {
            var txtMailId = document.getElementById('<%=hdnMailId.ClientID%>');
            if (txtMailId.value == "") {
                document.getElementById('<%=lblMessage.ClientID %>').innerText = "select atleast one user to send AutoURL.";
                document.getElementById('<%=lblMessage.ClientID %>').style.display = "block";

                return false;
            }
        }
    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <%-- <Services>
            <asp:ServiceReference Path="~/openf2.asmx" />
        </Services>--%>
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upAutoUrl" runat="server">

        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
            </script>
            <table id="tblSelect" runat="server" border="0" style="height: 10px; vertical-align: middle; width: 90%; margin-top: 10px"
                align="center">
                <tr>

                    <td style="border-top: 5px solid Navy; border-bottom: 2px solid Navy;" colspan="2">
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 20%;">
                                    <img runat="server" id="imgCompanyLogo" alt="" src="" height="50" width="170" />
                                </td>
                                <td align="right" style="width: 80%" class="AutoUrl_CompanyTitle">
                                    <asp:Label ID="lblCompanyTitle" runat="server" Text="Auto Url Panel" Font-Bold="True" Font-Size="18px" ForeColor="#000066"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 55%;" align="left" valign="top">
                        <div style="border: 2px solid Navy; margin-right: 5px;" align="left" valign="top">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    AUTO URL Settings
                                </div>
                            </div>
                            <div class="panel-success">
                                <table id="tblsearch" class="AutoUrl_Table" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td valign="middle" style="text-align: right; width: 18%;">URL expire after :
                                        </td>
                                        <td valign="middle" style="width: 80%;">
                                            <asp:DropDownList ID="drpExpireHours" runat="server" Width="170px">
                                                <asp:ListItem Value="30" Text="30 minutes"></asp:ListItem>
                                                <asp:ListItem Value="60" Text="1 hour"></asp:ListItem>
                                                <asp:ListItem Value="120" Text="2 hours"></asp:ListItem>
                                                <asp:ListItem Value="180" Text="3 hours"></asp:ListItem>
                                                <asp:ListItem Value="360" Text="6 hours"></asp:ListItem>
                                                <asp:ListItem Value="540" Text="9 hours"></asp:ListItem>
                                                <asp:ListItem Value="720" Text="12 hours"></asp:ListItem>
                                                <asp:ListItem Value="1440" Text="24 hours (1 Day)"></asp:ListItem>
                                                <asp:ListItem Value="2880" Text="48 hours (2 days)"></asp:ListItem>
                                                <asp:ListItem Value="4320" Text="72 hours (3 days)"></asp:ListItem>
                                                <asp:ListItem Value="7200" Text="5 days"></asp:ListItem>
                                                <asp:ListItem Value="10080" Text="1 week"></asp:ListItem>
                                                <asp:ListItem Value="20160" Text="2 weeks"  Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="43200" Text="1 month"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td valign="middle" style="text-align: right; width: 18%">Auto URL :
                                        </td>
                                        <td valign="middle" style="width: 80%;">
                                            <asp:DropDownList ID="drpAutoURL" runat="server" Width="170px" onchange="AutoUrlSelectionChange();">
                                                <asp:ListItem Value="Web" Text="Web"></asp:ListItem>
                                                <asp:ListItem Value="Excel" Text="Excel"></asp:ListItem>
                                                <asp:ListItem Value="Beast" Text="Beast"></asp:ListItem>
                                                <asp:ListItem Value="Launcher" Text="Launcher"></asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:Label ID="lblPackage" Text="Package" runat="server" Style="display: none"></asp:Label>

                                            <asp:DropDownList ID="DrpPackage" runat="server" Width="267px" Style="display: none">
                                                <%--    <asp:ListItem Value="1077E9A75B" Text="6.11.00(w/o Custom workbook)"></asp:ListItem>
                                                    <asp:ListItem Value="ED815EB13D" Text="6.11.00"></asp:ListItem>
                                                    <asp:ListItem Value="33E66D048E" Text="6.14.00"></asp:ListItem>
                                                    <asp:ListItem Value="3072ADBC01" Text="7.04.00"></asp:ListItem>
                                                    <asp:ListItem Value="FC2B0E8FA2" Text="7.07.04"></asp:ListItem>
                                                    <asp:ListItem Value="4663E25815" Text="7.07.05"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 18%">
                                            <asp:Label ID="lblGroup" Text="Select Group :" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" style="width: 80%;">
                                            <asp:DropDownList ID="ddGroup" runat="server" Width="170px" disabled='disabled'></asp:DropDownList>

                                            <input type="button" id="chngGroup" value="Change" class="btnClass" onclick="ChangeGroup();" style="margin-left: 10%" />
                                            <asp:HiddenField ID="hdnDPStats" Value="Disabled" runat="server"></asp:HiddenField>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td valign="middle" style="text-align: right; width: 10%">Comment :
                                        </td>
                                        <td style="width: 80%;" valign="top">
                                            <asp:TextBox ID="txtComment" runat='server' TextMode="MultiLine" Height="40" Width="200px"></asp:TextBox>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>

                    <td style="width: 45%;" align="left" valign="top">
                        <div style="border: 2px solid Navy; height: 220px" valign="top">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Extend  AUTO URL Validity
                                </div>
                            </div>
                            <div class="panel-success">
                                <table id="tblGUID" class="AutoUrl_Table" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 10%">GUID :
                                        </td>
                                        <td colspan="3" style="width: 100%">
                                            <asp:TextBox ID="txtGUID" runat="server" Width="85%"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vGUID" runat="server" ControlToValidate="txtGUID"
                                                ForeColor="Red" ValidationGroup="vGroupGUID">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:RegularExpressionValidator ID="rGUID" runat="server" ControlToValidate="txtGUID"
                                                ForeColor="Red" ValidationGroup="vGroupGUID" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?"
                                                ErrorMessage="Enter valid url"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 10%" valign="middle">Type:
                                        </td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="drpType" runat="server" Width="170px" onchange="SelectionChange();">
                                                <asp:ListItem Selected="True" Text="Web" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Share" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Excel" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Beast" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right; width: 15%;" valign="middle">Extend validity by :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpExtndValidity" runat="server" Width="170px">
                                                <asp:ListItem Text="30 minutes" Value="30"></asp:ListItem>
                                                <asp:ListItem Text="1 hour" Value="60"></asp:ListItem>
                                                <asp:ListItem Text="2 hours" Value="120"></asp:ListItem>
                                                <asp:ListItem Text="3 hours" Value="180"></asp:ListItem>
                                                <asp:ListItem Text="6 hours" Value="360"></asp:ListItem>
                                                <asp:ListItem Text="9 hours" Value="540"></asp:ListItem>
                                                <asp:ListItem Text="12 hours" Value="720"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="24 hours (1 Day)" Value="1440"></asp:ListItem>
                                                <asp:ListItem Text="48 hours (2 days)" Value="2880"></asp:ListItem>
                                                <asp:ListItem Text="72 hours (3 days)" Value="4320"></asp:ListItem>
                                                <asp:ListItem Text="96 hours (4 days)" Value="5760"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center" valign="top" style="text-align: center; border: none; height: 10px"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Button ID="btnVldiyExtnd" runat="server" CssClass="btnClass" OnClick="btnVldiyExtnd_Click"
                                                Text="Extend" ValidationGroup="vGroupGUID" />
                                            <%-- </td>
                                <td colspan="3">--%>
                                            <asp:Label ID="lblExtndValidity" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:ValidationSummary ID="vGUIDSmry" runat="server" ShowMessageBox="false" ShowSummary="false"
                                                ValidationGroup="vGroupGUID" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 8px"></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>

            </table>

            <table style="vertical-align: middle; width: 90%;" align="center">
                <tr>
                    <td style="width: 30%;" align="left" valign="top">
                        <div style="border: 2px solid Navy; margin-right: 5px;" align="left" valign="top">
                            <table id="NewCategory" style="width: 100%; border-bottom: none;" align="center">
                                <tr>
                                    <td>
                                        <div class="panel panel-primary">
                                            <div class="panel-heading">
                                                Category
                                      
                                            </div>
                                        </div>
                                        <div class="panel-success">
                                            <table>

                                                <tr>
                                                    <td colspan="2" style="border-top-color: black; text-align: center; padding-bottom: 5px">
                                                        <asp:TextBox Placeholder="Add new Group:" ID="txtGrpName" runat='server' Width="93%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%; vertical-align: top; border-color: white; border-collapse: none">
                                                        <asp:ListView ID="lstCategory" runat="server" Style="border: 0 solid #E0E0E0; border-collapse: none; width: 100%">
                                                            <LayoutTemplate>
                                                                <table id="tblCategory" cellpadding="1" style="border: 0 solid #E0E0E0; border-collapse: none; width: 100%">
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="Pointer">
                                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("CategoryName") %>' />
                                                                    </td>
                                                                    <td align='right' class='<%#Eval("CategoryId") %>'></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                    <td style="width: 60%; vertical-align: top; text-align: left; border-color: white; border-bottom: none;"
                                                        class="bgcolor">
                                                        <div id="Calclist" style="width: 100%; vertical-align: top; text-align: left; border-bottom: none; min-height: 320px; max-height: 320px; overflow-y: auto">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                    </td>
                                </tr>

                            </table>
                            <table id="GroupTr" style="width: 100%; border-bottom: none; display: none" align="center"
                                border="1">
                                <tr>
                                    <td class="AutoUrl_TableTitle" colspan="2">Group List
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%; vertical-align: top; border-color: white" id="groupList">
                                        <asp:ListView ID="lstGroup" runat="server" Style="border: 0 solid #E0E0E0; width: 100%">
                                            <LayoutTemplate>
                                                <div style="width: 100%; vertical-align: top; border-bottom: none; text-align: left; min-height: 320px; max-height: 320px; overflow-y: auto; overflow-x: hidden">
                                                    <ul id="tblGroup" cellpadding="1" style="border: 0 solid #E0E0E0; width: 100%">
                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                    </ul>
                                                </div>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <li class="Pointer" onclick="changeImageGroup(this);">
                                                    <img id="imgSelc" style="height: 14px !important; width: 14px !important;" class='<%#Eval("GroupId") %> unselected'
                                                        src="images/UnSelected.png" name="GrpName" />
                                                    <%--<input type="radio" class='<%#Eval("GroupId") %>' name="GrpName" />--%>
                                                    <label id="lblGroupName" class="name" style="display: inline-block">
                                                        <%#Eval("GroupName") %></label>
                                                </li>
                                                <%--  <li align='right' class='<%#Eval("GroupName") %>'>
                                              
                                                 </li>--%>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                    <td style="width: 50%; vertical-align: top; text-align: left; border-color: white; border-bottom: none"
                                        class="bgcolor">
                                        <div id="grpdivlst" style="width: 100%; vertical-align: top; border-bottom: none; text-align: left; min-height: 320px; max-height: 320px;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="1">
                                <tr>
                                    <td align="right" style="border-color: white">
                                        <asp:Button ID="btnSaveGroup" runat="server" Text="Save Group" CssClass="btnClass"
                                            ValidationGroup="Group"></asp:Button>
                                    </td>
                                    <td style="border: none" class="style1">
                                        <asp:Button ID="btnExistingGroup" runat="server" Text="Existing Group" CssClass="btnClass" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>



                    <td style="width: 70%;" align="left" valign="top">
                        <div style="align="left" valign="top">                           
                                <div class="heading">
                                    Select User and Send AUTO URL
                                </div>
                           
                            <div class="pnl" >
                                <table style="width: 100%" align="center" class="allUsers">
                                    <tr>
                                        <td>
                                              <div id="loading" style="vertical-align:central">
                                        <img src="images/loadingImage.png" />
                                    </div>
                                            <asp:Repeater ID="rptrUsers" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblAutoURL" style="width: 100%; border-spacing: 0; display: none">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th style="padding: 0px !important">
                                                                    <input type="checkbox" id="ChkAllCheck" class="ChkAllCheck" />
                                                                </th>
                                                                <th>User Id
                                                                </th>
                                                                <th>UserName
                                                                </th>

                                                                <th>Email Id
                                                                </th>

                                                            </tr>


                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr id='<%#Eval("UserID")%>'>
                                                        <td>

                                                            <input type="checkbox" id="CheckAll" name="CheckAll" value='<%#Eval("UserID") + " ," + Eval("UserName") + "," + Eval("Login_ID") + "," + Eval("CustomerId") +"!"%>' class="CheckAll" runat="server" />

                                                        </td>
                                                        <td style="margin-left: 1px;">

                                                            <%#Eval("UserID")%>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <%#Eval("UserName")%>   

                                                        </td>

                                                        <td>
                                                            <%#Eval("Login_Id")%>
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
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <table style="width: 100%; margin-top:10px">
                            <tr>

                                <td style="height: 23px; text-align: center; border: none; width: 20%">
                                    <asp:UpdateProgress AssociatedUpdatePanelID="upAutoUrl" ID="updProgress" runat="server"
                                        DynamicLayout="true">
                                        <ProgressTemplate>
                                            <img alt="Please wait..." src="images/Loding.gif" align="middle" id="imgLoad" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td align="center" style="border: none; width: 60%">
                                    <%--<asp:Label ID="lblMessInfo" runat="server" Font-Names="Verdana" Font-Size="8pt" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>--%>
                                    <asp:Label ID="lblMessage" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                        Font-Bold="true" Style="text-align: left; display: block" ForeColor="Navy"></asp:Label>
                                </td>
                                <td align="right" style="width: 20%">
                                    <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClick="btnSendMail_Click" OnClientClick="return(CheckboxSelection());"
                                        CssClass="btnClass CalcList"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: none; font-style: italic;" colspan="2">
                                    <asp:Label ID="lblNote" runat="server" Text="NOTE: UserId, LoginId, UserName and Mnemonic must be available for successful sending
                                    of emails."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


                <asp:HiddenField ID="hdnUserID" runat="server" />
                <asp:HiddenField ID="hdnStoreUserID" runat="server" />
                <asp:HiddenField ID="hdnGroup" runat="server" />
                <asp:HiddenField ID="hdnMailId" runat="server" />
                <asp:HiddenField ID="hdnCmeBackground" runat="server" Value="0" />
                <asp:HiddenField ID="CalcList" runat="server" />
                <input type="hidden" id="hdnPageFocusChange" value="true" />

            </table>
            </div>
                    </div>
        </ContentTemplate>
        <Triggers>

            <asp:AsyncPostBackTrigger ControlID="ddGroup" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>

