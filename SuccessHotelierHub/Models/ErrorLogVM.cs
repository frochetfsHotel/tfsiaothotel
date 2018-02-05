using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ErrorLogVM
    {
        public int? Id { get; set; }

        [DisplayName("Page Url")]
        public string PageUrl { get; set; }

        [DisplayName("Method Name")]
        public string MethodName { get; set; }

        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }

        [DisplayName("Target Site")]
        public string TargetSite { get; set; }

        [DisplayName("Stack Trace")]
        public string StackTrace { get; set; }

        [DisplayName("Inner Exception")]
        public string InnerException { get; set; }

        [DisplayName("Created DateTime")]
        public DateTime CreatedDateTime { get; set; }
    }
}