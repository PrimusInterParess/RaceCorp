namespace RaceCorp.Services.Constants
{
    public static class Messages
    {
        public const string LogoImageRequired = "Logo image is Requred!";
        public const string GpxFileRequired = "Gpx file is Requred!";

        public const string InvalidImageSize = "Invalid file size. It needs to be max 10mb.!";
        public const string InvalidImageExtension = "Invalid image extension ";
        public const string InvalidImageMessage = "Allowed image extentions are: png, jpg, gif. File size should not exceed 10mb";
        public const string InvalidFileExtension = "Invalid file extension!";
        public const string InvalidTrace = "Invalid trace";
        public const string InvalidStartDateExceptionMessage = $"Invalid date format.Trace date format should as follows- 'Year/Month/Day Hour:Minutes'.";

        public const string IvalidOperationMessage = "Invalid operation";

        public const string TownNameAlreadyExists = "Town with this name already exists";
        public const string TownNameLenghtError = "Town name should be between {2} and {1} characters long";

        public const string InvalidTeamNameLenghMessage = "{0} should be between{2} and {1}";

        public const string DescriptionLenghtErrorMessage = "Description must be between {2} and {1} characteres long";
    }
}
