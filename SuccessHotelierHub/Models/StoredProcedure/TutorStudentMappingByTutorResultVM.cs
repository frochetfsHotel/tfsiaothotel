using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class TutorStudentMappingByTutorResultVM
    {
        public Guid Id { get; set; }

        public Guid TutorId { get; set; }

        public Guid StudentId { get; set; }

        public int? UserId { get; set; }

        public Guid UserGUID { get; set; }
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}