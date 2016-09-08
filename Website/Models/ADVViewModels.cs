using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections;

namespace Website.Models
{
    public class SearchResult
    {
        public string Keyword { get; set; }
        public List<DetailURLViewModels> URLs { get; set; }
        public List<IndexDetailModel> Links { get; set; }
        public List<KeywordListViewModels> RelativeKeywords { get; set; }
        public int PriceOrder { get; set; }
        public string FromPrice { get; set; }
        public string ToPrice { get; set; }
        public string Currency { get; set; }
    }
    #region campaign
    public class CampaignListViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }
        [Display(Name = "Campaign")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Display Times")]
        public int DisplayTimes { get; set; }

        [Display(Name = "CTR")]
        [DisplayFormat(DataFormatString = "{0:N0}%")]
        public decimal CTR { get; set; }

        [Display(Name = "Average CPD")]
        [DisplayFormat(DataFormatString = "${0:N1}")]
        public decimal AverageCPD { get; set; }

        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "${0:N0}")]
        public decimal Cost { get; set; }

        public string CompanyCode { set; get; }
    }
    public class DetailCampaignViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Campaign Name")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }
        public string CompanyCode { set; get; }
    }
    public class EditCampaignViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "You must name the campaign")]
        [Display(Name = "Campaign name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must name the budget")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} is invalid. {0} must be a numberic and use a full stop (.) for decimals.")]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public string Message { get; set; }
    }
    #endregion
    #region Keyword
    public class KeywordListViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Keyword")]
        public string Keyword { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Bid")]
        public decimal Bid { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Marching")]
        public Service.ADV.Marching Marching { get; set; }

        [Display(Name = "Negative march")]
        public List<Data.Device> NegativeMarchs { get; set; }

        [Display(Name = "Display Times")]
        public int DisplayTimes { get; set; }

        [Display(Name = "CTR")]
        [DisplayFormat(DataFormatString = "{0:N0}%")]
        public decimal CTR { get; set; }

        [Display(Name = "Average CPD")]
        [DisplayFormat(DataFormatString = "${0:N1}")]
        public decimal AverageCPD { get; set; }

        [DisplayFormat(DataFormatString = "${0:N0}")]
        public decimal Cost { get; set; }
    }
    public class DetailKeywordListViewModels
    {
        [Display(Name = "Display Page Order")]
        public int DisplayPageOrder { get; set; }

        [Display(Name = "Search queries")]
        public string SearchQuery { get; set; }

        [Display(Name = "Display Time")]
        public string DisplayTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Bid")]
        public decimal Bid { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
        [Display(Name = "Click action")]
        public bool ClickAction { get; set; }
        
        [Display(Name = "Search Location")]
        public string SearchLocation { get; set; }
    }
    public class DetailKeywordViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Keyword")]
        public string Keyword { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Bid")]
        public decimal Bid { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Marching")]
        public Service.ADV.Marching Marching { get; set; }

        [Display(Name = "Negative march")]
        public List<Data.Device> NegativeMarchs { get; set; }
    }
    public class EditKeywordViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Required]
        [Display(Name = "Keyword")]
        public string Keyword { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Bid")]
        public decimal Bid { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Marching")]
        public Service.ADV.Marching Marching { get; set; }

        [Display(Name = "Negative march")]
        public List<Data.Device> NegativeMarchs { get; set; }

        [Display(Name = "URLId")]
        public decimal URLId { get; set; }
        public string Message { get; set; }
    }
    #endregion
    public class CategoryListViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Display Times")]
        public int DisplayTimes { get; set; }

        [Display(Name = "CTR")]
        [DisplayFormat(DataFormatString = "{0:N0}%")]
        public decimal CTR { get; set; }

        [Display(Name = "Average CPD")]
        [DisplayFormat(DataFormatString = "${0:N1}")]
        public decimal AverageCPD { get; set; }

        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "${0:N0}")]
        public decimal Cost { get; set; }
    }
    public class URLListViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "URLs")]
        public string URL { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Display Times")]
        public int DisplayTimes { get; set; }

        [Display(Name = "CTR")]
        [DisplayFormat(DataFormatString = "{0:N0}%")]
        public decimal CTR { get; set; }

        [Display(Name = "Average CPD")]
        [DisplayFormat(DataFormatString = "${0:N1}")]
        public decimal AverageCPD { get; set; }

        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "${0:N0}")]
        public decimal Cost { get; set; }

        [Display(Name = "Display Devices")]
        public string DisplayDevices { get; set; }
    }
    public class DetailCategoryViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public string Message { get; set; }
    }
    public class DetailURLViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }

        [Display(Name = "URL")]
        public string URL { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public string Message { get; set; }
    }
    public class EditCategoryViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Required]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public decimal CampaignId { get; set; }
        public string Message { get; set; }
    }
    public class EditURLViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }
        [Required]
        [Display(Name = "URL")]
        public string URL { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Required]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public List<Service.ADV.DeviceTypeInput> Devices { get; set; }
        public List<Service.ADV.PriceInput> Prices { get; set; }
        public decimal CategoryId { get; set; }

        [Display(Name = "Display Currency")]
        public Nullable<decimal> DisplayCurrencyId { get; set; }

        [Display(Name = "Display Location")]
        public Nullable<decimal> DisplayLocationId { get; set; }

        [Display(Name = "Display Time")]
        public Nullable<decimal> DisplayTimeframeId { get; set; }

        [Display(Name = "Display Times per person")]
        public Nullable<int> DisplayTimes { get; set; }
        public string Message { get; set; }
    }
    public class DetailLinkViewModels
    {
        [Display(Name = "Code")]
        [Key]
        public decimal Id { get; set; }
        [Required]
        [Display(Name = "URL")]
        public string URL { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Device")]
        public decimal DeviceId { get; set; }

        [Display(Name = "Device")]
        public string DeviceName { get; set; }

        [Display(Name = "Unit")]
        public decimal UnitId { get; set; }

        [Display(Name = "Unit")]
        public string UnitName { get; set; }

        [Display(Name = "Area")]
        public decimal AreaId { get; set; }

        [Display(Name = "Area")]
        public string AreaName { get; set; }

        [Display(Name = "Time frame")]
        public decimal TimeFrameId { get; set; }

        [Display(Name = "Time frame")]
        public string TimeFrameName { get; set; }

        [Display(Name = "Display Number/Person")]
        public int DisplayNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Required]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
    }
}