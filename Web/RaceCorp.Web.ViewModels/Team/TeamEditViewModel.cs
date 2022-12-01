namespace RaceCorp.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class TeamEditViewModel : IMapFrom<Team>
    {
        public string Id { get; set; }

        [Required]
        [DisplayName("Team name")]
        [StringLength(maximumLength: 20, ErrorMessage = InvalidTeamNameLenghMessage, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = DescriptionLenghtErrorMessage, MinimumLength = 15)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Town")]
        [StringLength(maximumLength: 20, ErrorMessage = TownNameLenghtError, MinimumLength = 2)]
        public string TownName { get; set; }

        public string ApplicationUserId { get; set; }

        public IFormFile Logo { get; set; }
    }
}
