namespace RaceCorp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Google.Apis.Drive.v3.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using Moq;
    using RaceCorp.Common;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Data.Tests.Mocks;
    using RaceCorp.Web.ViewModels.User;
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public async Task EditAsyncShouldChangeFirstName()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = "1" },
                },
            },
                new ApplicationUser
            {
                Id = "2",
                CreatedOn = DateTime.Now,
                Email = "kborisova@gmail.com",
                UserName = "kborisova@gmail.com",
                NormalizedEmail = "=kborisova@gmail.com".ToUpper(),
                NormalizedUserName = "kborisova@gmail.com".ToUpper(),
                FirstName = "Karolina",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.KariProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = "1" }, },
            },
            };

            var townsList = new List<Town>()
            {
                new Town
                {
                    Name = "Sofia",
                    Id = 1,
                },
                new Town
                {
                    Name = "Kresna",
                    Id = 2,
                },
            };

            IList<Claim> claimList = new List<Claim>();

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());

            var townRepo = MockRepo.MockDeletableRepository<Town>();
            townRepo.Setup(ur => ur.All()).Returns(townsList.AsQueryable());

            var usermanager = new Mock<MockUserManager>();

            usermanager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(claimList);
            usermanager.Setup(um => um.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            var singInManager = new Mock<MockSignInManager>();

            var userService = new UserService(
                singInManager.Object,
                usermanager.Object,
                userRepo.Object,
                townRepo.Object,
                null,
                null,
                null);

            var inputModel = new UserEditViewModel
            {
                FirstName = "Dani",
                LastName = "Borisov",
                Country = "France",
                Gender = Gender.Male,
                Town = "Kresna",
                Id = "1",
            };

            await userService.EditAsync(inputModel, string.Empty);

            var actual = userService.GetById<ApplicationUser>("1");

            Assert.Equal(inputModel.FirstName, actual.FirstName);
        }

        [Fact]
        public async Task EditAsyncShouldChangeLastName()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = "1" },
                },
            },
                new ApplicationUser
            {
                Id = "2",
                CreatedOn = DateTime.Now,
                Email = "kborisova@gmail.com",
                UserName = "kborisova@gmail.com",
                NormalizedEmail = "=kborisova@gmail.com".ToUpper(),
                NormalizedUserName = "kborisova@gmail.com".ToUpper(),
                FirstName = "Karolina",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.KariProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = "1" }, },
            },
            };

            var townsList = new List<Town>()
            {
                new Town
                {
                    Name = "Sofia",
                    Id = 1,
                },
                new Town
                {
                    Name = "Kresna",
                    Id = 2,
                },
            };

            IList<Claim> claimList = new List<Claim>();

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());

            var townRepo = MockRepo.MockDeletableRepository<Town>();
            townRepo.Setup(ur => ur.All()).Returns(townsList.AsQueryable());

            var usermanager = new Mock<MockUserManager>();

            usermanager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(claimList);
            usermanager.Setup(um => um.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            var singInManager = new Mock<MockSignInManager>();

            var userService = new UserService(
                singInManager.Object,
                usermanager.Object,
                userRepo.Object,
                townRepo.Object,
                null,
                null,
                null);

            var inputModel = new UserEditViewModel
            {
                FirstName = "Dani",
                LastName = "Morisov",
                Country = "France",
                Gender = Gender.Male,
                Town = "Kresna",
                Id = "1",
            };

            await userService.EditAsync(inputModel, string.Empty);

            var actual = userService.GetById<ApplicationUser>("1");

            Assert.Equal(inputModel.LastName, actual.LastName);
        }

        [Fact]
        public async Task EditAsyncShouldChangeCountry()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = "1" },
                },
            },
                new ApplicationUser
            {
                Id = "2",
                CreatedOn = DateTime.Now,
                Email = "kborisova@gmail.com",
                UserName = "kborisova@gmail.com",
                NormalizedEmail = "=kborisova@gmail.com".ToUpper(),
                NormalizedUserName = "kborisova@gmail.com".ToUpper(),
                FirstName = "Karolina",
                LastName = "Borisova",
                TownId = 1,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.KariProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>() { new IdentityUserRole<string>() { RoleId = "1" }, },
            },
            };

            var townsList = new List<Town>()
            {
                new Town
                {
                    Name = "Sofia",
                    Id = 1,
                },
                new Town
                {
                    Name = "Kresna",
                    Id = 2,
                },
            };

            IList<Claim> claimList = new List<Claim>();

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());

            var townRepo = MockRepo.MockDeletableRepository<Town>();
            townRepo.Setup(ur => ur.All()).Returns(townsList.AsQueryable());

            var usermanager = new Mock<MockUserManager>();

            usermanager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(claimList);
            usermanager.Setup(um => um.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            var singInManager = new Mock<MockSignInManager>();

            var userService = new UserService(
                singInManager.Object,
                usermanager.Object,
                userRepo.Object,
                townRepo.Object,
                null,
                null,
                null);

            var inputModel = new UserEditViewModel
            {
                FirstName = "Dani",
                LastName = "Morisov",
                Country = "France",
                Gender = Gender.Male,
                Town = "Kresna",
                Id = "1",
            };

            await userService.EditAsync(inputModel, string.Empty);

            var actual = userService.GetById<ApplicationUser>("1");

            Assert.Equal(inputModel.Country, actual.Country);
        }

        [Fact]
        public async Task EditAsyncShouldChangeGender()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Gender = Gender.Secret,
                Country = "Bulgaria",
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = "1" },
                },
            },
            };

            var townsList = new List<Town>()
            {
                new Town
                {
                    Name = "Sofia",
                    Id = 1,
                },
                new Town
                {
                    Name = "Kresna",
                    Id = 2,
                },
            };

            IList<Claim> claimList = new List<Claim>();

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());

            var townRepo = MockRepo.MockDeletableRepository<Town>();
            townRepo.Setup(ur => ur.All()).Returns(townsList.AsQueryable());

            var usermanager = new Mock<MockUserManager>();

            usermanager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(claimList);
            usermanager.Setup(um => um.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            var singInManager = new Mock<MockSignInManager>();

            var userService = new UserService(
                singInManager.Object,
                usermanager.Object,
                userRepo.Object,
                townRepo.Object,
                null,
                null,
                null);

            var inputModel = new UserEditViewModel
            {
                FirstName = "Dani",
                LastName = "Morisov",
                Country = "France",
                Gender = Gender.Male,
                Town = "Kresna",
                Id = "1",
            };

            await userService.EditAsync(inputModel, string.Empty);

            var actual = userService.GetById<ApplicationUser>("1");

            Assert.Equal(inputModel.Gender, actual.Gender);
        }

        [Fact]
        public async Task EditAsyncShouldChangeProfilePicture()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                CreatedOn = DateTime.Now,
                Email = "diesonnekind@gmail.com",
                UserName = "diesonnekind@gmail.com",
                NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
                NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
                FirstName = "Yordan",
                LastName = "Borisov",
                TownId = 1,
                Country = "Bulgaria",
                DateOfBirth = new DateTime(2020, 04, 22),
                Gender=Gender.Secret,
                ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
                PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                Roles = new List<IdentityUserRole<string>>()
                {
                    new IdentityUserRole<string>() { RoleId = "1" },
                },
            },
            };

            var townsList = new List<Town>()
            {
                new Town
                {
                    Name = "Sofia",
                    Id = 1,
                },
                new Town
                {
                    Name = "Kresna",
                    Id = 2,
                },
            };

            var townRepo = MockRepo.MockDeletableRepository<Town>();
            townRepo.Setup(ur => ur.All()).Returns(townsList.AsQueryable());

            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(x => x.FileName).Returns("test.jpg");

            var inputModel = new UserEditViewModel
            {
                FirstName = "Yordan",
                LastName = "Borisov",
                Country = "Bulgaria",
                Town = "Sofia",
                Id = "1",
                Gender = Gender.Secret,
                UserProfilePicture = mockFormFile.Object,
                DateOfBirth = new DateTime(2020, 04, 22),
            };

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());

            var imageRepo = MockRepo.MockDeletableRepository<Image>();
            imageRepo.Setup(ir => ir.AddAsync(It.IsAny<Image>())).Returns(Task.CompletedTask);

            var fileService = new Mock<FileService>(null, imageRepo.Object);
            var singInManager = new Mock<MockSignInManager>();
            var usermanager = new Mock<MockUserManager>();

            var userService = new UserService(
               singInManager.Object,
               usermanager.Object,
               userRepo.Object,
               townRepo.Object,
               fileService.Object,
               null,
               null);

            await userService.EditAsync(inputModel, string.Empty);

            var actual = userService.GetById<ApplicationUser>("1");

            Assert.NotEqual(GlobalConstants.DaniProfilePicturePath, actual.ProfilePicturePath);
        }

        [Fact]
        public void GetAllAsyncHomePageShouldReturnAllUsersWithoutCurrentUser()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
            },
                new ApplicationUser
            {
                Id = "2",
            },
                new ApplicationUser
            {
                Id = "3",
            },
                new ApplicationUser
            {
                Id = "4",
            },
                new ApplicationUser
            {
                Id = "5",
            },
            };

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.AllAsNoTracking()).Returns(usersList.AsQueryable());

            var connectionRepo = MockRepo.MockDeletableRepository<Connection>();
            var requestRepo = MockRepo.MockDeletableRepository<Request>();

            var userService = new UserService(null, null, userRepo.Object, null, null, requestRepo.Object, connectionRepo.Object);

            var usersHome = userService.GetAllAsyncHomePage("1");

            Assert.DoesNotContain(usersHome, u => u.Id == "1");
        }

        [Fact]
        public void GetAllAsyncTShouldReturnAllUsers()
        {
            MockAutoMapper.InitializeAutoMapper();

            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
            },
                new ApplicationUser
            {
                Id = "2",
            },
                new ApplicationUser
            {
                Id = "3",
            },
                new ApplicationUser
            {
                Id = "4",
            },
                new ApplicationUser
            {
                Id = "5",
            },
            };

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.AllAsNoTracking()).Returns(usersList.AsQueryable());

            var userService = new UserService(null, null, userRepo.Object, null, null, null, null);

            var allUsers = userService.GetAllAsync<UserAllViewModel>();

            Assert.Equal(allUsers.Count, usersList.Count);
        }

        [Fact]
        public void GetEmailShouldReturnUsersEmailIfExists()
        {
            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                Email = "first@mail",
            },
                new ApplicationUser
            {
                Id = "2",
                Email = "second@mail",

            },
                new ApplicationUser
            {
                Id = "3",
                Email = "third@mail",

            },
                new ApplicationUser
            {
                Id = "4",
                Email = "forth@mail",

            },
                new ApplicationUser
            {
                Id = "5",
                Email = "fifth@mail",

            },
            };

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.AllAsNoTracking()).Returns(usersList.AsQueryable());

            var userService = new UserService(null, null, userRepo.Object, null, null, null, null);

            var email = userService.GetUserEmail("1");

            Assert.Equal("first@mail", email);
        }

        [Fact]
        public void GetEmailShouldReturnNullIfDoesntExists()
        {
            var usersList = new List<ApplicationUser>()
            {
                new ApplicationUser
            {
                Id = "1",
                Email = "first@mail",
            },
                new ApplicationUser
            {
                Id = "2",
                Email = "second@mail",

            },
                new ApplicationUser
            {
                Id = "3",
                Email = "third@mail",

            },
                new ApplicationUser
            {
                Id = "4",
                Email = "forth@mail",

            },
                new ApplicationUser
            {
                Id = "5",
                Email = "fifth@mail",

            },
            };

            var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            userRepo.Setup(ur => ur.AllAsNoTracking()).Returns(usersList.AsQueryable());

            var userService = new UserService(null, null, userRepo.Object, null, null, null, null);

            var email = userService.GetUserEmail("7");

            Assert.Null(email);
        }

        //[Fact]
        //public void GetProfileModelByIdShouldReturnModelWhenUserExists()
        //{
        //    MockAutoMapper.InitializeAutoMapper();

        //    var usersList = new List<ApplicationUser>()
        //    {
        //        new ApplicationUser
        //    {
        //        Id = "1",
        //        CreatedOn = DateTime.Now,
        //        Email = "diesonnekind@gmail.com",
        //        UserName = "diesonnekind@gmail.com",
        //        NormalizedEmail = "diesonnekind@gmail.com".ToUpper(),
        //        NormalizedUserName = "diesonnekind@gmail.com".ToUpper(),
        //        FirstName = "Yordan",
        //        LastName = "Borisov",
        //        TownId = 1,
        //        Country = "Bulgaria",
        //        DateOfBirth = new DateTime(2020, 04, 22),
        //        Gender=Gender.Secret,
        //        ProfilePicturePath = GlobalConstants.DaniProfilePicturePath,
        //        PasswordHash = "AQAAAAEAACcQAAAAEFtyhksza71QGv3QHjiJZpH1N7QnVRPMCdPLZzsg9TpkL4ivLAXUiIZiGixtuUQsog==",
                
        //    },
        //        new ApplicationUser
        //    {
        //        Id = "2",
        //        Email = "second@mail",

        //    },
        //        new ApplicationUser
        //    {
        //        Id = "3",
        //        Email = "third@mail",

        //    },
        //        new ApplicationUser
        //    {
        //        Id = "4",
        //        Email = "forth@mail",

        //    },
        //        new ApplicationUser
        //    {
        //        Id = "5",
        //        Email = "fifth@mail",

        //    },
        //    };

        //    var userRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
        //    userRepo.Setup(ur => ur.All()).Returns(usersList.AsQueryable());
        //    userRepo.Setup(ur => ur.AllAsNoTracking()).Returns(usersList.AsQueryable());

        //    var requestRepo = MockRepo.MockDeletableRepository<Request>();
        //    var connectionrepo = MockRepo.MockDeletableRepository<Connection>();

        //    var userService = new UserService(null, null, userRepo.Object, null, null, requestRepo.Object, connectionrepo.Object);

        //    var userProfileModel = userService.GetProfileModelById("1", "1");

        //    Assert.Null(userProfileModel);
        //}
    }
}
