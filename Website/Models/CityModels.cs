using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class CityDetailViewModel
    {
        [Key]
        public decimal CityId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Country")]
        public decimal CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }
        public bool FromChild { get; set; }
        public string ErrorMessage { get; set; }
    }
}
