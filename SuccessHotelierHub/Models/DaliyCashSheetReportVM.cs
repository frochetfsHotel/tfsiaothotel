using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class DailyCashSheet
    {
        public string Date { get; set; }
        public double SumOfTotalAmount { get; set; }
        public string Day { get; set; }
        public List<DaliyCashSheetReportVM> Result { get; set; }
    }


    public class DaliyCashSheetReportVM
    {
        //public int RowNum { get; set; }        
        public double TotalAmount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string Description { get; set; }
    }
}