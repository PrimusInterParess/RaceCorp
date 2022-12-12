namespace RaceCorp.Web.Areas.Administration.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Web.Areas.Administration.Infrastructure.Contracts;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class AdminFileService : IAdminFileService
    {
        private readonly string[] imageExtensions = new[] { "jpg", "png", "gif" };

        private readonly IDeletableEntityRepository<Image> imageRepo;

        public AdminFileService(IDeletableEntityRepository<Image> imageRepo)
        {
            this.imageRepo = imageRepo;
        }

        public async Task ProccessingImageData(IFormFile file, string imageName, string userId, string roothPath, string childrenFolderName)
        {
            var extension = this.ValidateFile(file, GlobalConstants.Image);

            if (extension == null)
            {
                throw new ArgumentNullException(InvalidImageMessage);
            }

            var imageDto = new Image()
            {
                ParentFolderName = ImageParentFolderName,
                ChildFolderName = childrenFolderName,
                Extension = extension,
                ApplicationUserId = userId,
                CreatedOn = DateTime.Now,
                Name = imageName,
            };

            var imageRoothPath = $"{roothPath}/{ImageParentFolderName}";

            await this.SaveFileIntoFileSystem(
                   file,
                   imageRoothPath,
                   childrenFolderName,
                   imageDto.Id,
                   extension);

            await this.imageRepo.AddAsync(imageDto);
            await this.imageRepo.SaveChangesAsync();
        }

        private async Task SaveFileIntoFileSystem(IFormFile file, string roothPath, string folderName, string dbId, string extension)
        {
            Directory.CreateDirectory($"{roothPath}/{folderName}/");

            var physicalPath = $"{roothPath}/{folderName}/{dbId}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        private string ValidateFile(IFormFile file, string expectedFileType)
        {
            string extention;

            extention = Path.GetExtension(file.FileName).TrimStart('.');

            if (expectedFileType == GlobalConstants.Image)
            {
                if (file.Length > 20 * 1024 * 1024)
                {
                    return null;
                }

                return this.imageExtensions
                    .FirstOrDefault(e => e == extention);
            }

            return null;
        }
    }
}
