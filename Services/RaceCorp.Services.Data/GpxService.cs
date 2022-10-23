namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
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

        public GpxService(
            IRepository<Gpx> gpxRepo,
            IGoogleDriveService googleDriveService)
        {
            this.gpxRepo = gpxRepo;
            this.googleDriveService = googleDriveService;
        }

        public async Task<Gpx> ProccessingData(
            IFormFile file,
            string userId,
            string inputModelname,
            string gxpFileRoothPath,
            string pathToServiceAccountKeyFile)
        {
            string extention;

            try
            {
                extention = Path.GetExtension(file.FileName).TrimStart('.');
            }
            catch (Exception)
            {
                throw new Exception(GpxFileRequired);
            }

            var validateFileExtention = this.ValidateExtension(extention);

            if (validateFileExtention == false)
            {
                throw new Exception(InvalidFileExtension + extention);
            }

            var gpxDto = new Gpx()
            {
                Extension = extention,
                UserId = userId,
            };

            try
            {
                await this.SaveIntoFileSystem(
                    file,
                    gxpFileRoothPath,
                    inputModelname,
                    gpxDto.Id,
                    extention);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var gxpFilePath = $"{gxpFileRoothPath}\\{inputModelname}\\{gpxDto.Id}.{extention}";

            var googleId = await this.googleDriveService
                .UloadGpxFileToDrive(
                gxpFilePath,
                pathToServiceAccountKeyFile,
                inputModelname,
                DirectoryId);

            gpxDto.GoogleDriveId = googleId;
            gpxDto.GoogleDriveDirectoryId = DirectoryId;

            return gpxDto;
        }

        public Task SaveAsyncIntoDb(Gpx fileData)
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveIntoFileSystem(
            IFormFile file,
            string filePath,
            string folderName,
            string fileDbId,
            string extension)
        {
            Directory.CreateDirectory($"{filePath}/{folderName}/");

            var physicalPath = $"{filePath}/{folderName}/{fileDbId}.{extension}";

            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);

            await file.CopyToAsync(fileStream);
        }

        public bool ValidateExtension(string extension)
        {
            return this.allowedExtensions.Any(x => extension.EndsWith(x));
        }
    }
}
