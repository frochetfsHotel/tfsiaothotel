using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Models
{
    public class BreakFastReport
    {
        public string Date { get; set; }
        
        public List<SearchBreakFastReportResultVM> Results { get; set; }
    }
}