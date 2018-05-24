using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class CompanyVM
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
    }
}