using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class TimeFrameDetailViewModel
    {
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "From Hour")]
        [Required(ErrorMessage = "From Hour is required")]
        [Numeric(ErrorMessage="From Hour requires a number")]
        public short FromHour { get; set; }

        [Display(Name = "To Hour")]
        [Required(ErrorMessage = "To Hour is required")]
        [Numeric(ErrorMessage = "To Hour requires a number")]
        public short ToHour { get; set; }
        public bool FromChild { get; set; }
        public string ErrorMessage { get; set; }
    }
}
