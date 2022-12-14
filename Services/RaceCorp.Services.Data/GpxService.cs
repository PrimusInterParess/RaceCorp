namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;

    using static System.Net.Mime.MediaTypeNames;
    using static RaceCorp.Services.Constants.Common;
    using static RaceCorp.Services.Constants.Drive;
    using static RaceCorp.Services.Constants.Messages;

    public class GpxService : IGpxService
    {
        private readonly string[] allowedExtensions = new[] { "gpx" };
        private readonly IRepository<Gpx> gpxRepo;
        private readonly IGoogleDriveService googleDriveService;
        private readonly IFileService fileService;
        private readonly IWebHostEnvironment environment;

        public GpxService(
            IRepository<Gpx> gpxRepo,
            IGoogleDriveService googleDriveService,
            IFileService fileService,
            IWebHostEnvironment environment)
        {
            this.gpxRepo = gpxRepo;
            this.googleDriveService = googleDriveService;
            this.fileService = fileService;
            this.environment = environment;
        }

        public Gpx GetGpxById(string id)
        {
            return this.gpxRepo
                .AllAsNoTracking()
                .FirstOrDefault(f => f.Id == id);
        }

        public async Task<Gpx> ProccessingData(
            IFormFile file,
            string userId,
            string childrenFolderName)
        {
            if (file == null)
            {
                throw new ArgumentNullException(GpxFileRequired);
            }

            var extention = this.fileService.ValidateFile(file, GlobalConstants.Gpx);

            if (extention == null)
            {
                throw new ArgumentNullException(InvalidFileExtension + GpxFileRequired);
            }

            var gpxDto = new Gpx()
            {
                Extension = extention,
                ApplicationUserId = userId,
                ParentFolderName = GpxFolderName,
                ChildFolderName = childrenFolderName,
            };

            var gpxRootPath = $"{this.environment.WebRootPath}/{GpxFolderName}";

            try
            {
                await this.fileService
                    .SaveFileIntoFileSystem(
                    file,
                    gpxRootPath,
                    childrenFolderName,
                    gpxDto.Id,
                    extention);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var gpxFilePath = $"{gpxRootPath}/{childrenFolderName}/{gpxDto.Id}.{extention}";

            try
            {
                var googleId = await this.googleDriveService
               .UloadGpxFileToDrive(
               gpxFilePath,
               childrenFolderName);

                gpxDto.GoogleDriveId = googleId;
                gpxDto.GoogleDriveDirectoryId = DirectoryId;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            await this.gpxRepo.AddAsync(gpxDto);

            return gpxDto;
        }
    }
}
