namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Upload;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using RaceCorp.Common;
    using RaceCorp.Services.Data.Contracts;

    using static RaceCorp.Services.Constants.Drive;

    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly IWebHostEnvironment environment;

        public GoogleDriveService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<string> UloadGpxFileToDrive(
            string gpxFilePath,
            string uploadFileName)
        {
            string serviceAccountPath = null;

            if (this.environment.IsDevelopment())
            {
                serviceAccountPath = Path.GetFullPath(GlobalConstants.GoogleCredentialsFilePath);
            }
            else if (this.environment.IsProduction())
            {
                serviceAccountPath = this.environment.WebRootPath + GlobalConstants.GoogleCredentialsFilePath;
            }

            try
            {
                var credentials = GoogleCredential.FromFile(serviceAccountPath).CreateScoped(DriveService.ScopeConstants.Drive);
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = uploadFileName + ".gpx",
                    Parents = new List<string>() { DirectoryId },
                };

                string uploadFileId;
                try
                {
                    await using (var fsSource = new FileStream(gpxFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var request = service.Files.Create(fileMetadata, fsSource, "application/gpx+xml");
                        request.Fields = "*";
                        var result = await request.UploadAsync(CancellationToken.None);

                        if (result.Status == UploadStatus.Failed)
                        {
                            Console.WriteLine($"Error uploading file: {result.Exception.Message}");
                        }

                        uploadFileId = request.ResponseBody?.Id;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return uploadFileId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
