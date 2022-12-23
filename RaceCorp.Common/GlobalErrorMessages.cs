namespace RaceCorp.Common
{
    public static class GlobalErrorMessages
    {
        // formats
        public const string StringLengthError = "{0} should be between {2} and {1} characters!";
        public const string AlreadyRegisteredForAnotherTrace = "Cannot register for this trace.You are already registered for {0} and it starts {1}";
        public const string AlreadyHaveTeam = "You are already a member of {0}";

        // pass
        public const string PasswordConfirmPassowrdDontMatch = "The password and confirmation password do not match.";

        // team
        public const string TeamAlreadyExists = "Invalid team name!";
        public const string AlreadyHaveCreatedTeam = "You have already created team!";
        public const string AlreadyRequested = "You have already requested to join the team";
        public const string TeamNoLongerExists = "Team no longer exists.Have a nice life!";
        public const string InvalidTeam = "Invalid team!";
        public const string TeamDeleted = "Team is deleted!";

        // invalid
        public const string InvalidRequest = "You made invalid request!";
        public const string InvalidInputData = "Invalid Input data";
        public const string InvalidRaceId = "Invalid race";
        public const string InvalidSearchTerms = "Invalid search terms. Max words count is two";
        public const string NotExistingContent = "The content you are trying to reach does not exists";

        // unauthorized
        public const string UnauthorizedRequest = "Unauthorized request";

        // connection
        public const string AlreadyRequestedConnection = "Connection is already requested!";
        public const string AlreadyConnected = "Already connected!";
    }
}
