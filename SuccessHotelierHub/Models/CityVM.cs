using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class CityVM
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}