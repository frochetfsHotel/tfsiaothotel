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

function loadToastrSetting() {
    toastr.options = {
        closeButton: true,
        progressBar: false,
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
        window.location.href = url;
    }
}
