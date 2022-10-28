namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

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

        public LogoService(
            IFileService fileService,IRepository<Logo> logoRepo)
        {
            this.fileService = fileService;
            this.logoRepo = logoRepo;
        }

        public async Task<Logo> ProccessingData(IFormFile logoInputFile, string userId, string imagePath)
        {
            var extension = this.fileService.ValidateFile(logoInputFile, GlobalConstants.Image);

            if (extension == null)
            {
                throw new ArgumentNullException(InvalidImageMessage);
            }

            var logoDto = new Logo()
            {
                Extension = extension,
                UserId = userId,
            };

            await this.fileService
                .SaveFileIntoFileSystem(
                   logoInputFile,
                   imagePath,
                   LogosFolderName,
                   logoDto.Id,
                   extension);

            await this.logoRepo.AddAsync(logoDto);

            return logoDto;
        }
    }
}
