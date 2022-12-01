using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceCorp.Common
{
    public static class GlobalErrorMessages
    {
        public const string StringLengthError = "{0} should be between {2} and {1} characters!";

        public const string PasswordConfirmPassowrdDontMatch = "The password and confirmation password do not match.";

        public const string InvalidRaceId = "Invalid race";

        public const string InvalidInputData = "Invalid Input data";

        public const string TeamAlreadyExists = "Invalid team name!";

        public const string AlreadyHaveCreatedTeam = "You have alredy created team!";

        public const string InvalidRequest = "You made invalid request!";

        public const string AlreadyRequested = "You have already requested to join the team";

        public const string InvalidSearchTerms = "Invalid search terms. Max words count is two";

        public const string TeamNoLongerExists = "Team no longer exists.Have a nice life!";

        public const string InvalidTeam = "Invalid team!";

        public const string UnauthorizedRequest = "Unauthorized request";

        public const string AlreadyRequestedConnection = "Connection is already requested!";

        public const string AlreadyConnected = "Already connected!";

        public const string AlreadyRegisteredForAnotherTrace = "Cannot register for this trace.You are already registered for {0} and it starts {1}";

        public const string TeamDeleted = "Team is deleted!";
    }
}
