namespace RaceCorp.Web.ViewModels.ApplicationUsers
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ApplicationUserProfilePictureUploadModel
    {
        public string UserId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
}
