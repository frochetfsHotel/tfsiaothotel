using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class UsersActivityLogVM
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("Url")]        
        public string Url { get; set; }

        [DisplayName("Page Name")]        
        public string PageName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Ip Address")]
        public string IpAddress { get; set; }

        public DateTime? RecordedOn { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}