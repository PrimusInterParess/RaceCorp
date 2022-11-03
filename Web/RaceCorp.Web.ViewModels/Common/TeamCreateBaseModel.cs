namespace RaceCorp.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    using RaceCorp.Data.Models;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class TeamCreateBaseModel
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = InvalidTeamNameLenghMessage, MinimumLength = 2)]
        public string? Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = DescriptionLenghtErrorMessage, MinimumLength = 15)]

        public string? Description { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = TownNameLenghtError, MinimumLength = 2)]
        public string? TownName { get; set; }

        public string CreatorId { get; set; }
    }
}
