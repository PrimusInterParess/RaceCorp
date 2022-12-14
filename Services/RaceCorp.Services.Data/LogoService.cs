namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Messages;

    public class LogoService : ILogoService
    {
        private readonly IFileService fileService;
        private readonly IRepository<Logo> logoRepo;
        private readonly IWebHostEnvironment environment;

        public LogoService(
            IFileService fileService,
            IRepository<Logo> logoRepo,
            IWebHostEnvironment environment)
        {
            this.fileService = fileService;
            this.logoRepo = logoRepo;
            this.environment = environment;
        }

        public async Task<Logo> ProccessingData(IFormFile logoInputFile, string userId)
        {
            var extension = this.fileService.ValidateFile(logoInputFile, GlobalConstants.Image);

            if (extension == null)
            {
                throw new ArgumentNullException(InvalidImageMessage);
            }

            var logoRoothPath = $"{this.environment.WebRootPath}/{ImageParentFolderName}";

            var logoDto = new Logo()
            {
                Extension = extension,
                ApplicationUserId = userId,
                ParentFolderName = ImageParentFolderName,
                ChildFolderName = LogosFolderName,
            };

            await this.fileService
                .SaveFileIntoFileSystem(
                   logoInputFile,
                   logoRoothPath,
                   LogosFolderName,
                   logoDto.Id,
                   extension);

            await this.logoRepo.AddAsync(logoDto);

            return logoDto;
        }
    }
}
