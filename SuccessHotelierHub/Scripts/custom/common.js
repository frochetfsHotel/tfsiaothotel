//This is a global varibale - will be set once the form is dirty and reset when saved
var data_needs_saving = false;

//var validNavigation = false;

//var clicked = false;

$(document).ready(function (e) {
    setNavigation();
});

$(function () {

    LoadLeftSideMenu();

    $('.mydatepicker').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        allowInputToggle: true
    })
    .change(changeColorOfDatePickerLabel)
    .on('changeDate', changeColorOfDatePickerLabel);

    $('div.date .input-group-addon').on('click', function () {        
        $(this).prev('input[type="text"]').datepicker().focus();
    });

});

var RootPath = ""; //Store Site Root Location.

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};



var StatusCode = {
    SUCCESS: 200,
    FAILURE: 400
};

var Alert = {
    SUCCESS: "alert-success",
    ERROR: "alert-danger",
    INFO: "alert-info",
    WARNING: "alert-warning"
};

var ToasterType = {
    SUCCESS: "success",
    ERROR: "error",
    INFO: "info",
    WARNING: "warning"
};

var ToasterTitle = {
    SUCCESS: "Success!",
    ERROR: "Error!",
    INFO: "Info!",
    WARNING: "Warning!"
};

var ToasterTimeOut = {
    ONE_SECOND: 1000,
    FIVE_SECOND: 5000,
    TEN_SECOND: 10000,
    FIFTEEN_SECOND: 15000,
    TWENTY_SECOND: 20000,
    TWENTY_FIVE_SECOND: 25000,
    THIRTY_SECOND: 30000,
    THIRTY_FIVE_SECOND: 35000,
    FOURTY_SECOND: 40000,
    FOURTY_FIVE_SECOND: 45000,
    FIFTY_SECOND: 50000,
    FIFTY_FIVE_SECOND: 55000,
    ONE_MINUTE: 60000,
    FIVE_MINUTE: 300000,
};

var Delimeter = {
    BREAKLINE: "<br/>",
    NEWLINE: "\n",
    COMMA: ",",
    SEMICOLON: ";",
    PIPE: "|",
    DOUBLEPIPE: "||",
    DOT: ".",
    HASH: "#",
    SPACE: " "
};

var DateFormat = {
    DDMMYYYY: "dd/MM/yyyy",
    MMDDYYYY: "MM/dd/yyyy",
    YYYYMMDD: "yyyy/MM/dd"
};

var DateSeprator = {
    SLASH: "/",
    DASH: "-",
    DOT: "."
}

var Messages = {
    ERRORMESSAGE: "Unknown Error Occured. Server response not received."
};

function loadToastrSetting() {
    toastr.options = {
        tapToDismiss: true,
        closeButton: true,
        progressBar: false,
        preventDuplicates: true,
        showMethod: 'slideDown',
        timeOut: 4000
    };

}

function showToaster(message, toastrType) {
    //loadToastrSetting();
    loadToastrSetting_V2(ToasterTimeOut.TEN_SECOND);

    if (toastrType == ToasterType.SUCCESS) {
        //toastr.success(ToasterTitle.SUCCESS, message); //Remove "Success!" title.
        toastr.success(message);
    }
    else if (toastrType == ToasterType.ERROR) {
        //toastr.error(ToasterTitle.ERROR, message); //Remove "Error!" title.
        toastr.error(message);
    }
    else if (toastrType == ToasterType.INFO) {
        //toastr.info(ToasterTitle.INFO, message); //Remove "Info!" title.
        toastr.info(message);
    }
    else if (toastrType == ToasterType.WARNING) {
        //toastr.warnnig(ToasterTitle.WARNING, message); //Remove "Warning!" title.
        toastr.warnnig(message);
    }
}

function loadToastrSetting_V2(timeout) {

    if (IsNullOrEmpty(timeout) || timeout == "0") {
        timeout = ToasterTimeOut.ONE_MINUTE; //Default TimeOut 1 minute.
    }

    toastr.options = {
        tapToDismiss: true,
        closeButton: true,
        progressBar: false,
        preventDuplicates: true,
        showMethod: 'slideDown',
        timeOut: timeout
    };

}

function showToaster_V2(message, toastrType, timeout) {
    loadToastrSetting_V2(timeout);

    if (toastrType == ToasterType.SUCCESS) {
        toastr.success(ToasterTitle.SUCCESS, message);
    }
    else if (toastrType == ToasterType.ERROR) {
        toastr.error(message);
    }
    else if (toastrType == ToasterType.INFO) {
        toastr.info(ToasterTitle.INFO, message);
    }
    else if (toastrType == ToasterType.WARNING) {
        toastr.warnnig(ToasterTitle.WARNING, message);
    }
}

function showAlert(message, className) {
    $("#spMessage").html(message);
    var alertClass = "alert alert-dismissable " + className;
    $('#dvAlert').show().fadeIn();
    $("#dvAlert").removeClass();
    $('#dvAlert').addClass(alertClass);
    $("html, body").animate({ scrollTop: 30 }, 500);
}

function hideAlert() {
    setTimeout(function () { $('#dvAlert').fadeOut('slow') }, 5000);
}

function hideAlert_V2(timeout) {
    setTimeout(function () { $('#dvAlert').fadeOut('slow') }, timeout);
}

function closeAlert() {
    $('.alert').fadeOut('slow');
}

// Fuction for scroll body at given element
function scrollToControl(control) {
    $('html,body').animate({ scrollTop: $('#' + control).offset().top }, 500);
}


function IsNullOrEmpty(item) {
    if (item == null || item == undefined || item == "") {
        return true;
    }
    return false;
}

function showLoader() {
    $('#divLoader').show();
}

function hideLoader() {
    $('#divLoader').hide();
}

function validateEmail(email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (pattern.test(email)) {
        return true;
    }
    return false;
}


function redirectTo(url) {
    if (!IsNullOrEmpty(url)) {
        window.location.href = (RootPath + url);
    }
}


function onlyDigits(e) {
    var key = e.keyCode;

    //key = 9; TAB Key. (Allow Tab Key)

    if (!((key == 9) || (key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
        e.preventDefault();
    }
}

function onlyNumeric(event) {
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}


/*** Date Utility Functions START ****/
function GetDate(objDate, dateFormat) {
    var d = objDate;
    if (IsNullOrEmpty(d) || d == "Invalid Date") {
        return "";
    }

    var month, date, year;
    var MM, dd, yyyy;
    var fullDate = "";

    date = d.getDate();
    month = (d.getMonth() + 1); // : Month start with (0-11) e.g. Jan = 0, Feb = 1, Mar = 2 etc.
    year = d.getFullYear();

    //set initial value.
    yyyy = year;
    MM = month;
    dd = date;

    //Append '0' prefix in date.
    if (date >= 1 && date <= 9) {
        dd = ("0" + date);
    }

    //Append '0' prefix in month.
    if (month >= 1 && month <= 9) {
        MM = ("0" + month);
    }

    if (dateFormat == DateFormat.DDMMYYYY) {
        fullDate = dd + "/" + MM + "/" + yyyy;
    }
    else if (dateFormat == DateFormat.MMDDYYYY) {
        fullDate = MM + "/" + dd + "/" + yyyy;
    }
    else if (dateFormat == DateFormat.YYYYMMDD) {
        fullDate = yyyy + "/" + MM + "/" + dd;
    }
    return fullDate;
}

function ConvertDDMMYYYY_To_MMDDYYYY(inputDate, dateSeparator) {
    if (IsNullOrEmpty(inputDate)) { return ""; }
    if (IsNullOrEmpty(dateSeparator)) { dateSeparator = DateSeprator.SLASH; }


    var dateParts = inputDate.split(dateSeparator)

    var MM, dd, yyyy;

    dd = dateParts[0];
    MM = dateParts[1];
    yyyy = dateParts[2];

    return trim(MM + dateSeparator + dd + dateSeparator + yyyy);
}

function GetDateObject(inputDateString, dateSeparator) {
    if (IsNullOrEmpty(inputDateString)) { return null; }

    var dateParts = inputDateString.split(dateSeparator)

    var MM, dd, yyyy;

    dd = dateParts[0];
    MM = dateParts[1];
    yyyy = dateParts[2];

    var dObj = new Date(Number(yyyy), (Number(MM) - 1), Number(dd));

    return dObj;
}

function GetDayOfWeek(day) {
    var weekDay = "";
    switch (day) {
        case 0:
            weekDay = "Sunday";
            break;
        case 1:
            weekDay = "Monday";
            break;
        case 2:
            weekDay = "Tuesday";
            break;
        case 3:
            weekDay = "Wednesday";
            break;
        case 4:
            weekDay = "Thursday";
            break;
        case 5:
            weekDay = "Friday";
            break;
        case 6:
            weekDay = "Saturday";
            break;
        default:
            weekDay = "";
            break;
    }

    return weekDay;
}


function GetFullMonthName(monthNo) {
    var monthName = "";
    switch (monthNo) {
        case 1:
            monthName = "January";
            break;
        case 2:
            monthName = "February";
            break;
        case 3:
            monthName = "March";
            break;
        case 4:
            monthName = "April";
            break;
        case 5:
            monthName = "May";
            break;
        case 6:
            monthName = "June";
            break;
        case 7:
            monthName = "July";
            break;
        case 8:
            monthName = "August";
            break;
        case 9:
            monthName = "September";
            break;
        case 10:
            monthName = "October";
            break;
        case 11:
            monthName = "November";
            break;
        case 12:
            monthName = "December";
            break;
        default:
            monthName = "";
            break;
    }
    return monthName;
}


function GetShortMonthName(monthNo) {
    var monthName = "";
    switch (monthNo) {
        case 1:
            monthName = "Jan";
            break;
        case 2:
            monthName = "Feb";
            break;
        case 3:
            monthName = "Mar";
            break;
        case 4:
            monthName = "Apr";
            break;
        case 5:
            monthName = "May";
            break;
        case 6:
            monthName = "Jun";
            break;
        case 7:
            monthName = "Jul";
            break;
        case 8:
            monthName = "Aug";
            break;
        case 9:
            monthName = "Sep";
            break;
        case 10:
            monthName = "Oct";
            break;
        case 11:
            monthName = "Nov";
            break;
        case 12:
            monthName = "Dec";
            break;
        default:
            monthName = "";
            break;
    }
    return monthName;
}

function ParseJsonDate(date) {
    if (!IsNullOrEmpty(date)) {
        var dt = new Date(parseInt(date.substr(6)));

        return GetDate(dt, DateFormat.DDMMYYYY);
    }
    return "";
}

function ParseJsonDateTime(date) {
    if (!IsNullOrEmpty(date)) {
        var dt = new Date(parseInt(date.substr(6)));

        return dt.format("dd/mm/yyyy HH:MM:ss");
    }
    return "";
}

function GetTime(dateObject) {
    if (IsNullOrEmpty(dateObject)) {
        dateObject = new Date();
    }

    var datetext = dateObject.toTimeString();

    datetext = datetext.split(' ')[0];

    return datetext;
}

function GetDateObjectFromTime(time) {
    var today = new Date();
    var year = today.getFullYear();
    var month = today.getMonth();
    var day = today.getDate();

    var hour = 0, minute = 0;

    if(!IsNullOrEmpty(time)){
        var timeArr = time.split(':');

        hour = parseInt(timeArr[0]);
        minute = parseInt(timeArr[1]);
    }
    var dtObj = new Date(year, month, day, hour, minute, 0, 0);

    return dtObj;
}

/*** Date Utility Functions END ****/

function trim(item) {
    if (!IsNullOrEmpty(item)) {
        return item.trim();
    }
    return item;
}

function RemoveLastCharacter(item, char) {
    if (!IsNullOrEmpty(item)) {
        var lastChar = trim(item).slice(-1);
        if (lastChar == char) {
            item = trim(item).slice(0, -1);
        }
    }

    return item;
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}


/**** Load Left Side Menu ***/

function setCurrentMenu(menu, redirectUrl) {
    setCookie('TopMenu', menu, 1);

    setTimeout(function () {
        redirectTo(redirectUrl);
    }, 200);

}

function LoadLeftSideMenu() {
    var menu = getCookie('TopMenu');

    if (!IsNullOrEmpty(menu)) {
        hideAllLeftSideMenu();
        $('.topMenu').removeClass('active');

        if (menu == "Dashboard") {
            $('#MenusDashboard').show();
            $('#menuDashboard').addClass('active');
        }
        else if (menu == "Reservation") {
            $('#MenusReservation').show();
            $('#menuReservation').addClass('active');
        }
        else if (menu == "FrontDesk") {
            $('#MenusFrontDesk').show();
            $('#menuFrontDesk').addClass('active');
        }
        else if (menu == "Cashiering") {
            $('#MenusCashiering').show();
            $('#menuCashiering').addClass('active');
        }
        else if (menu == "MasterScreen") {
            $('#MenusMasterScreens').show();
            $('#menuMasterScreen').addClass('active');
        }
        else if (menu == "ManageStudents") {
            $('#MenusManageStudents').show();
            $('#menuManageStudents').addClass('active');
        }
        else if (menu == "ReportsScreen") {
            $('#MenusReportsScreen').show();
            $('#menuReportsScreen').addClass('active');
        }
        else if (menu == "LoggedInUserScreen") {
            $('#MenusLoggedInUserScreen').show();
            $('#menuLoggedInUserScreen').addClass('active');
        }
        else if (menu == "ChangePassword") {
            $('#MenusChangePasswordScreen').show();
            $('#menuChangePasswordScreen').addClass('active');
        }
    }
}

function hideAllLeftSideMenu() {
    $('.leftsidemenu').hide();
}

function goToByScroll(id) {
    // Scroll
    $('html,body').animate({
        scrollTop: $("#" + id).offset().top
    }, 'slow');
}

function goToSpecificElement(id) {

    var row = document.getElementById(id);
    row.scrollIntoView(true, { behavior: "smooth", block: "end", inline: "nearest" });
}

function arrayContains(searchItem, array) {
    return (array.indexOf(searchItem) > -1) ? true : false;
}

function changeColorOfDatePickerLabel(element) {
    if (element) {
        if (!IsNullOrEmpty($(element)[0].target.value)) {
            $('label[for="' + $(element)[0].target.id + '"]').addClass("label-bold");
        } else {
            $('label[for="' + $(element)[0].target.id + '"]').removeClass("label-bold");
        }
    }
}

function RemoveLineBreak(data) {
    if (data != null && data != undefined && data != "") {
        return data.replace(/(\r\n|\n|\r)/gm, "");
    }
    return "";
}

function FormatNumberWithTwoDecimal(amount) {
    if (amount != null && amount != '' && amount != undefined) {
        return Number(amount).toFixed(2);
    }
    return "0.00";
}

function RemoveArrayItemByValue(itemToRemove, arr) {
    if (arr != null && arr.length > 0) {
        arr.splice($.inArray(itemToRemove, arr), 1);
    }

    return arr;
}


/** Boostrap Modal ***/

function OpenModal(modelId, isAllowToCloseModalFromOutsideClick) {

    if (isAllowToCloseModalFromOutsideClick) {
        $('#' + modelId).modal('toggle');
    } else {
        $('#' + modelId).modal({
            keyboard: false,
            //backdrop: 'static',
            //backdrop: false,
            show: true
        })
    }
}

function CloseModal(modelId) {
    $('#' + modelId).modal('hide');
}



function ShowErrorMessage(statusCode, errorMessage) {
    if (statusCode == "401") {
        showToaster("You are not authorize to perform this action", ToasterType.ERROR);
    } else {
        showToaster(errorMessage, ToasterType.ERROR);
    }
}

function openPageInNewWindow(url) {
    window.open(url, "_blank");
}

function Contains(string, searchText) {
    return string.includes(searchText);
}

function html_entity_decode(message) {
    return message.replace(/[<>'"]/g, function (m) {
        return '&' + {
            '\'': 'apos',
            '"': 'quot',
            '&': 'amp',
            '<': 'lt',
            '>': 'gt',
        }[m] + ';';
    });
}
function parseBoolean(value) {
    if (value == 'True' || value == 'true' || value == '1') {
        return true;
    } else {
        return false;
    }
}

function IsValidCreditCardNo(number) {
    if (!IsNullOrEmpty(number)) {
        if (number.length != 16) {
            return false;
        }
        return true;
    }
    return false;
}

function IsValidCVVNo(number) {
    if (!IsNullOrEmpty(number)) {
        if (number.length == 3 || number.length == 4) {
            return true;
        }
        return false;
    }
    return false;
}

function ConvertToNegative(num) {
    return -Math.abs(num);
}

//function AutoLogOut() {
//    console.log('auto log out');
//    debugger;
//    //if (window.performance) {
//    //    console.info("window.performance works fine on this browser");
//    //}
//    //if (window.performance.navigation.type == PerformanceNavigation.TYPE_RELOAD) {
//    //    console.info("This page is reloaded.");
//    //}
//    //else if (window.performance.navigation.type == PerformanceNavigation.TYPE_NAVIGATE) {
//    //    console.info("This page is navigated.");
//    //}
//    //else if (window.performance.navigation.type == PerformanceNavigation.TYPE_BACK_FORWARD) {
//    //    console.info("This page is back forward with history.");
//    //}
//    //else if (window.performance.navigation.type == PerformanceNavigation.TYPE_RESERVED) {
//    //    console.info("any other way.");
//    //}
//    //else {
//    //    debugger;
//    //    console.info("This page is not reloaded");
//    //}

//    if (!validNavigation) {
//        callServerForBrowserCloseEvent();
//    }

//}

//function callServerForBrowserCloseEvent() {
//    //…...Do you operation here

//    $.ajax({
//        url: '/Account/SignOut',
//        async: false,
//        contentType: "application/json; charset=utf-8",
//        type: "GET",
//        success: function (data) {
//            alert(data);
//        },
//        error: function (x, y, z) {
//            alert(x.responseText + "  " + x.status);
//        }
//    });
//}

//function wireUpWindowUnloadEvents() {
//    /*
//    * List of events which are triggered onbeforeunload on IE
//    * check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
//    */


//    /**
//  * For a list of events that triggers onbeforeunload on IE
//  * check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
//  *
//  * onbeforeunload for IE and chrome
//  * check http://stackoverflow.com/questions/1802930/setting-onbeforeunload-on-body-element-in-chrome-and-ie-using-jquery
//  */
//    //var dont_confirm_leave = 0; //set dont_confirm_leave to 1 when you want the user to be able to leave without confirmation
//    //var leave_message = 'You sure you want to leave?'
//    //function goodbye(e) {
//    //    debugger;
//    //    if (!validNavigation) {
//    //        if (dont_confirm_leave !== 1) {
//    //            if (!e) e = window.event;
//    //            //e.cancelBubble is supported by IE - this will kill the bubbling process.
//    //            e.cancelBubble = true;
//    //            e.returnValue = leave_message;
//    //            //e.stopPropagation works in Firefox.
//    //            if (e.stopPropagation) {
//    //                e.stopPropagation();
//    //                e.preventDefault();
//    //            }
//    //            //return works for Chrome and Safari
//    //            return leave_message;
//    //        }
//    //    }
//    //}
//    //window.onbeforeunload = goodbye;


//    //window.onbeforeunload = function (e) {

//    //    // Get the first available attribute
//    //    //evt = window.event || event;

//    //    //var y = evt.clientY || evt.screenY || evt.pageY;
//    //    debugger;

//    //    var posx = 0;
//    //    var posy = 0;
//    //    if (!e) var e = window.event;
//    //    if (e.pageX || e.pageY) {
//    //        posx = e.pageX;
//    //        posy = e.pageY;
//    //    }
//    //    else if (e.clientX || e.clientY) {
//    //        posx = e.clientX + document.body.scrollLeft
//    //            + document.documentElement.scrollLeft;
//    //        posy = e.clientY + document.body.scrollTop
//    //            + document.documentElement.scrollTop;
//    //    }
//    //    alert(posx + ", " + posy)

//    //    //if (y < 0) {
//    //    //    validNavigation = false;
//    //    //    AutoLogOut();
//    //    //}        
//    //    return;
//    //}

//    //window.onload = function CallbackFunction(event) {
//    //    debugger;
//    //    if (window.event) {

//    //        if (window.event.clientX < 40 && window.event.clientY < 0) {

//    //            validNavigation = true;


//    //        } else {

//    //            validNavigation = true;
//    //        }

//    //    } else {

//    //        if (event.currentTarget.performance.navigation.type == 2) {

//    //            validNavigation = true;

//    //        }
//    //        if (event.currentTarget.performance.navigation.type == 1) {

//    //            validNavigation = true;
//    //        }
//    //    }
//    //}

//    window.onbeforeunload = function (e) {
//        debugger;
//        if (validNavigation == false) {
//            console.log("close event");
//        }
//    };

//    //window.onload = function (e) {
//    //    debugger;
//    //    console.log("load event");
//    //    if (window.event) {
//    //        validNavigation = true;
//    //    } else {
//    //        if (event.currentTarget.performance.navigation.type == 2) {
//    //            validNavigation = true;
//    //        }
//    //        if (event.currentTarget.performance.navigation.type == 1) {
//    //            validNavigation = true;
//    //        }
//    //    }
//    //};

//    // Attach the event keypress to exclude the F5 refresh
//    $(document).on('keypress', function (e) {
//        debugger;
//        if (e.keyCode == 116) {
//            validNavigation = true;
//        }
//    });

//    // Attach the event click for all links in the page
//    $(document).on("click", "a", function () {
//        debugger;
//        validNavigation = true;
//    });

//    // Attach the event submit for all forms in the page
//    $(document).on("submit", "form", function () {
//        debugger;
//        validNavigation = true;
//    });

//    // Attach the event click for all inputs in the page
//    $(document).on("click", "input[type='submit']", function () {
//        debugger;
//        validNavigation = true;
//    });

//    $(document).on("click", "button[type='submit']", function () {
//        debugger;
//        validNavigation = true;
//    });

//}



//function CheckBrowser() {
//    if (clicked == false) {
//        //Browser closed
//    }
//    else {
//        //redirected 
//        clicked = false;
//    }
//}

//function bodyUnload() {
    
//    debugger;
//    if (clicked == false)//browser is closed
//    {
//        var request = GetRequest();

//        request.open("GET", "/Account/SignOut", false);
//        request.send();
//        alert('This is close event.');
//    }
//}
//function GetRequest() {
//    var xmlhttp;
//    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
//        xmlhttp = new XMLHttpRequest();
//    }
//    else {// code for IE6, IE5
//        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
//    }
//    return xmlhttp;
//}



/*** Seconds to Time Duration *******/


const intervalToLevels = (interval, levels) => {
    const cbFun = (d, c) => {
        let bb = d[1] % c[0],
          aa = (d[1] - bb) / c[0];
        aa = aa > 0 ? aa + c[1] : '';

        return [d[0] + aa, bb];
    };

    let rslt = levels.scale.map((d, i, a) => a.slice(i).reduce((d, c) => d * c))
      .map((d, i) => ([d, levels.units[i]]))
      .reduce(cbFun, ['', interval]);
    return rslt[0];
};

const TimeLevels = {
    scale: [365, 30, 24, 60, 60, 1],
    units: ['y ', 'M ', 'd ', 'h ', 'm ', 's ']
};
const secondsToString = interval => intervalToLevels(interval, TimeLevels);

function secondsToDuration(seconds) {

    seconds = parseInt(seconds, 10);

    var years = Math.floor(seconds / (3600 * 24 * 30 * 365));
    seconds -= years * 3600 * 24 * 30 * 365;

    var months = Math.floor(seconds / (3600 * 24 * 30));
    seconds -= months * 3600 * 24 * 30;

    var days = Math.floor(seconds / (3600 * 24));
    seconds -= days * 3600 * 24;

    var hrs = Math.floor(seconds / 3600);
    seconds -= hrs * 3600;

    var mnts = Math.floor(seconds / 60);
    seconds -= mnts * 60;
    return (years + " yrs, " + months + " months, " + days + " days, " + hrs + " Hrs, " + mnts + " Minutes, " + seconds + " Seconds");
}


function SecondsToHHMMSS(totalSeconds) {
    var hours = Math.floor(totalSeconds / 3600);
    var minutes = Math.floor((totalSeconds - (hours * 3600)) / 60);
    var seconds = totalSeconds - (hours * 3600) - (minutes * 60);

    // round seconds
    seconds = Math.round(seconds * 100) / 100

    var result = (hours < 10 ? "0" + hours : hours);
    result += ":" + (minutes < 10 ? "0" + minutes : minutes);
    result += ":" + (seconds < 10 ? "0" + seconds : seconds);
    return result;
}

/*** Seconds to Time Duration *******/


function setNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    if (Contains(path, '/EditIndividualProfile/')) {
        path = path.replace("/EditIndividualProfile/", "/IndividualProfileList");
    }
    else if (Contains(path, '/EditBulkReservation/')) {
        path = path.replace("/EditBulkReservation/", "/BulkReservation");
    }
    else if (Contains(path, '/EditAdmin/')) {
        path = path.replace("/EditAdmin/", "/ListAdmin");
    }
    else if (Contains(path, '/Rate/Edit/')) {
        path = path.replace("/Rate/Edit/", "/Rate/ManagePrice");
    }
    else if (Contains(path, '/Tutor/ViewActivity/')) {
        path = path.replace("/Tutor/ViewActivity/", "/Tutor/ViewStudent");
    }
    else if (Contains(path, '/Tutor/ReservationLog/')) {
        path = path.replace("/Tutor/ReservationLog/", "/Tutor/ViewStudent");
    }
    else if (Contains(path, '/Tutor/FolioLog/')) {
        path = path.replace("/Tutor/FolioLog/", "/Tutor/ViewStudent");
    }
    else if(Contains(path, '/Edit/')) {
        path = path.replace("/Edit/", "/List");
    }

    $(".sidebar-menu a").each(function () {        
        var href = $(this).attr('href');
        if (path.substring(0, href.length) === href) {            
            $(this).closest('li').addClass('active');
            $(this).parents('li.treeview').addClass('menu-open');
            $(this).parents('ul.treeview-menu').css({ display: 'block' });
        }
    });
}

