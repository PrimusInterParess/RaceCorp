// See https://aka.ms/new-console-template for more information
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

public class Program
{
    //public static void Main(string[] args)
    //{
    //    Console.WriteLine("Hello, World!");


    //    var service = GetService();

    //    var result = CreateFolder(
    //        "main", "GxpFiles", service);

    //    Console.WriteLine(result);


    //}

    //public static string CreateFolder(string parent, string folderName, DriveService service)
    //{

    //    var driveFolder = new Google.Apis.Drive.v3.Data.File();
    //    driveFolder.Name = folderName;
    //    driveFolder.MimeType = "application/vnd.google-apps.folder";
    //    driveFolder.Parents = new string[] { parent };
    //    var command = service.Files.Create(driveFolder);

    //    try
    //    {
    //        var file = command.Execute();
    //        return file.Id;


    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e.Message);
    //        throw;
    //    }
    //}

    //private static DriveService GetService()
    //{
    //    var tokenResponse = new TokenResponse
    //    {
    //        AccessToken = "ya29.a0Aa4xrXMUBok4yA33PD4aoZjRsCSwd8fAkVFIrf13KmPv0Z_2foiQkXrnYSXSdgrIyDoieIzHRcN97gc1GavhZ5UbXzpuNi4U6CLqnqLJVIp1JrZZtV_rDa64AJxEJk5OfVbwbk5i7Ty9boyhHtEkXIYMp-JxaCgYKATASARASFQEjDvL98JpQOwy_M9-qL4nePI51iw0163",
    //        RefreshToken = "1//044h5OHDws5fbCgYIARAAGAQSNwF-L9IrAAIRoJra_Xqt_aDwDciqCQfkQjECFAWDRbDbYWuwC5-WBa9v8JxTXuvqMw2snwsKVm4",
    //    };


    //    var applicationName = "RaceCorp"; // Use the name of the project in Google Cloud
    //    var username = "diesonnekind@gmail.com"; // Use your email


    //    var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
    //    {
    //        ClientSecrets = new ClientSecrets
    //        {
    //            ClientId = "1060738667904-ekbsr8m9p8r6ei01sj79pcs403pev78j.apps.googleusercontent.com",
    //            ClientSecret = "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"
    //        },
    //        Scopes = new[] { Scope.Drive },
    //        DataStore = new FileDataStore(applicationName)
    //    });


    //    var credential = new UserCredential(apiCodeFlow, username, tokenResponse);


    //    var service = new DriveService(new BaseClientService.Initializer
    //    {
    //        HttpClientInitializer = credential,
    //        ApplicationName = applicationName
    //    });
    //    return service;
    //}

    static string[] Scopes = { DriveService.Scope.Drive };
    static string ApplicationName = "RaceCorp";

    static void Main(string[] args)
    {
        UserCredential credential;

        credential = GetCredentials();

        // Create Drive API service.
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });


        UploadBasicImage("you file full path", service);

        string pageToken = null;

        do
        {
            ListFiles(service, ref pageToken);

        } while (pageToken != null);

        Console.WriteLine("Done");
        Console.Read();


    }

    private static void ListFiles(DriveService service, ref string pageToken)
    {
        // Define parameters of request.
        FilesResource.ListRequest listRequest = service.Files.List();
        listRequest.PageSize = 1;
        //listRequest.Fields = "nextPageToken, files(id, name)";
        listRequest.Fields = "nextPageToken, files(name)";
        listRequest.PageToken = pageToken;
        listRequest.Q = "mimeType='image/jpeg'";

        // List files.
        var request = listRequest.Execute();


        if (request.Files != null && request.Files.Count > 0)
        {


            foreach (var file in request.Files)
            {
                Console.WriteLine("{0}", file.Name);
            }

            pageToken = request.NextPageToken;

            if (request.NextPageToken != null)
            {
                Console.WriteLine("Press any key to conti...");
                Console.ReadLine();



            }


        }
        else
        {
            Console.WriteLine("No files found.");

        }


    }

    private static void UploadBasicImage(string path, DriveService service)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File();
        fileMetadata.Name = Path.GetFileName(path);
        fileMetadata.MimeType = "image/jpeg";
        FilesResource.CreateMediaUpload request;
        using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
        {
            request = service.Files.Create(fileMetadata, stream, "image/jpeg");
            request.Fields = "id";
            request.Upload();
        }

        var file = request.ResponseBody;

        Console.WriteLine("File ID: " + file.Id);

    }

    private static UserCredential GetCredentials()
    {
        UserCredential credential;

        using (var stream = new FileStream("C:\\Users\\yborisov\\Desktop\\RaceCorp\\RaceCrop\\Web\\RaceCorp.Web\\client_secret_1060738667904-ekbsr8m9p8r6ei01sj79pcs403pev78j.apps.googleusercontent.com (1).json", FileMode.Open, FileAccess.Read))
        {
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            // Console.WriteLine("Credential file saved to: " + credPath);
        }

        return credential;
    }
}