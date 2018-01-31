using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class StudentDetailsForTutorMappingResultVM
    {
        public Guid UserGUID { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public bool IsMapped { get; set; }
        public Guid? TutorStudentMappingId { get; set; }
    }
}