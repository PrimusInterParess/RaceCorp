namespace RaceCorp.Common
{
    public static class GlobalConstants
    {
        // common
        public const string SystemName = "RaceCorp";
        public const string Image = "image";
        public const string Gpx = "gpx";
        public const string ConfirmPasswordDisplay = "Confirm password";
        public const string ProfilePictire = "ProfilePicture";
        public const string ServiceAccountName = "Service Account";

        // roles
        public const string AdministratorRoleName = "Administrator";
        public const string UserRoleName = "User";

        // event types
        public const string EventTypeRace = "Race";
        public const string EventTypeRide = "Ride";

        // request types
        public const string RequestTypeTeamJoin = "TeamJoin";
        public const string RequestTypeTeamLeave = "TeamLeave";
        public const string RequestTypeDisconnectUser = "DisconnectUser";
        public const string RequestTypeConnectUser = "ConnectUser";

        // post fix
        public const string ProfilePicterPostFix = "ProfilePicture";

        // date formats
        public const string DateStringFormat = "d MMMM yyyy HH:mm";
        public const string DateMessageFormat = "d/M/yyyy HH:mm";
        public const string DateStringLongFormat = "D";

        // string fomats
        public const string HubGroupNameFormat = "{0}{1}";
        public const string MapUrlTraceGpx = "https://gpx.studio/?state=%7B%22ids%22:%5B%22{0}%22%5D%7D&embed&distance";
        public const string EmailJoinTeamSubject = "Request to join {0}";
        public const string EmailJoinTeamText = "{0} wants to join {1}. The Request is created on {2}.\n Have a great day!\n RaceCorp";

        // display names
        public const string FirstNameDisplay = "First name";
        public const string LastNameDisplay = "Last name";
        public const string DateOfBirhDisplay = "Date of Birth";

        // cred rootpaths
        public const string ServiceAccountPath = "/Credentials/testproject-366105-9ceb2767de2a.json";
        public const string GoogleCredentialsFilePath = "/Credentials/testproject-366105-9ceb2767de2a.json";
        public const string CredentialsPath = "/Credentials/testproject-366105-9ceb2767de2a.json";

        // images rootpaths
        public const string TownCarocelPicture = "/Images/System/1488a87b-4c8f-4603-9326-f6d7ec496424.jpg";
        public const string MountainCarocelPicture = "/Images/System/8ef223eb-d02f-47e9-b07c-9cabb74a87c9.jpg";
        public const string RacesCarocelPicture = "/Images/System/117656b5-e99d-4dc6-aefa-d544a01a4821.jpg";
        public const string RidesCarocelPicture = "/Images/System/3f8236c9-8883-4403-8157-b1ed262ce1ed.jpg";
        public const string DaniProfilePicturePath = "/Images/defaulProfile/dani.JPEG";
        public const string KariProfilePicturePath = "/Images/defaulProfile/kari.JPEG";
        public const string MaleProfilePicturePath = "/Images/defaulProfile/Male.png";
        public const string FemaleProfilePicturePath = "/Images/defaulProfile/Female.png";
        public const string SecretProfilePicturePath = "/Images/defaulProfile/Secret.png";
        public const string NebesnaProfilePicturePath = "/Images/defaulProfile/nebesna.jpg";
        public const string EstelleProfilePicturePath = "/Images/defaulProfile/estelle.jpg";
        public const string KrumProfilePicturePath = "/Images/defaulProfile/krum.jpg";
        public const string RideDefaultImage = "/Images/default/rideDefaultImage.jpg";
        public const string LoginDefaultImage = "/Images/default/login.JPG";
        public const string RegisterDefaultImage = "/Images/default/register.JPEG";

        // messages
        public const string SuccessfulRequestJoin = "You have successfully requested to join the team!";
        public const string SuccessfulRequestConnect = "You have successfully requested to connect!";
        public const string SuccessfulTeamLeave = "You successfully left the team! Good luck!";
        public const string SuccessfulDisconnect = "You successfully disconnected! Good luck!";
        public const string AlreadyHaveCreatedTeam = "You already have created team!";
        public const string UregisteredMessage = "Your are unregistered!";
        public const string RegisteredMessage = "Your are now registered!";
        public const string RemovedTeamMember = "Team member Removed!";
        public const string SecretText = "Keep 'Secret' save, and remember it. With it you can recover your password in case you forget it.";
        public const string AdminMessageSend = "Your message have been send! Thank you!";
        public const string PrivacyPage = "Privacy Policy\r\nThis policy applies to the information that can be collected at the RaceCorp website.\r\n\r\nBy registering at RaceCorp or filling out our contact form, you agree to accept the practices described in this Personal Data Protection Policy and provide us with actual and correct information insofar as it concerns you.\r\n\r\nRaceCorp knows that you are concerned about how information about you is used and shared, and we appreciate your confidence that we will do this carefully and wisely. RaceCorp is committed to complying and respecting your wishes about the information we collect for you on our website.\r\n\r\nPersonal information\r\nThe information we learn from you helps us personalize and constantly improve the way you use RaceCorp.\r\n\r\nThe history of your transactions and the information you provide to us online at RaceCorp, by phone or e -mail is reliably stored in our client database and can only be provided to related companies or some third parties that we have checked carefully With regard to their reliability and integrity.\r\n\r\nRaceCorp, as well as other third parties serving your order, may provide you with information related to your registration and payment on your order, including advertising, information and other messages, at the email address you specify and/or telephone number S\r\n\r\nRaceCorp has the power to process your personal data (including, but not limited to name, surname, email address) and send it to the third parties mentioned above to fulfill your obligations on your order.\r\n\r\nSome of the information we receive from you is voluntarily submitted, although many of it is needed to execute your requests properly. Moting such an infomation can lead to your order refusal. In addition to fulfilling your orders, we use the information you give us, for such purposes as answering your inquiries, scoring your future use of the system and communicating with you.\r\n\r\nAny personal information you reveal to us is protected. RaceCorp maintains standards and procedures for protection against unauthorized access to client information to prevent unauthorized removal or modification of data. RaceCorp welcomes the accepted security standards of all transactions that may include fees through a service available from this Site, including the use of encryption and SSL services. It is important for you to protect yourself from unotoid access to your password and your computer. Always check that you have \"written off\" after you have completed the use of a shared computer.\r\n\r\nAt any time, you can log in to the site and repair all the information that applies to you and which is stored by RaceCorp. You can at any time apply to RaceCorp to disclose what personal information we have for you and to request to change this information.\r\n\r\nE -mail policy\r\nIn addition to exceptions stated in this policy, RaceCorp will never sell, share, or in any other way to distribute your e-mail address you have sent us, or which is otherwise obtained through your use on the site. Each e-mail address sent directly to RaceCorp will only remain possession of RaceCorp and related companies.\r\n\r\nIf you do not want to receive promotion emails from RaceCorp in the future, please tell us through the contact form.\r\n\r\nIf you provide us with your mailing address, you can receive periodic emails from us with information about new products and services, or for expected events. If you do not want to receive such emails, please contact us through our contact form.\r\n\r\nCookies\r\nLike most websites that make retail sales, we use cookies that represent pieces of data sent to your browser that allow our website to identify you when you access it. Although there is some concern for possible complications regarding personal information through cookies, it is important for you to understand that cookies cannot retrieve any information about you that you have not already disclosed voluntarily. If you wish, the \"Help\" part of the toolbar of most browsers will give you information on how to prevent new cookies from your browser, how to inform you when you receive new cookies, or how to completely deactivate \" Cookies. \"\r\n\r\nIf you have any questions about any of the elements of our Web Policy for Personal Data Protection, please email us through the contact form.";

        // ports
        public const string PortHome = "44319";
        public const string PortWork = "5001";

        // carosel image names
        public const string TownImageName = "Town";
        public const string MountainImageName = "Mountain";
        public const string UpcommingRaceImageName = "Upcoming Races";
        public const string UpcommingRidesImageName = "Upcoming Rides";

        // admin
        public const string AdminEmail = "diesonnekind@gmail.com";
        public const string AdminName = "Dani from RaceCorp";
    }
}
