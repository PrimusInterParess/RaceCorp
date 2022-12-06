namespace RaceCorp.Web.ViewModels.User
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Mapping;

    public class UserEditViewModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [StringLength(GlobalIntValues.StringMaxLenth, ErrorMessage = GlobalErrorMessages.StringLengthError, MinimumLength = GlobalIntValues.StringMinLenth)]

        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalIntValues.StringMaxLenth, ErrorMessage = GlobalErrorMessages.StringLengthError, MinimumLength = GlobalIntValues.StringMinLenth)]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [StringLength(GlobalIntValues.StringMaxLenth, ErrorMessage = GlobalErrorMessages.StringLengthError, MinimumLength = GlobalIntValues.StringMinLenth)]
        public string Town { get; set; }

        [Required]
        [StringLength(GlobalIntValues.StringMaxLenth, ErrorMessage = GlobalErrorMessages.StringLengthError, MinimumLength = GlobalIntValues.StringMinLenth)]
        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;

        public string About { get; set; }

        [Url]
        public string LinkedInLink { get; set; }

        [Url]
        public string FacoBookLink { get; set; }

        [Url]

        public string GitHubLink { get; set; }

        public string TwitterLink { get; set; }

        public virtual IFormFile UserProfilePicture { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserEditViewModel>().ForMember(x => x.Town, opt
                   => opt.MapFrom(x => x.Town.Name));
        }
    }
}
