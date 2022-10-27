namespace RaceCorp.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class PictureUploadModel
    {
        [Required]
        public IFormFile Picture { get; set; }

        public string Type { get; set; }
    }
}
