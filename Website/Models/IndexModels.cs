using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class IndexDetailModel
    {
        [Key]
        [Display(Name = "Code")]
        public decimal Id { get; set; }
        
        [Display(Description="URL")]
        [MaxLength(200)]
        [Required(ErrorMessage="URL is required!")]
        public string URL { get; set; }

        [Display(Name = "Analyse Children?")]
        public bool AnalysedChildren { get; set; }

        [Display(Name = "Overried?")]
        public bool IsOverride { get; set; }

        public bool UnanalysedPicture { get; set; }

        [Display(Name = "Price Rule")]
        [Required(ErrorMessage = "Price Rule is required!")]
        [MaxLength(100)]
        public string PriceRule { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required!")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required!")]
        public double? Price { get; set; }

        [Display(Name = "Short Description")]
        [Required(ErrorMessage = "Short Description is required!")]
        [MaxLength(1000)]
        public string ShortDescription { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm:ss}")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Description = "Picture")]
        [Required(ErrorMessage = "Picture is required!")]
        [MaxLength(200)]
        public string Picture { get; set; }

        public string Message { get; set; }
    }
}