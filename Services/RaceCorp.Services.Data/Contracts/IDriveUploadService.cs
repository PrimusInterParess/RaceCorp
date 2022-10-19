namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDriveUploadService
    {
        public string DriveUploadWithConversion(string filePath);
    }
}
