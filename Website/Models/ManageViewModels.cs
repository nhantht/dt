using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace Admin.Models
{
    public class UserInfoViewModel
    {
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Logined Session?")]
        public string Session { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }
        [Display(Name = "IP")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public string IP { get; set; }
        [Display(Name = "Points")]
        public int Points { get; set; }
        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> ToDate { get; set; }
    }
    public class UpdateAccountViewModel
    {
        [Required]
        [Display(Name = "Phone")]
        [Key]
        public string Phone { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Full name")]
        public string Fullname { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Logined Session?")]
        public string Session { get; set; }

        public string Message { get; set; }
        [Display(Name = "Active from")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> InActiveFrom { get; set; }

        [Display(Name = "Active to")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> InActiveTo { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }
    }
    public class AccoutListViewModel
    {
        [Display(Name = "Phone")]
        [Key]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Full name")]
        public string Fullname { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Active from")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> InActiveFrom { get; set; }

        [Display(Name = "Active to")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public Nullable<DateTime> InActiveTo { get; set; }

        [Display(Name = "IP")]
        public string IP { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Status")]
        public short StatusId { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Points")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Points { get; set; }
    }
}