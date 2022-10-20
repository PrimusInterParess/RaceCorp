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
    using Microsoft.AspNetCore.Http;
    using RaceCorp.Services.Data.Contracts;

    public class GoogleDriveService : IGoogleDriveService
    {
        private const string ServiceAccountEmail = "testproject@testproject-366105.iam.gserviceaccount.com";
        private const string DirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa";

        public async Task<string> UloadGpxFileToDrive(IFormFile gpxFile, string serviceAccountKeyPath, string uploadFileName)
        {
            var credentials = GoogleCredential.FromFile(serviceAccountKeyPath).CreateScoped(DriveService.ScopeConstants.Drive);

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

            await using (var fsSource = new FileStream(uploadFileName, FileMode.Open, FileAccess.Read))
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

            return uploadFileId;
        }
    }
}
