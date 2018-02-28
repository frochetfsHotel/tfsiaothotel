using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationPackageMappingVM
    {
        public Guid Id { get; set; }
        public Guid? ReservationId { get; set; }

        [DisplayName("Package")]
        [Required(ErrorMessage = "Please select package.")]
        public Guid? PackageId { get; set; }

        [DisplayName("Package Name")]
        public string PackageName { get; set; }

        [DisplayName("Price")]
        public double? PackagePrice { get; set; }

        [DisplayName("Description")]
        public string PackageDescription { get; set; }

        [DisplayName("Qty")]
        [Required(ErrorMessage = "Please enter qty.")]
        public int Qty { get; set; }

        [DisplayName("Posting Rhythm")]
        public string PostingRhythm { get; set; }

        [DisplayName("Begin Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Please select begin date.")]
        public DateTime? BeginDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Please select end date.")]
        public DateTime? EndDate { get; set; }

        public double? TotalAmount { get; set; }

        public Guid? PackageCalculationRatioId { get; set; }
        public string PackageCalculationRatio { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}