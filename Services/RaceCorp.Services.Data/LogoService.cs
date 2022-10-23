namespace RaceCorp.Services.Data
{
    using System.IO;
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class LogoService : ILogoService
    {
        private readonly IImageService imageService;

        public LogoService(IImageService imageService)
        {
            this.imageService = imageService;
        }

        public async Task<Logo> ProccessingData(IFormFile logoInputFile, string userId, string imagePath)
        {
            var extension = string.Empty;

            try
            {
                extension = Path.GetExtension(logoInputFile.FileName).TrimStart('.');
            }
            catch (Exception)
            {
                throw new Exception(LogoImageRequired);
            }

            var validateImageExtension = this.imageService.ValidateImageExtension(extension);

            if (validateImageExtension == false)
            {
                throw new Exception(InvalidImageExtension + extension);
            }

            if (logoInputFile.Length > 10 * 1024 * 1024)
            {
                throw new Exception(InvalidImageSize);
            }

            var logoDto = new Logo()
            {
                Extension = extension,
                UserId = userId,
            };

            await this.imageService
                .SaveImageIntoFileSystem(
                   logoInputFile,
                   imagePath,
                   LogosFolderName,
                   logoDto.Id,
                   extension);

            return logoDto;
        }
    }
}
