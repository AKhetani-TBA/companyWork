function RepositionOpenedWidget() {
    try {
        //Reading elementid around which widget is getting opened.
        var widgetElement = $.trim($('#hdnWgtElement').val());

        if (widgetElement !== null && widgetElement !== '') {
            positionWidget(widgetElement);
        }
    }
    catch (err) {
        alert(err.Message);
    }
}

function positionWidget(elementId) {
    try {
        //Repositioning widget around elementId
        var element = $('#' + elementId);
        var elementHeight = $(element).outerHeight(true);
        var elementWidth = $(element).outerWidth(true);

        var elementOffSet = $(element).offset();
        elementOffSet.top += elementHeight;

        var widgetId = "";

        if ($(element).hasClass("priceWidget")) {
            widgetId = "dvPriceWidgetMobile";
        }
        else if ($(element).hasClass("termWidget")) {
            widgetId = "dvTermWidgetMobile";
        }
        else if ($(element).hasClass("basisWidget")) {
            widgetId = "tblBasis";
            if ($('#divTermBasisWidget').css('display') == 'block') {
                this.Reposition_Term_BasisWidget($('#txtFrequency'));
            }

        }

        var widget = $('#' + widgetId);

        $(widget).css({
            'margin-left': '0px !important',
        });

        $(widget).offset(elementOffSet);

        //If element is not visible on screen will reposition.
        if (!isElementCompletelyVisible(widget)) {
            elementOffSet.top = elementOffSet.top - widget.outerHeight(true) - elementHeight;
            $(widget).offset(elementOffSet);
        }
        if (!isElementCompletelyVisibleHorizontal(widget)) {
            elementOffSet.left = elementOffSet.left - widget.outerWidth(true) + elementWidth;
            $(widget).offset(elementOffSet);
        }
    }
    catch (err) {
        $('#hdnWgtElement').val('');
    }
}

function Reposition_Term_BasisWidget(objClicked) {
    try {
        var offset = $($("#tblBasis")).offset();
        var height = $($("#tblBasis")).outerHeight(true);

        var x = offset.top;
        var y = offset.left;

        if (x <= 391) // height from top to link is less than widget open below otherwise it will open on top side
        {
            $('#divTermBasisWidget').css({
                top: 0,
                left: 0
            });
        }
        else {
            $('#divTermBasisWidget').css({
                top: -210,
                left: 0
            });
        }
    }
    catch (err) {
        var strerrordesc = "Function:display_Term_BasisWidget(); Error is : " + err.description + "; Error number is " + err.number + "; Message :" + err.message;
        onJavascriptLog("basisWidgetScript.js", strerrordesc);
    }
}

//Common function to check specified html element is visible on screen or not.
function isElementCompletelyVisible(element) {
    var windowTop = $(window).scrollTop();
    var windowBottom = windowTop + $(window).height();

    var elementTop = $(element).offset().top;
    var elementBottom = elementTop + $(element).height();

    return ((elementBottom <= windowBottom) && (elementTop >= windowTop));
}

function isElementCompletelyVisibleHorizontal(element) {

    var windowLeft = $(window).scrollLeft();
    var windowRight = windowLeft + $(window).width();

    var elementLeft = $(element).offset().left;
    var elementRight = elementLeft + $(element).width();

    return ((elementRight <= windowRight) && (elementLeft >= windowLeft));
}

$.keyCode = {
    BACKSPACE: 8, TAB: 9, ENTER: 13, SHIFT: 16, CONTROL: 17, ALTER: 18, PAUSE_BREAK: 19, CAPS_LOCK: 20, ESCAPE: 27, SPACE: 32,
    PAGE_UP: 33, PAGE_DOWN: 34, END: 35, HOME: 36, LEFT: 37, RIGHT: 39, UP: 38, DOWN: 40, INSERT: 45, DELETE: 46,
    ZERO: 48, ONE: 49, TWO: 50, THREE: 51, FOUR: 52, FIVE: 53, SIX: 54, SEVEN: 55, EIGHT: 56, NINE: 57,
    a: 58, b: 59, c: 67, d: 68, e: 69, f: 70, g: 71, h: 72, i: 73, j: 74, k: 75, l: 76, m: 77, n: 78,
    o: 79, p: 80, q: 81, r: 82, s: 83, t: 84, u: 85, v: 86, w: 87, x: 88, y: 89, z: 90,
    LEFTWINDOW_KEY: 91, RIGHTWINDOW_KEY: 92, SELECT_KEY: 93,
    NUMPAD_ZERO: 96, NUMPAD_ONE: 97, NUMPAD_TWO: 98, NUMPAD_THREE: 99, NUMPAD_FOUR: 100, NUMPAD_FIVE: 101,
    NUMPAD_SIX: 102, NUMPAD_SEVEN: 103, NUMPAD_EIGHT: 104, NUMPAD_NINE: 105,
    NUMPAD_MULTIPLY: 106, NUMPAD_ADD: 107, NUMPAD_ENTER: 108, NUMPAD_SUBTRACT: 109, NUMPAD_DECIMAL: 110, NUMPAD_DIVIDE: 111,
    F1: 112, F2: 113, F3: 114, F4: 115, F5: 116, F6: 117, F7: 118, F8: 119, F9: 120, F10: 121, F11: 122, F12: 123,
    NUM_LOCK: 144, SCROLL_LOCK: 145, SEMI_COLON: 186, EQUAL_SIGN: 187, COMMA: 188, DASH: 189, PERIOD: 190, FORWARD_SLASH: 191, GRAVE_ACCENT: 192,
    OPEN_BRACKET: 219, BACK_SLASH: 220, CLOSE_BRACKET: 221, SINGLE_QUOTE: 222
};

function GEN_Func_IsNumberKey(_key) {
    if ((_key >= $.keyCode.ZERO && _key <= $.keyCode.NINE) || (_key >= $.keyCode.NUMPAD_ZERO && _key <= $.keyCode.NUMPAD_NINE)) {
        return true;
    }
    else { return false; }
}

function GEN_Func_IsNumberOrHyphenKey(_key) {
    if (GEN_Func_IsNumberAndHyphenKey(_key) || $.keyCode.DASH || $.keyCode.NUMPAD_SUBTRACT) {
        return true;
    }
    else { return false; }
}

function GEN_Func_IsDefaultAllowedKeys(_key) {
    if (_key == $.keyCode.TAB || _key == $.keyCode.BACKSPACE || _key == $.keyCode.ALTER || _key == $.keyCode.LEFTWINDOW_KEY || _key == $.keyCode.RIGHTWINDOW_KEY || _key == $.keyCode.CONTROL || _key == $.keyCode.CAPS_LOCK || _key == $.keyCode.DELETE || _key == $.keyCode.RIGHT || _key == $.keyCode.LEFT) {
        return true;
    }
    else { return false; }
}

function ReplaceAll(Source, stringToFind, stringToReplace) {
    var temp = Source;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}

function ValidateEmailId(emailString) {
    emailAddress = $.trim(emailString);
    if (emailAddress != "") {
        var regularExpression = /^[a-z0-9!#$%&'-*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/g;

        if (regularExpression.test(emailAddress)) {
            return true;
        }
        else {
            return false;
        }
    }
}

function ValidatePhoneId(phoneNumber) {
    var obj;

    phoneNumber = $.trim(phoneNumber);

    if (phoneNumber != "") {
        var number = phoneNumber;

        //var pattern = new RegExp(/^[+0-9()-/]{4,20}$/);
        //var pattern = new RegExp(/^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$/);
        //(^(\+?\-? *[0-9]+)([,0-9 ]*)([0-9 ])*$)|(^ *$)
        var pattern = new RegExp(/^\+(?:[0-9] ?){6,14}[0-9]$/);             // Compulsory + and country code

        if (!number.match(pattern)) {
            return false;
        }
        return true;
    }
    return false;
}

var curntSpanIndex = 0;

function getCurrentRowName() {
    return "rowNum" + curntSpanIndex;
}

function getRelativeRowName(spnIndx) {
    return "rowNum" + spnIndx;
}

function getCurrentRowIndex() {
    return curntSpanIndex;
}

function IncrCurrentRowIndex() {
    curntSpanIndex++;
}

function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

function mobileAndTabletcheck() {
    var check = false;
    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
}

function detectBrowser() {
    var N = navigator.appName;
    var UA = navigator.userAgent;
    var temp;
    var browserVersion = UA.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
    if (browserVersion && (temp = UA.match(/version\/([\.\d]+)/i)) != null)
        browserVersion[2] = temp[1];
    browserVersion = browserVersion ? [browserVersion[1], browserVersion[2]] : [N, navigator.appVersion, '-?'];
    return browserVersion;
}

var shareErrorCode = {
    SuccessfullLogin: 0,
    UserNotRegisteredOrBlocked: -1,
    FailedAutoUrl: -2,
    FailedWithAutoUrl: -3,
    UnauthorizedIp: -5,
    NoSessionRights: -6,
    UrlExpired: -9,
    CalculatorClosed: -10,
    properties: {
        '0': { name: "SuccessfullLogin", value: 0, description: "Shared Beast Calc - Successful login" },
        '-1': { name: "UserNotRegisteredOrBlocked", value: -1, description: "Shared Beast Calc - Failed to login with the AutoURL as User is either not registered or blocked" },
        '-2': { name: "FailedAutoUrl", value: -2, description: "Shared Beast Calc - Failed to login for AutoURL" },
        '-3': { name: "FailedWithAutoUrl", value: -3, description: "Shared Beast Calc - Failed to login with the AutoURL" },
        '-5': { name: "UnauthorizedIp", value: -5, description: "Shared Beast Calc - Failed to login because of Unauthorized IP Address" },
        '-6': { name: "NoSessionRights", value: -6, description: "Shared Beast Calc - Failed to login because of no rights for the Session" },
        '-9': { name: "UrlExpired", value: -9, description: "Shared Beast Calc - Failed to login with the AutoURL as URL Expired" },
        '-10': { name: "CalculatorClosed", value: -10, description: "Shared Beast Calc - Failed to login with the AutoURL as the initiator has closed the calculator" },
        '-99999': { name: "SessionError", description: "Their is some Error! Not able to store your session." },
    }
};

var loginErrorCode = {
    properties: {
        '-99999': { name: "SessionError", description: "Their is some Error! Not able to store your session." },
        '-9': { name: "InvalidIp", description: "Invalid Ip Address." },
        '-8': { name: "UserLockedOutByHelpdesk", description: "Account is locked by help desk." },
        '-7': { name: "MaxLoginExceeded", description: "Maximum login exceeded." },
        '-6': { name: "PasswordMustChange", description: "Please change your password." },
        '-5': { name: "PasswordWrongUserLockedOut", description: "Your Account has been locked due to maximum attempt of wrong password." },
        '-4': { name: "PasswordWrongLastTime", description: "Invalid Password, You Have Last Attempt To Login." },
        '-3': { name: "PasswordWrong", description: "Invalid Password." },
        '-2': { name: "UserNotFound", description: "User is not registered. Please register first or contact us for further assistance." },
    }
};

// Called when an exception is occured during Token Authentication
function ShowExceptionPopUp(exceptionMessage) {
    try {
        $('#exceptionMessage').html(exceptionMessage);
        $('#exceptionModal').modal({ keyboard: false, backdrop: 'static' });
    }
    catch (err) {
        console.log("popup error.");
    }
}

Array.prototype.remove = function (element, array) {
    for (var i = this.length - 1; i >= 0; i--) {
        if (this[i] === element) {
            this.splice(i, 1);
            if (!array)
                break;
        }
    }
    return this;
};

// Switches name with value. Called on Focus change of text field.
function switchValuesForNameAndDisplay(element) {
    var valFrmTitle = element.attr("name");
    if (element.attr("title") != "datepick" && element.hasClass("priceWidget") == false && element.hasClass("termWidget") == false && element.hasClass("basisWidget") == false && element.hasClass("inputDisable") == false) {
        var elementValue = element.val();
        element.val(valFrmTitle).attr("name", elementValue);
    }
}

// Common DatePicker change date events for all applications.
function datepickChangeDate(currentControl, appParameter) {
    try {
        var value = currentControl.val();
        if (value !== "") {
            appParameter.ElementType = "DDList";
            appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
            appParameter.ElementValue = value;
            signalrService.UpdateValueInApplication(appParameter);
        }
    } catch (err) {
        console.log("Function:datepick(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
    }

}

// Common texbox click events for all applications.
function textClick(currentControl, appParameter) {
    appParameter.ElementType = "DDList";
    appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
    appParameter.ElementValue = currentControl.val();

    if (currentControl.hasClass("priceWidget")) {
        try {
            display_PriceWidget(appParameter, currentControl);
        }
        catch (err) {
            console.log("Function:text_priceWidget(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
        }
    }
    else if (currentControl.hasClass("termWidget")) {
        try {
            display_TermWidget(appParameter, currentControl);
        }
        catch (err) {
            console.log("Function:text_termWidget(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
        }
    }
    else if (currentControl.hasClass("basisWidget")) {
        try {
            display_BasisWidget(appParameter, currentControl);
        }
        catch (err) {
            console.log("Function:text_basisWidget(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
        }
    }
}

// Common dropdown change events for all applications.
function selectChange(currentControl, appParameter) {
    try {
        var value = currentControl;
        if (value !== "" && value !== "notselected") {
            appParameter.ElementType = "DDList";
            appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
            appParameter.ElementValue = currentControl.val();
            signalrService.UpdateValueInApplication(appParameter);
        }

    } catch (err) {
        console.log("Function:select_DDList();  Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
    }

}

// Common Textbox key down event for all applications.
function textKeyDown(event, currentControl, appParameter) {
    if (!event.shiftKey) {
        var keyNumber = event.keyCode;
        if (currentControl.attr("title") === "datepick") event.preventDefault ? event.preventDefault() : event.returnValue = false;
        if (keyNumber === 13 || keyNumber === 9) {
            var hasChanged = currentControl.hasClass('changeDone');
            if (hasChanged) {
                currentControl.removeClass('changeDone');
                var value = currentControl.val();
                if (value !== "" || currentControl.attr("allownull") == "true") {
                    appParameter.ElementType = "DDList";
                    appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
                    if (value.indexOf("-") !== -1) {
                        currentControl[0].name = "-" + value.replace('-', '');
                        currentControl[0].value = "-" + value.replace('-', '');
                        appParameter.ElementValue = "-" + value.replace('-', '');
                    }
                    else {
                        appParameter.ElementValue = value;
                    }
                    signalrService.UpdateValueInApplication(appParameter);
                }
            }
        }
        else {

            if ((keyNumber > 47 && keyNumber < 58) || (keyNumber > 95 && keyNumber < 106) || keyNumber === 8 || keyNumber === 46) {
                currentControl.addClass('changeDone');
            }
            else if (event.key.indexOf('-') === 0) {
                if (currentControl.val().indexOf("-") !== -1)
                    event.preventDefault ? event.preventDefault() : event.returnValue = false;
                currentControl.addClass('changeDone');
            }
            else if (event.keyCode === 110 || event.keyCode === 190) {
                if (currentControl.val().indexOf(".") !== -1)
                    event.preventDefault ? event.preventDefault() : event.returnValue = false;
                currentControl.addClass('changeDone');
            }
            else if (keyNumber > 34 && keyNumber < 41) {
                event.returnValue = true;
            }
            else {
                event.preventDefault();
            }
        }
    }
    else {
        event.preventDefault();
    }
}

// Common button click event for all applications.
function buttonClick(currentControl, appParameter) {
    try {
        if (!currentControl.hasClass("inputDisable")) {
            var value = currentControl.attr("name");
            if (value === "1")
                currentControl.attr("name", "0");
            else
                currentControl.attr("name", "1");
            value = value === "1" ? 0 : 1;
            appParameter.ElementType = "DDList";
            appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
            appParameter.ElementValue = value;
            signalrService.UpdateValueInApplication(appParameter);
        }
    } catch (err) {
        console.log("Function:button_inputDisable(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
    }

}

// Common text box changes event for all applications.
function textChange(currentControl, appParameter) {
    try {
        if (currentControl.attr("title") !== "datepick") {
            appParameter.ElementType = "DDList";
            appParameter.ElementId = currentControl.attr("id").substring(currentControl.attr("id").lastIndexOf('_') + 1);
            appParameter.ElementValue = currentControl.val();
            signalrService.UpdateValueInApplication(appParameter);
            currentControl.name = currentControl.val();
        }
    } catch (err) {
        console.log("Function:textChange(); Error is : " + err.description + "; ElementId is :" + currentControl.attr("id") + "; Error number is " + err.number + "; Message :" + err.message);
    }

}

$(document).ready(function () {
    $("#logo").prop("href", config.logoUrl);
    $(".companyLogo").prop("href", config.logoUrl);

    $("#home").prop("href", config.indexUrl);
    $("#linkWhy").prop("href", config.indexWhyUrl);
    $("#linkHow").prop("href", config.indexHowUrl);
    $("#linkAdassNPass").prop("href", config.indexAdaasUrl);
    $("#linkOurClients").prop("href", config.indexClientUrl);

    $("#linkAppStore").prop("href", config.appstoreUrl);

    $("#linkCompany").prop("href", config.companyUrl);
    $("#linkIdeafactory").prop("href", config.companyIdeaUrl);
    $("#linkMedia").prop("href", config.companyMediaUrl);
    $("#linkManagementTeam").prop("href", config.companyManagementUrl);

    $("#linkProductAndServices").prop("href", config.productUrl);

    $("#linkBeastExcel").prop("href", config.beastExcelUrl);

    $("#linkDownloadBeastExcel").prop("href", config.beastExcelDownloadUrl);

    $("#linkTechNDevZone").prop("href", config.techDevZoneUrl);
    $("#linkBeastFramework").prop("href", config.devZoneBeastUrl);
    $("#linkCloudArchitecture").prop("href", config.devZoneCloudUrl);
    $("#linkBeastSDK").prop("href", config.devZoneBeastSdkUrl);

    $("#linkSampleView").prop("href", config.sampleApp);

    $(".contactUs").prop("href", config.contactUrl);
    $("#linkCareer").prop("href", config.careerUrl);

    RemoveExcelDownloadDropDown();
});

function RemoveExcelDownloadDropDown() {
    if (window.location.hostname == "www.thebeastapps.com") {
        $("#linkBeastExcel").next().remove();
    }
}

//Used to remove all cookie.
function deleteAllCookies() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}