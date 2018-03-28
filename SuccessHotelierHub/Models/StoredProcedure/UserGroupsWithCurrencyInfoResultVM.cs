using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class UserGroupsWithCurrencyInfoResultVM
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]        
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Currency")]        
        public Guid CurrencyId { get; set; }

        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Currency Name")]
        public string CurrencyName { get; set; }

        [DisplayName("Conversion Rate")]
        public double ConversionRate { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}