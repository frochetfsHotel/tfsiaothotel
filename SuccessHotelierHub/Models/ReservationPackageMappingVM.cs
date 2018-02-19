using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationPackageMappingVM
    {
        public Guid Id { get; set; }
        public Guid? ReservationId { get; set; }

        public Guid? PackageId { get; set; }
        public string PackageName { get; set; }
        public double? PackagePrice { get; set; }
        public string PackageDescription { get; set; }

        public Guid? PackageCalculationRatioId { get; set; }
        public string PackageCalculationRatio { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}