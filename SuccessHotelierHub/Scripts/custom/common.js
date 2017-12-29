var RootPath = ""; //Store Site Root Location.

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

function loadToastrSetting() {
    toastr.options = {
        closeButton: true,
        progressBar: false,
        preventDuplicates: true,
        showMethod: 'slideDown',
        timeOut: 4000
    };
}

function showToaster(message, toastrType) {
    loadToastrSetting();

    if (toastrType == ToasterType.SUCCESS) {
        toastr.success(ToasterTitle.SUCCESS, message);
    }
    else if (toastrType == ToasterType.ERROR) {
        toastr.error(ToasterTitle.ERROR, message);
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

function closeAlert() {
    $('.alert').fadeOut('slow');
}

// Fuction for scroll body at given element
function scrollToControl(control) {
    // alert($('#' + control).offset().top);
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

    //if (e.shiftKey || e.ctrlKey || e.altKey) {
    //    e.preventDefault();        
    //} else {        
        var key = e.keyCode;

        //key = 9; TAB Key. (Allow Tab Key)

        if (!((key == 9) || (key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
    //}
}

function onlyNumeric(event) {
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}

function GetDate(objDate, dateFormat) {    
    var d = objDate;
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

function trim(item) {
    if (!IsNullOrEmpty(item)) { 
        return item.trim();
    }
    return item;
}