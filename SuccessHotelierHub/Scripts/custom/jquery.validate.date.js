//$(function () {
//    $.validator.methods.date = function (value, element) {
//        debugger;

//        //Here update given date format dd/MM/yyyy to MM/dd/yyyy.
//        value = ConvertDDMMYYYY_To_MMDDYYYY(value, DateSeprator.SLASH);

//        //jQuery has removed $.browser from 1.9 and their latest release.
//        var isChromeBrowser = /chrom(e|ium)/.test(navigator.userAgent.toLowerCase());
//        //if ($.browser.webkit) {
//        if (isChromeBrowser) {

//            //ES - Chrome does not use the locale when new Date objects instantiated:
//            var d = new Date();

//            return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
//        }
//        else {

//            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
//        }
//    };
//});

$(function () {
    // Replace the builtin US date validation with UK date validation
    $.validator.addMethod(
        "date",
        function (value, element) {
            var bits = value.match(/([0-9]+)/gi), str;
            if (!bits)
                return this.optional(element) || false;
            str = bits[1] + '/' + bits[0] + '/' + bits[2];
            return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
        },
        ""
    );
});
