var selectedBox = "";
var selectedBoxTd = "";
var selectedTime = "00:00";
var selectedTimeId;
var content = "";
var GrossRem = "0";
var amPm = "AM";
var high = 0, tempId; //highlight code 0,1 and 2
function showText(ids) {
    var down, up;
    down = parseInt(ids.parentNode.childNodes[8].value);
    up = parseInt(ids.parentNode.childNodes[10].value);
    document.getElementById('errMsg').style.display = "block";
    document.getElementById('errMsg').innerHTML = "Amount Range...<br/>Min  : " + down + "<br/>Max  : " + up;
    high = 2;
    tempId = ids.parentNode.childNodes[0];
    ids.style.backgroundColor = "#FFF8DC";
}
function getGrossRemaining(slabId, value, dropValue, packId, gross, empId) {
    var xmlhttp, temp, url;
    url = "getRemainingGross.aspx?slabId=" + slabId + "&packId=" + packId + "&currentInput=" + value + "&method=" + dropValue + "&empId=" + empId;
    // alert(url);
    if (window.XMLHttpRequest) {      // code for IE7+, Fire***, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            GrossRem = parseInt(xmlhttp.responseText);
            var finalAnswer = 0;
            if (dropValue == "1") {
                finalAnswer = parseInt(gross) - (parseInt(GrossRem) + parseInt(value));
            }
            else {
                value = (parseInt(gross) * parseInt(value)) / 100;
                finalAnswer = parseInt(gross) - (parseInt(GrossRem) + parseInt(value));
            }
            var perRem = 0;
            var tempStr = "";
            tempStr = ((parseInt(finalAnswer) * 100) / parseInt(gross)).toString();
            perRem = tempStr.substr(0, 5);
            if (isNaN(finalAnswer)) {
                getGrossRemaining(slabId, 0, 0, packId, gross, empId);
            }
            else {
                document.getElementById('getAmount').innerHTML = finalAnswer + " Rs.  (" + perRem + " %)";
                document.getElementById('flagAmount').innerHTML = finalAnswer;
            }
        }
    }
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}
function showLabel(ids) {
    ids.style.backgroundColor = "#FFFFFF";
}
function updateSum(ids) {
    var i = 1, len = 0, sum = 0, tt;
    // document.getElementById('errMsg').innerHTML = ids.parentNode.parentNode.parentNode.parentNode.childNodes.length - 1;
    len = ids.parentNode.parentNode.parentNode.parentNode.childNodes.length - 1;
    for (i = 1; i <= len; i++) {
        //   document.getElementById('errMsg').innerHTML =  ids.parentNode.parentNode.parentNode.parentNode.childNodes[i].childNodes[4].childNodes[0].childNodes[0].value;
        try {
            if (ids.parentNode.parentNode.parentNode.parentNode.childNodes[i].childNodes[4].childNodes[0].childNodes[0].value.length != 0) {
                sum = sum + parseInt(ids.parentNode.parentNode.parentNode.parentNode.childNodes[i].childNodes[4].childNodes[0].childNodes[0].value);
            }
        }
        catch (ex)
        { }
        //  alert(ids.parentNode.parentNode.parentNode.parentNode.childNodes[i].innerHTML);
        // alert("");
    }
    ids.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.childNodes[1].childNodes[3].childNodes[0].innerHTML = sum;
}
function update(ids) {
    //start
    //    var empId, sectionDetailId,amount;
    //    amount = ids.parentNode.childNodes[2].value;
    //    sectionDetailId = ids.parentNode.childNodes[4].value;
    //    empId = ids.parentNode.childNodes[6].value;
    //       SaveDetails("saveEmpInvestment.aspx?empId="+ empId + "&amount=" + amount + "&sectionDetailId=" +sectionDetailId);
    //end
    var down, up;
    down = parseInt(ids.parentNode.childNodes[8].value);
    up = parseInt(ids.parentNode.childNodes[10].value);
    var empId, sectionDetailId, wef;
    var amount;
    amount = parseInt(ids.parentNode.childNodes[0].value);
    sectionDetailId = ids.parentNode.childNodes[2].value;
    empId = ids.parentNode.childNodes[4].value;
    wef = ids.parentNode.childNodes[6].value;
    // if (amount >= down && amount <= up) {
    // SaveDetails("saveEmpInvestment.aspx?empId=" + empId + "&amount=" + amount + "&sectionDetailId=" + sectionDetailId + "&forTheYear=" + forTheYear);
    // }
    // else
    {
        if (ids.value.length == 0) {
            SaveDetails("saveEmpInvestment.aspx?empId=" + empId + "&amount=101010&sectionDetailId=" + sectionDetailId + "&wef=" + wef);
            //alert("nothing");
        }
        else {
            //alert("hi");
            high = 0;
            tempId = ids.parentNode.childNodes[0];
            //highlight();
            //ids.focus();
            //  document.getElementById('errMsg').style.display = "block";
            //  document.getElementById('errMsg').innerHTML = "Amount Range...<br/>Min  : " + down + "<br/>Max  : " + up;
            //   ids.parentNode.childNodes[0].value = down;
            SaveDetails("saveEmpInvestment.aspx?empId=" + empId + "&amount=" + amount + "&sectionDetailId=" + sectionDetailId + "&wef=" + wef);
            setTimeout("document.getElementById('errMsg').style.display='none'", 6000);
        }
        // alert("Amount not eligible..." + amount + "  " + down + "  " + up );
    }
}
function highlight() {
    if (high == 0) {
        // alert(ids.value);
        high = 1;
        tempId.style.backgroundColor = "#FFF8DC";
        //alert("2");
        setTimeout("highlight();", 400);
    }
    else if (high == 1) {
        //alert("");
        high = 0;
        tempId.style.backgroundColor = "#F08080";
        setTimeout("highlight();", 400);
    }
    else {
        tempId.style.backgroundColor = "#FFFFFF";
        return;
    }
}
function checkExistenceofId(value, id) {
    //alert(value);
    var url = "checkExistence.aspx?value=" + value;
    // alert(url);
    ajax(id, url);
}
function SaveDetails(page) {
    var xmlhttp;
    if (window.XMLHttpRequest) {      // code for IE7+, Fire***, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            // alert("hi");
            //    document.getElementById(id).innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.open("GET", page, true);
    xmlhttp.send();
}
function SearchEmployees(value, page) {
    var xmlhttp;
    if (window.XMLHttpRequest) {      // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById(id).innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.open("GET", page, true);
    xmlhttp.send();
}
function ajax(id, url) {
    var xmlhttp;
    // alert(url);
    ////  var cntnt;
    //// var o = document.createElement('div');
    ////o.setAttribute('id', 'tmp');    
    //// var txt = "";
    ////  alert(id + "   " + url);
    if (window.XMLHttpRequest) {      // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            //alert(document.getElementById(id).innerHTML);
            //alert(xmlhttp.responseText);
            //// txt = xmlhttp.responseText;
            //// if (txt.length > 0) {
            //alert(xmlhttp.responseText);
            document.getElementById(id).innerHTML = xmlhttp.responseText;
            ////   o.innerHTML = xmlhttp.responseText;
            //document.getElementById(id). = o.childNodes[0].childNodes(3); //o.childNodes[0].innerHTML;
            //o.childNodes[0].removeChild(o.childNodes[0].childNodes[1]);
            //o.childNodes[0].removeChild(o.childNodes[0].childNodes[2]);
            //o.childNodes[0].removeChild[1];
            //alert(o.getElementsByTagName('div').item(2).innerHTML);
        }
        //// document.getElementById(id).innerHTML = o.getElementsByTagName('div').item(2).innerHTML;
    }
    //alert(xmlhttp.responseText);
    ////}
    // alert(id + "   " + url);
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}
function changeColor(id, color) {
    document.getElementById(id).style.background = color;
}
function brightit(id) {
    if (document.getElementById(id).style.backgroundColor != 'black') {
        document.getElementById(id).style.backgroundColor = '#404040';
        document.getElementById(id).style.borderBottom = '1px solid #000000';
        document.getElementById(id).style.borderLeft = '1px solid #8D8D8D';
        document.getElementById(id).style.borderRight = '1px solid #676767';
        document.getElementById(id).style.borderTop = '1px solid #B4B4B4';
    }
}
function recolorit(id) {
    if (document.getElementById(id).style.backgroundColor != 'black') {
        document.getElementById(id).style.backgroundColor = '#232323';
        document.getElementById(id).style.borderColor = '#232323';
    }
}
function brightlink(id) {
    if (document.getElementById(id).style.backgroundColor != 'white') {
        document.getElementById(id).style.backgroundColor = '#C0C0C0';
    }
}
function recolorlink(id) {
    if (document.getElementById(id).style.backgroundColor != 'white') {
        document.getElementById(id).style.backgroundColor = '#505050';
    }
}
function brighttime(id) {
    document.getElementById(id).style.backgroundColor = '#C0C0C0';
}
function picktime() {
    try {
        selectedTime = document.getElementById(selectedTimeId).innerText;
        document.getElementById(selectedBox).value = document.getElementById(selectedTimeId).innerText + " " + amPm;
    }
    catch (ex) {
        if (document.getElementById(selectedBox).value != "00:00 AM" || document.getElementById(selectedBox).value != "00:00 PM") {
            var s = document.getElementById(selectedBox).value;
            s = s.substring(0, 5);
            document.getElementById(selectedBox).value = s + " " + amPm;
        }
        else {
            document.getElementById(selectedBox).value = selectedTime + " " + amPm;
        }
    }
    //document.getElementById('TimePicker').style.display = 'none';
    //alert(document.getElementById(selectedBoxTd).innerHTML);       
    document.getElementById(selectedBoxTd).removeChild(document.getElementById("TimePicker"));
    selectedTimeId = "";
    document.getElementById(selectedBoxTd)
    //document.getElementById(selectedBoxTd).innerHTML = content;
}
function showProgress() {
    document.getElementById('progressBar').style.display = 'block';
}
function hideProgress() {
    window.parent.document.getElementById('progressBar').style.display = 'none';
}
function changeContent(ids) {
    var iframe = document.createElement('iframe');
    iframe.setAttribute('width', '100%');
    iframe.setAttribute("frameBorder", "0");
    iframe.setAttribute('scrolling', 'no');
    iframe.setAttribute('height', '100%');
    iframe.setAttribute('id', 'ifrm');
    iframe.filter = "chroma(color = #FFFFFF)";
    iframe.setAttribute("allowtransparency", "true");
    iframe.setAttribute("filter:chroma(color = '#FFFFFF')", "");
    iframe.setAttribute('src', ids);
    var dest = document.getElementById('content_body');
    dest.innerHTML = '';
    dest.appendChild(iframe);
    if (ids == 'Home.aspx') {
        document.getElementById('ifrm').style.height = document.documentElement.clientHeight - 220;
        document.getElementById('contentTitle').innerHTML = "Home";
        document.getElementById('searchbar').style.display = 'none';
        document.getElementById('progressBar').style.display = 'none';
    }
    else if (ids == 'Contacts.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "VCM Contacts";
        document.getElementById('progressBar').style.display = 'none';
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'SiteMap.aspx') {
        document.getElementById('ifrm').style.height = document.documentElement.clientHeight - 220;
        document.getElementById('contentTitle').innerHTML = "EMS Map";
        document.getElementById('progressBar').style.display = 'none';
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmployeeList.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employees Personal Details";
        document.getElementById('searchbar').style.position = 'absolute';
        document.getElementById('searchbar').style.top = 163;
        var winW;
        winW = document.body.offsetWidth / 2;
        document.getElementById('searchbar').style.left = winW + 120;
        document.getElementById('searchbar').style.display = 'block';
        document.getElementById('progressBar').style.display = 'none';
    }
    else if (ids == 'AddEmployee.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Add New Employee";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmployeePersonal.aspx?op=edit') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Personal Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmployeePersonal.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Personal Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmployeeRelations.aspx') {
        document.getElementById('ifrm').style.height = 1100 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Relationships";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmpQualification.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Qualifications";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmpEmployers.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Employers";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmpProjects.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Projects/ Products";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmployeeBank.aspx') {
        document.getElementById('ifrm').style.height = 1100 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Bank Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'EmpDocument.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Documents Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'Documents.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Documents Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'SkillsDetail.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Skill Details";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'GeneralDetails.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's General Information";
        document.getElementById('searchbar').style.display = 'none';
    }
    else if (ids == 'MonthlyAttendance.aspx') {
        document.getElementById('ifrm').style.height = 1100 + "px";
        document.getElementById('contentTitle').innerHTML = "Monthly Attendance";
    }
    else if (ids == 'DailyAttendance.aspx?page=1') {
        document.getElementById('ifrm').style.height = 1200 + "px";
        document.getElementById('contentTitle').innerHTML = "Daily Attendance";
    }
    else if (ids == 'DailyAttendance.aspx') {
        document.getElementById('ifrm').style.height = 1200 + "px";
        document.getElementById('contentTitle').innerHTML = "Daily Attendance";
    }
    else if (ids == 'LeaveBalanceEntry.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave Balance Entries";
    }
    else if (ids == 'MonthlyAttendance.aspx?page=1') {
        document.getElementById('ifrm').style.height = 1000 + "px";
        document.getElementById('contentTitle').innerHTML = "Monthly Attendance Details";
    }
    else if (ids == 'LeaveCOffDetails.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave COff Assignment";
    }
    else if (ids == 'LeaveEntitlement.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave Allocation Entry";
    }
    else if (ids == 'LeaveEligibility.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave Eligibility";
    }
    else if (ids == 'LeaveType.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave Types";
    }
    else if (ids == 'LeaveApplication.aspx') {
        document.getElementById('ifrm').style.height = 700 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave  Application";
        document.getElementById('progressBar').style.display = 'none';
    }
    else if (ids == 'Holiday.aspx') {
        document.getElementById('ifrm').style.height = 1200 + "px";
        document.getElementById('contentTitle').innerHTML = "Holiday List ";
    }
    else if (ids == 'LeaveTypeMaster.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Leave Type Master";
        document.getElementById('progressBar').style.display = 'none';
    }
    else if (ids == 'RFID.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Card Numbers";
    }
    else if (ids == 'PackageDetails.aspx') {
        document.getElementById('ifrm').style.height = 1500 + "px";
        document.getElementById('contentTitle').innerHTML = "Package Details";
    }
    else if (ids == 'Deductions.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Slabs";
    }
    else if (ids == 'DeductionDetail.aspx?flag=0') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee Deduction Slabs";
    }
    else if (ids == 'DeductionDetail.aspx?flag=1') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Employer Deduction Slabs";
    }
    else if (ids == 'IncomeTax.aspx') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Income Tax";
    }
    else if (ids == 'Earnings.aspx') {
        document.getElementById('ifrm').style.height = 1000 + "px";
        document.getElementById('contentTitle').innerHTML = "Earnings";
    }
    else if (ids == 'Departments.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Departments";
    }
    else if (ids == 'Designations.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Designations";
    }
    else if (ids == 'Banks.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Banks";
    }
    else if (ids == 'EmployeeCard.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "  px";
        document.getElementById('contentTitle').innerHTML = "Cards Management";
    }
    else if (ids == 'MonthlyAttendance.aspx') {
        document.getElementById('ifrm').style.height = 565 + "px";
        document.getElementById('contentTitle').innerHTML = "Monthly Attendance";
    }
    else if (ids == 'ManageUsers.aspx') {
        document.getElementById('ifrm').style.height = 900 + "px";
        document.getElementById('contentTitle').innerHTML = "Manage Users";
    }
    else if (ids == 'StatusDetails.aspx') {
        document.getElementById('ifrm').style.height = 900 + "px";
        document.getElementById('contentTitle').innerHTML = "Status Details";
    }
    else if (ids == 'AutoLeaveAllocatting.aspx') {
        document.getElementById('ifrm').style.height = 900 + "px";
        document.getElementById('contentTitle').innerHTML = "Vypar Leave Allocation";
    }
    else if (ids == 'AutoLeaveAllocatting.aspx') {
        document.getElementById('ifrm').style.height = 900 + "px";
        document.getElementById('contentTitle').innerHTML = "Vypar Leave Allocation";
    }
    else if (ids == 'CLSLPolicy.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "CL & SL Leave Policy";

    }
    else if (ids == 'PLPolicy.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "PL Leave Policy";

    }
    else if (ids == 'AutoLeaveDeduction.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee Leaves Deduction";

    }
    else if (ids == 'AutoLeaveDeductApproved.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee Deducted Leaves Approve";

    }

    else if (ids == 'EmpOBImport.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "CL & SL Leave Policy";

    }
    else if (ids == 'OLPolicy.aspx') {
        document.getElementById('ifrm').style.height = 700 + "px";
        document.getElementById('contentTitle').innerHTML = "Optional Leave Policy";

    }
    else if (ids == 'AutoLeaveEligibility.aspx') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee's Leave Eligibility & Balance";

    }
    else if (ids == 'DeclaredAmount.aspx') {
        document.getElementById('ifrm').style.height = 2000 + "px";
        document.getElementById('contentTitle').innerHTML = "Investement Declaration";
    }
    else if (ids == 'PaySlip.aspx') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Generate Payslip";
    }
    else if (ids == 'SectionRules.aspx') {
        document.getElementById('ifrm').style.height = 600 + "px";
        document.getElementById('contentTitle').innerHTML = "Exemptions";
    }
    else if (ids == 'Sections.aspx') {
        document.getElementById('ifrm').style.height = 1150 + "px";
        document.getElementById('contentTitle').innerHTML = "Exemptions";
    }
    else if (ids == 'EarningSlabs.aspx') {
        document.getElementById('ifrm').style.height = 1500 + "px";
        document.getElementById('contentTitle').innerHTML = "Exemptions";
    }
    else if (ids == 'EmpLeaveEntitlement.aspx') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Employee Entitlement & Leave Balance";
    }
    else if (ids == 'Notifications.aspx') {
        document.getElementById('ifrm').style.height = 800 + "px";
        document.getElementById('contentTitle').innerHTML = "Notification(s)";
        document.getElementById('progressBar').style.display = 'none';
    }
    else {
        document.getElementById('ifrm').style.height = document.documentElement.clientHeight - 220;
        document.getElementById('contentTitle').innerHTML = "VCM Partners (India) Private Limited";
        document.getElementById('progressBar').style.display = 'none';
        document.getElementById('main_body').style.height = document.documentElement.clientHeight - 170;
    }
    changeDesignHeight();
}
function changeDesignHeight() {
    document.getElementById('leftshade').style.height = document.getElementById('content_body').offsetHeight;
    document.getElementById('rightshade').style.height = document.getElementById('content_body').offsetHeight;
    if ((document.getElementById('content_body').offsetHeight + 220) < (document.documentElement.clientHeight)) {
        document.getElementById('main_body').style.height = document.documentElement.clientHeight - 170;
    }
    else {
        document.getElementById('main_body').style.height = document.getElementById('content_body').offsetHeight + 50;
    }
    document.getElementById('left_shade').style.height = document.getElementById('main_body').offsetHeight;
    document.getElementById('right_shade').style.height = document.getElementById('main_body').offsetHeight;
    document.getElementById('left_menu').style.height = document.getElementById('main_body').offsetHeight;
}
function changeColors(id, parent) {
    var t = document.getElementById(parent);
    var i = 0;
    for (i = 0; i < document.getElementById(parent).childNodes.length; i++) {
        document.getElementById(parent).childNodes[i].style.backgroundColor = "#606060";
    }
    if (id != '') {
        id.style.backgroundColor = "white";
    }
}
function changeColors2(id, parent) {
    var t = document.getElementById(parent);
    var i = 0;
    for (i = 0; i < document.getElementById(parent).childNodes.length; i++) {
        document.getElementById(parent).childNodes[i].style.backgroundColor = "#707070";
    }
    if (id != '') {
        id.style.backgroundColor = "white";
    }

}
function changeAll() {
    for (i = 0; i < document.getElementById("leavediv").childNodes.length; i++) {
        document.getElementById("leavediv").childNodes[i].style.backgroundColor = "#606060";
    }
}
function showPicker() {
    destroyPicker();
    content = document.getElementById(selectedBoxTd).innerHTML;
    document.getElementById(selectedBoxTd).innerHTML = document.getElementById(selectedBoxTd).innerHTML + document.getElementById("right").innerHTML;
    document.getElementById('TimePicker').style.display = 'block';
}
function destroyPicker() {
    try {
        document.getElementById("issueTimeTd").removeChild(document.getElementById("TimePicker"));
    }
    catch (ex)
{ }
    try {
        document.getElementById("endtimeTd").removeChild(document.getElementById("TimePicker"));
    }
    catch (ex)
    { }
    try {
        document.getElementById("revoketimeTd").removeChild(document.getElementById("TimePicker"));
    }
    catch (ex)
    { }
}

function hidelogdiv() {
    var dv = document.getElementById("in_out_logs");
    dv.style.display = "none";
    document.getElementById("lblDetailLogs").innerHTML = "";
    document.getElementById("lblOutsideDetails").innerHTML = "";
}

function setstatusdiv() {
    var dv = document.getElementById("statusDiv");
    dv.style.position = "absolute";
    dv.style.pixelLeft = event.x + 50;
    dv.style.pixelTop = event.y - 159;
}

function appsetstatusdiv() {
    var dv = document.getElementById("appstatusDiv");
    dv.style.position = "absolute";
    dv.style.pixelLeft = event.x - 250;
    dv.style.pixelTop = event.y - 150;
}
function savePosition() {
    document.getElementById('X').value = event.x - 370;
    document.getElementById('Y').value = event.y - 15;
}
function showstatusdiv() {
    document.getElementById("statusDiv").style.display = 'block';
}
function appshowstatusdiv() {
    document.getElementById("appstatusDiv").style.display = 'block';
}
function setstatusdivExtra() {
    var dv = document.getElementById("statusDivExtra");
    dv.style.position = "absolute";
    dv.style.pixelLeft = event.x - 300;
    dv.style.pixelTop = event.y - 100;
}
function showstatusdivExtra() {
    document.getElementById("statusDivExtra").style.display = 'block';
}

function CheckOtherIsCheckedByGVID(spanChk) {
    var IsChecked = spanChk.checked;
    if (IsChecked) {
        spanChk.parentElement.parentElement.style.backgroundColor = 'Silver';
    }
    var CurrentRdbID = spanChk.id;
    var Chk = spanChk;
    Parent = document.getElementById("srchView");
    var items = Parent.getElementsByTagName('input');
    for (i = 0; i < items.length; i++) {
        if (items[i].id != CurrentRdbID && items[i].type == "radio") {
            if (items[i].checked) {
                items[i].checked = false;
            }
        }
    }
}

function ResetScrollPosition() {
    var scrollX = document.getElementById('__SCROLLPOSITIONX');
    var scrollY = document.getElementById('__SCROLLPOSITIONY');
    if (scrollX && scrollY) {
        scrollX.value = 0;
        scrollY.value = 0;
    }
}

//function SendMothlyAttach() {
//    var theApp; 	//Reference to Outlook.Application
//    var theMailItem; 	//Outlook.mailItem

//    var MailTo = document.form1.elements['mailto'].value;
//    var subject = "EMS :: Your " + document.form1.elements['monthyear'].value + "Till Date Presence";
//    var msg = document.form1.elements['mailtext'].value;
//    try {
//        var theApp = new ActiveXObject("Outlook.Application");
//        var theMailItem = theApp.CreateItem(0);
//        theMailItem.to = MailTo;
//        theMailItem.Subject = subject;
//        theMailItem.HTMLBody = msg;
//        theMailItem.display();
//        theMailItem = null;
//    }
//    catch (err) {
//       
//    }
//}


function SendAttendanceMail() {
    var theApp	//Reference to Outlook.Application
    var theMailItem	//Outlook.mailItem
    var MailTo = document.form1.elements['mailto'].value;
    var subject = "EMS :: Your " + document.form1.elements['monthyear'].value + "Till Date Presence"
    var msg = document.getElementById("griddiv").innerHTML + "<br/>" + document.getElementById("summery").innerHTML;


    for (i = 1; i <= 9; i++) {
        msg = msg.replace('<INPUT id=srchView_ctl0' + i + '_chkSendMail type=checkbox name=srchView$ctl0' + i + '$chkSendMail>', '');
        msg = msg.replace('<INPUT style="BORDER-RIGHT-WIDTH: 0px; WIDTH: 16px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px" id=srchView_ctl0' + i + '_statusImageButton onmouseover=setstatusdiv(); src="http://indiadev/vcmems/images/pen(2).ico" type=image name=srchView$ctl0' + i + '$statusImageButton>', '');
        msg = msg.replace('<INPUT style="BORDER-RIGHT-WIDTH: 0px; WIDTH: 18px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px" id=srchView_ctl0' + i + '_logsImageButton onmousemove=showlogdiv(); onmouseout=hidelogdiv() src="http://indiadev/vcmems/images/clock.ico" type=image name=srchView$ctl0' + i + '$logsImageButton>', '');
        msg = msg.replace('<INPUT style="BORDER-RIGHT-WIDTH: 0px; WIDTH: 20px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px" id=srchView_ctl01_ImageButton1 src="http://indiadev/vcmems/images/e_mail.png" type=image name=srchView$ctl01$ImageButton1>', '');
    }
    for (i = 10; i <= 35; i++) {
        msg = msg.replace('<INPUT id=srchView_ctl' + i + '_chkSendMail type=checkbox name=srchView$ctl' + i + '$chkSendMail>', '');
        msg = msg.replace('<INPUT style="BORDER-RIGHT-WIDTH: 0px; WIDTH: 16px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px" id=srchView_ctl' + i + '_statusImageButton onmouseover=setstatusdiv(); src="http://indiadev/vcmems/images/pen(2).ico" type=image name=srchView$ctl' + i + '$statusImageButton>', '');
        msg = msg.replace('<INPUT style="BORDER-RIGHT-WIDTH: 0px; WIDTH: 18px; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px" id=srchView_ctl' + i + '_logsImageButton onmousemove=showlogdiv(); onmouseout=hidelogdiv() src="http://indiadev/vcmems/images/clock.ico" type=image name=srchView$ctl' + i + '$logsImageButton>', '');
    }
    try {
        var theApp = new ActiveXObject("Outlook.Application")
        var theMailItem = theApp.CreateItem(0)
        theMailItem.to = MailTo;
        theMailItem.Subject = (subject);
        theMailItem.Body = (msg);
        theMailItem.display()
    }
    catch (err) {
    }
}