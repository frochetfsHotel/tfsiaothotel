using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class StateVM
    {
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}