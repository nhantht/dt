using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class CountryDetailViewModel
    {
        [Key]
        public decimal CountryId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Telephone Code")]
        [Required(ErrorMessage = "Telephone Code is required")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string TelephoneCode { get; set; }

        [Display(Name = "City Number")]
        public int CityNumber { get; set; }
        public bool FromChild { get; set; }        
        public string ErrorMessage { get; set; }
    }
}
