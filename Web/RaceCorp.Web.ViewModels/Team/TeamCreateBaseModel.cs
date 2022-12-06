namespace RaceCorp.Web.ViewModels.Team
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class TeamCreateBaseModel
    {
        [Required]
        [DisplayName("Team name")]
        [StringLength(maximumLength: 20, ErrorMessage = InvalidTeamNameLenghMessage, MinimumLength = 2)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DisplayName("Town")]
        [StringLength(maximumLength: 20, ErrorMessage = TownNameLenghtError, MinimumLength = 2)]
        public string TownName { get; set; }

        public IFormFile Logo { get; set; }

        public string CreatorId { get; set; }
    }
}
