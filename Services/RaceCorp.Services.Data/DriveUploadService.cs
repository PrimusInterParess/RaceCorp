namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using RaceCorp.Services.Data.Contracts;

    public class DriveUploadService : IDriveUploadService
    {
        public string DriveUploadWithConversion(string filePath)
        {
            return null;
        }
    }
}
