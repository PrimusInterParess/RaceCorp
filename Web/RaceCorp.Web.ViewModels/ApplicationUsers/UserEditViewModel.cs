﻿namespace RaceCorp.Web.ViewModels.ApplicationUsers
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

        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(GlobalIntValues.DescriptionMaxLegth, ErrorMessage = GlobalErrorMessages.StringLengthError, MinimumLength = GlobalIntValues.DescriptionMinLegth)]
        public string About { get; set; }

        public virtual IFormFile UserProfilePicture { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserEditViewModel>().ForMember(x => x.Town, opt
                   => opt.MapFrom(x => x.Town.Name));
        }
    }
}
