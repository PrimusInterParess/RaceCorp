namespace RaceCorp.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;
    using static RaceCorp.Web.ViewModels.Constants.StringValues;

    public class SearchInputModel
    {
        public string Area { get; set; }

        [Required]
        [Display(Name = DisplayName)]
        [StringLength(DefaultStrMaxValueSearch, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]

        public string QueryInput { get; set; }
    }
}
