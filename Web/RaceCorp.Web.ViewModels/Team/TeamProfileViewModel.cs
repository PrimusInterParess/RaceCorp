namespace RaceCorp.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Request;
    using RaceCorp.Web.ViewModels.User;

    public class TeamProfileViewModel : IMapFrom<Team>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LogoImagePath { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public bool IsMember { get; set; }

        public bool RequestedJoin { get; set; }

        public string TownName { get; set; }

        public bool CurrentUserIsOwner { get; set; } = false;

        public ICollection<TeamMember> TeamMembers { get; set; }

        public ICollection<RequestModel> JoinRequests { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Team, TeamProfileViewModel>()
                                .ForMember(x => x.JoinRequests, opt => opt.MapFrom(x => x.ApplicationUser.Requests.Where(r => r.Type == GlobalConstants.RequestTypeTeamJoin)));
            ;
        }
    }
}
