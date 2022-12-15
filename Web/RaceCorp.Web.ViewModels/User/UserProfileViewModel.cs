namespace RaceCorp.Web.ViewModels.User
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Request;

    public class UserProfileViewModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TownName { get; set; }

        public string Country { get; set; }

        public string About { get; set; }

        public string TeamId { get; set; }

        public string TeamName { get; set; }

        public string TeamLogoImagePath { get; set; }

        public string MemberInTeamId { get; set; }

        public string MemberInTeamName { get; set; }

        public string MemberInTeamLogoImagePath { get; set; }

        public string ProfilePicturePath { get; set; }

        public string LinkedInLink { get; set; }

        public string FacoBookLink { get; set; }

        public string GitHubLink { get; set; }

        public string TwitterLink { get; set; }

        public int RequestsCount { get; set; }

        public int UnreadMessages { get; set; }

        public bool IsConnected { get; set; } = false;

        public bool RequestedConnection { get; set; } = false;

        public bool CanMessageMe { get; set; } = false;

        public ICollection<RequestModel> ConnectRequest { get; set; }

        public ICollection<UserConnectionsViewModel> Connections { get; set; }

        public ICollection<CreatedRideBaseModel> CreatedRides { get; set; }

        public ICollection<CreatedRaceBaseModel> CreatedRaces { get; set; }

        public ICollection<UserRideBaseModel> Rides { get; set; }

        public ICollection<UserTraceBaseModel> Traces { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            // the last one to close the door!
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>()
                .ForMember(x => x.ProfilePicturePath, opt =>
                opt.MapFrom(x => x.ProfilePicturePath == null ?
                       "\\Images\\default\\Murgash.jpg" : x.ProfilePicturePath))
                .ForMember(x => x.TeamId, opt => opt.MapFrom(x => x.Team.Id))
                .ForMember(x => x.MemberInTeamId, opt => opt.MapFrom(x => x.MemberInTeam.Id))
                .ForMember(x => x.MemberInTeamLogoImagePath, opt => opt.MapFrom(x => x.MemberInTeam.LogoImagePath))
                .ForMember(x => x.MemberInTeamName, opt => opt.MapFrom(x => x.MemberInTeam.Name))
                 .ForMember(x => x.RequestsCount, opt => opt.MapFrom(x => x.Requests.Where(r => r.IsApproved == false).ToList().Count))
                .ForMember(x => x.ConnectRequest, opt => opt.MapFrom(x => x.Requests.Where(r => r.Type == GlobalConstants.RequestTypeConnectUser)))
                .ForMember(x => x.UnreadMessages, opt => opt.MapFrom(x => x.InboxMessages.Where(m => m.IsRead == false && x.Connections.Any(c => c.InterlocutorId == m.SenderId)).ToList().Count));
        }
    }
}
