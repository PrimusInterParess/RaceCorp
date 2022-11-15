namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;

    using AutoMapper;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;

    public class UserInboxViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public ICollection<UserMessageViewModel> Connections { get; set; } = new HashSet<UserMessageViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserInboxViewModel>().ForMember(x => x.Connections, opt
                       => opt.MapFrom(x => x.Connections));
        }
    }
}
