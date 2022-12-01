namespace RaceCorp.Web.ViewModels.Team
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class TeamRemoveMemberModel : IMapFrom<Team>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool CurrentUserIsOwner { get; set; } = false;

        public string ApplicationUserId { get; set; }

        public ICollection<TeamMemberEditModel> TeamMembers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Team, TeamRemoveMemberModel>()
                                .ForMember(x => x.TeamMembers, opt => opt.MapFrom(x => x.TeamMembers.Where(tm => tm.Id != x.ApplicationUserId)));
        }
    }
}
