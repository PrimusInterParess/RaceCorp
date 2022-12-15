namespace RaceCorp.Web.ViewModels.Message
{
    using System.ComponentModel.DataAnnotations;

    using static RaceCorp.Web.ViewModels.Constants.Messages;
    using static RaceCorp.Web.ViewModels.Constants.NumbersValues;

    public class MessageInputModel
    {
        public string ReceiverProfilePicurePath { get; set; }

        public string ReceiverFirstName { get; set; }

        public string ReceiverLastName { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        [StringLength(DefaultDescriptionMaxValue, MinimumLength = DefaultStrMinValue, ErrorMessage = DefaultStringLengthErrorMessage)]
        public string Content { get; set; }
    }
}
