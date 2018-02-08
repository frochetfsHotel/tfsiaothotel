using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class UserPageAccessRightsResultVM
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid PageId { get; set; }

        [DisplayName("Page Code")]        
        public string PageCode { get; set; }

        [DisplayName("Page Name")]        
        public string PageName { get; set; }

        [DisplayName("Page Description")]
        public string PageDescription { get; set; }

        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}