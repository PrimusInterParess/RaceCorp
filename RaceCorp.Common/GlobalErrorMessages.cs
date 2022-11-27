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

        public const string InvalidRequest = "You made invalid Request! Fudge off";

        public const string AlreadyRequested = "You have already requested to join the team";

        public const string InvalidSearchTerms = "Invalid search terms. Max words count is two";

    }
}
