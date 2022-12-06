namespace RaceCorp.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class ContactFormModel
    {
        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(DefaultStrMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Subject { get; set; }

        [Required]
        [StringLength(DefaultDescriptionMaxValue, MinimumLength = DefaultDescriptionMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Content { get; set; }
    }
}
