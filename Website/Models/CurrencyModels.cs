using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class CurrencyDetailViewModel
    {
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "Code is required")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]

        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        public bool FromChild { get; set; }
        public string ErrorMessage { get; set; }
    }
}
