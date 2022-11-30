namespace RaceCorp.Services.Data
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.Common;
    using RaceCorp.Web.ViewModels.EventRegister;

    using Trace = RaceCorp.Data.Models.Trace;

    public class EventService : IEventService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IRepository<ApplicationUserRide> userRideRepo;
        private readonly IRepository<ApplicationUserRace> userRaceRepo;
        private readonly IRepository<ApplicationUserTrace> userTraceRepo;
        private readonly IDeletableEntityRepository<Team> teamRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;
        private readonly IDeletableEntityRepository<Connection> connectionRepo;
        private readonly IDeletableEntityRepository<Conversation> conversationRepo;

        public EventService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IRepository<ApplicationUserRide> userRideRepo,
            IRepository<ApplicationUserRace> userRaceRepo,
            IRepository<ApplicationUserTrace> userTraceRepo,
            IDeletableEntityRepository<Team> teamRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Request> requestRepo,
            IDeletableEntityRepository<Connection> connectionRepo,
            IDeletableEntityRepository<Conversation> conversationRepo)
        {
            this.userManager = userManager;
            this.rideRepo = rideRepo;
            this.raceRepo = raceRepo;
            this.userRideRepo = userRideRepo;
            this.userRaceRepo = userRaceRepo;
            this.userTraceRepo = userTraceRepo;
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.requestRepo = requestRepo;
            this.connectionRepo = connectionRepo;
            this.conversationRepo = conversationRepo;
        }

        public async Task RegisterUserEvent(EventRegisterModel eventModel)
        {
            if (eventModel.EventType == "Ride")
            {
                try
                {
                    await this.RegisterUserRide(eventModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
            else
            {
                try
                {
                    await this.RegisterUserRace(eventModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
        }

        public async Task Unregister(EventRegisterModel eventModel)
        {
            if (eventModel.EventType == "Ride")
            {
                try
                {
                    await this.UnregisterUserRide(eventModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
            else
            {
                try
                {
                    await this.UnregisterUserRace(eventModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
        }

        public async Task ProccesRequest(RequestInputModel inputModel)
        {
            if (inputModel.Type == GlobalConstants.RequestTypeTeamJoin)
            {
                try
                {
                    await this.RequestJoinTeamAsync(inputModel.TargetId, inputModel.RequesterId);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                          new ArgumentException(e.Message) :
                          new InvalidOperationException(e.Message);
                }
            }
            else if (inputModel.Type == GlobalConstants.RequestTypeConnectUser)
            {
                try
                {
                    await this.RequestConnectUserAsync(inputModel.RequesterId, inputModel.TargetId);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
            else if (inputModel.Type == GlobalConstants.RequestTypeTeamLeave)
            {
                try
                {
                    await this.LeaveTeamAsync(inputModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
            else if (inputModel.Type == GlobalConstants.RequestTypeDisconnectUser)
            {
                try
                {
                    await this.DisconnectUserAsync(inputModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
        }

        public async Task ProccesApproval(ApproveRequestModel inputModel)
        {
            if (inputModel.RequestType == GlobalConstants.RequestTypeTeamJoin)
            {
                try
                {
                    await this.ApproveJoinRequestAsync(inputModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                        new ArgumentException(e.Message) :
                        new InvalidOperationException(e.Message);
                }
            }
            else
            {
                try
                {
                    await this.ApproveConnectRequestAsync(inputModel);
                }
                catch (Exception e)
                {
                    throw e.GetType() == typeof(ArgumentException) ?
                         new ArgumentException(e.Message) :
                         new InvalidOperationException(e.Message);
                }
            }
        }

        private async Task DisconnectUserAsync(RequestInputModel inputModel)
        {
            var requester = this.userRepo
               .All()
               .Include(u => u.Connections)
               .Include(u => u.Requests)
               .Include(c => c.Conversations)
               .FirstOrDefault(u => u.Id == inputModel.RequesterId);
            if (requester == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var targetUser = this.userRepo
                .All()
                .Include(u => u.Connections)
                .Include(u => u.Requests)
                .Include(c => c.Conversations)
                .FirstOrDefault(u => u.Id == inputModel.TargetId);
            if (targetUser == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var requesterConnectRequest = requester.Requests
                .FirstOrDefault(
                r => r.RequesterId == targetUser.Id &&
                r.Type == GlobalConstants.RequestTypeConnectUser);
            if (requesterConnectRequest != null)
            {
                requester.Requests.Remove(requesterConnectRequest);
            }

            var targetUserConnectRequest = targetUser.Requests
               .FirstOrDefault(
                r => r.RequesterId == requester.Id &&
               r.Type == GlobalConstants.RequestTypeConnectUser);
            if (targetUserConnectRequest != null)
            {
                targetUser.Requests.Remove(targetUserConnectRequest);
            }

            var requesterConnection = requester.Connections
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);
            if (requesterConnection != null)
            {
                requester.Connections.Remove(requesterConnection);
                this.connectionRepo.Delete(requesterConnection);
            }

            var targetUserConnection = targetUser.Connections
              .FirstOrDefault(
              c => c.Id == requester.Id + targetUser.Id ||
              c.Id == targetUser.Id + requester.Id);
            if (targetUserConnection != null)
            {
                targetUser.Connections.Remove(targetUserConnection);
                this.connectionRepo.Delete(targetUserConnection);
            }

            var requesterConversation = requester.Conversations
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);
            if (requesterConversation != null)
            {
                requester.Conversations.Remove(requesterConversation);
                this.conversationRepo.Delete(requesterConversation);
            }

            var targetUserConversation = targetUser.Conversations
                .FirstOrDefault(
                c => c.Id == requester.Id + targetUser.Id ||
                c.Id == targetUser.Id + requester.Id);

            if (targetUserConversation != null)
            {
                targetUser.Conversations.Remove(targetUserConversation);
                this.conversationRepo.Delete(targetUserConversation);
            }

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }

        private async Task LeaveTeamAsync(RequestInputModel inputModel)
        {
            var teamDb = this.teamRepo
                .All()
                .Include(t => t.TeamMembers)
                .Include(t => t.ApplicationUser)
                .ThenInclude(u => u.Requests)
                .FirstOrDefault(t => t.Id == inputModel.TargetId);

            if (teamDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidTeam);
            }

            var requester = teamDb.TeamMembers.FirstOrDefault(m => m.Id == inputModel.RequesterId);
            if (requester == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.UnauthorizedRequest);
            }

            if (requester.Id == teamDb.ApplicationUserId)
            {
                var newOnwer = teamDb.TeamMembers.FirstOrDefault(m => m.Id != requester.Id);
                if (newOnwer != null)
                {
                    newOnwer.Team = teamDb;
                    teamDb.ApplicationUser = newOnwer;
                    teamDb.TeamMembers.Remove(requester);
                    await this.teamRepo.SaveChangesAsync();
                    return;
                }
            }

            var teamOnwer = teamDb.ApplicationUser;
            if (teamOnwer != null)
            {
                var requestToRemove = teamOnwer
                .Requests
                .FirstOrDefault(r => r.IsApproved && r.RequesterId == requester.Id && r.Type == GlobalConstants.RequestTypeTeamJoin);
                if (requestToRemove != null)
                {
                    teamOnwer.Requests.Remove(requestToRemove);
                    teamDb.TeamMembers.Remove(requester);
                }
            }

            if (teamOnwer == null)
            {
                teamDb.TeamMembers.Remove(requester);
            }

            if (teamDb.TeamMembers.Count == 0)
            {
                this.teamRepo.HardDelete(teamDb);
                await this.teamRepo.SaveChangesAsync();
                throw new ArgumentException(GlobalErrorMessages.TeamNoLongerExists);
            }

            try
            {
                await this.teamRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }

        private async Task RegisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo
                .All()
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (ride == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = await this.userManager
                .FindByIdAsync(eventModel.UserId);

            if (user == null)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            if (ride.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId))
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            var userRide = new ApplicationUserRide
            {
                CreatedOn = DateTime.UtcNow,
                Ride = ride,
                ApplicationUser = user,
            };

            await this.userRideRepo.AddAsync(userRide);
            await this.rideRepo.SaveChangesAsync();
        }

        private async Task RegisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo
                .All()
                .Include(r => r.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (race == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = await this.userManager
                .FindByIdAsync(eventModel.UserId);

            if (user == null)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }

            var trace = race.Traces
                .FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));

            if (trace == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            Trace traceUserRegisterIn = null;

            var isAlredyRegistered = race
                .RegisteredUsers
                .Any(u => u.ApplicationUserId == eventModel.UserId);

            if (isAlredyRegistered)
            {
                traceUserRegisterIn = race.Traces
                 .FirstOrDefault(t => t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId));

                var currdentTraceSpan = trace.StartTime;

                var traceAlreadyRegesteredInSpan = traceUserRegisterIn.StartTime + traceUserRegisterIn.ControlTime;

                if (currdentTraceSpan < traceAlreadyRegesteredInSpan)
                {
                    throw new InvalidOperationException(
                        string.Format(GlobalErrorMessages.AlreadyRegisteredForAnotherTrace, traceUserRegisterIn.Name, traceUserRegisterIn.StartTime.ToString(GlobalConstants.DateStringFormat)));
                }
            }

            var userTrace = new ApplicationUserTrace
            {
                CreatedOn = DateTime.UtcNow,
                Race = race,
                Trace = trace,
                ApplicationUser = user,
            };

            user.Traces.Add(userTrace);

            if (isAlredyRegistered == false)
            {
                var userRace = new ApplicationUserRace
                {
                    CreatedOn = DateTime.UtcNow,
                    Race = race,
                    Trace = trace,
                    ApplicationUser = user,
                };

                await this.userRaceRepo.AddAsync(userRace);
            }

            try
            {
                await this.userTraceRepo.AddAsync(userTrace);
                await this.raceRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception(GlobalErrorMessages.InvalidRequest);
            }
        }

        private async Task UnregisterUserRide(EventRegisterModel eventModel)
        {
            var ride = this.rideRepo
                .All()
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (ride == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var user = ride
                .RegisteredUsers
                .FirstOrDefault(u => u.ApplicationUserId == eventModel.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }

            try
            {
                this.userRideRepo.Delete(user);
                await this.userRideRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        private async Task UnregisterUserRace(EventRegisterModel eventModel)
        {
            var race = this.raceRepo
                .All()
                .Include(t => t.Traces)
                .ThenInclude(t => t.RegisteredUsers)
                .Include(r => r.RegisteredUsers)
                .FirstOrDefault(r => r.Id == eventModel.Id);

            if (race == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var trace = race.Traces
                .FirstOrDefault(t => t.Id == int.Parse(eventModel.TraceId));

            if (trace == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var participatesInAnotherTrace = race.Traces
                .FirstOrDefault(t =>
                t.RegisteredUsers.Any(u => u.ApplicationUserId == eventModel.UserId && u.TraceId != trace.Id));

            if (participatesInAnotherTrace == null)
            {
                var registeredUserRace = race.RegisteredUsers.FirstOrDefault(u =>
                u.ApplicationUserId == eventModel.UserId);

                this.userRaceRepo.Delete(registeredUserRace);
            }

            var registeredUserTrace = trace
                .RegisteredUsers
                .FirstOrDefault(u =>
            u.ApplicationUserId == eventModel.UserId);

            try
            {
                this.userTraceRepo.Delete(registeredUserTrace);
                await this.userTraceRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        private async Task RequestJoinTeamAsync(string teamId, string requesterId)
        {
            var teamDb = this.teamRepo
                 .All()
                 .Include(t => t.ApplicationUser)
                 .ThenInclude(u => u.Requests)
                 .FirstOrDefault(t => t.Id == teamId);
            if (teamDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidTeam);
            }
            var teamOwner = teamDb.ApplicationUser;
            if (teamOwner.Id == requesterId)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
            if (teamOwner.Requests.Any(r => r.RequesterId == requesterId))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyRequested);
            }
            var requester = this.userRepo
                .All()
                .Include(u => u.Team)
                .Include(u => u.MemberInTeam)
                .FirstOrDefault(u => u.Id == requesterId);
            if (requester == null)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
            var request = new Request()
            {
                Type = GlobalConstants.RequestTypeTeamJoin,
                TargetUser = teamOwner,
                RequesterId = requesterId,
                Description = $"{requester.FirstName} {requester.LastName} want to join {teamDb.Name}",
                CreatedOn = DateTime.UtcNow,
            };
            teamOwner.Requests.Add(request);
            try
            {
                await this.requestRepo.AddAsync(request);
                await this.requestRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }

        private async Task RequestConnectUserAsync(string currentUserId, string targetUserId)
        {
            var userDb = this.userRepo
               .All()
               .Include(u => u.Requests)
               .Include(u => u.Connections)
               .FirstOrDefault(u => u.Id == currentUserId);
            var targetUserDb = this.userRepo
                .All()
                .Include(u => u.Requests)
                .Include(u => u.Connections)
                .FirstOrDefault(u => u.Id == targetUserId);
            if (userDb == null ||
                targetUserDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }
            if (targetUserDb.Connections.Any(c => c.Id == userDb.Id) ||
                targetUserDb.Requests.Any(r => r.Type == GlobalConstants.RequestTypeConnectUser && r.RequesterId == userDb.Id))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyRequestedConnection);
            }
            var request = new Request()
            {
                Type = GlobalConstants.RequestTypeConnectUser,
                TargetUser = targetUserDb,
                Requester = userDb,
                Description = $"{userDb.FirstName} {userDb.LastName} want to connect with you",
                CreatedOn = DateTime.UtcNow,
            };
            targetUserDb.Requests.Add(request);
            try
            {
                await this.requestRepo.AddAsync(request);
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }
        }

        private async Task ApproveJoinRequestAsync(ApproveRequestModel inputModel)
        {
            var requestDb = this.requestRepo
                .All()
                .Include(r => r.Requester).ThenInclude(u => u.MemberInTeam)
                .Include(r => r.TargetUser).ThenInclude(u => u.Team)
                .FirstOrDefault(r => r.Id == inputModel.RequestId);

            if (requestDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var targetUser = requestDb.TargetUser;

            if (targetUser == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var requesterDb = this.userRepo
                .All()
                .FirstOrDefault(u => u.Id == requestDb.RequesterId);

            if (requesterDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            requestDb.IsApproved = true;

            requesterDb.MemberInTeam = targetUser.Team;
            targetUser.Team.TeamMembers.Add(requesterDb);

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        private async Task ApproveConnectRequestAsync(ApproveRequestModel inputModel)
        {
            var requestDb = this.requestRepo
                .All()
                .Include(r => r.Requester)
                .Include(r => r.TargetUser)
                .FirstOrDefault(r => r.Id == inputModel.RequestId);

            if (requestDb == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            var requester = requestDb.Requester;
            var targetUser = requestDb.TargetUser;

            if (requester == null || targetUser == null)
            {
                throw new ArgumentException(GlobalErrorMessages.InvalidRequest);
            }

            if (requester.Connections.Any(c => c.Id == targetUser.Id))
            {
                throw new InvalidOperationException(GlobalErrorMessages.AlreadyConnected);
            }

            var connectionExists = this.connectionRepo
                .AllWithDeleted()
                .Any(c => c.Id == targetUser.Id + requester.Id);

            if (connectionExists == false)
            {
                targetUser.Connections.Add(new Connection
                {
                    Id = targetUser.Id + requester.Id,
                    ApplicationUser = targetUser,
                    CreatedOn = DateTime.UtcNow,
                    Interlocutor = requester,
                });

                requester.Connections.Add(new Connection
                {
                    Id = requester.Id + targetUser.Id,
                    ApplicationUser = requester,
                    CreatedOn = DateTime.UtcNow,
                    Interlocutor = targetUser,
                });
            }
            else
            {
                var connectionASide = this.connectionRepo
                    .AllWithDeleted()
                    .FirstOrDefault(c => c.Id == targetUser.Id + requester.Id);

                var connectionBSide = this.connectionRepo
                    .AllWithDeleted()
                    .FirstOrDefault(c => c.Id == requester.Id + targetUser.Id);

                if (connectionASide.ApplicationUserId == targetUser.Id)
                {
                    targetUser.Connections.Add(connectionASide);
                    requester.Connections.Add(connectionBSide);
                }
                else
                {
                    targetUser.Connections.Add(connectionBSide);
                    requester.Connections.Add(connectionASide);
                }

                connectionASide.IsDeleted = false;
                connectionBSide.IsDeleted = false;
                connectionASide.ModifiedOn = DateTime.UtcNow;
                connectionBSide.ModifiedOn = DateTime.UtcNow;
                targetUser.Connections.Add(connectionASide);
                requester.Connections.Add(connectionBSide);
            }

            var conversationExists = this.conversationRepo
                .AllWithDeleted()
                .Any(c => c.Id == targetUser.Id + requester.Id);

            if (conversationExists)
            {
                var sideAConversation = this.conversationRepo
                    .AllWithDeleted()
                    .FirstOrDefault(c => c.Id == targetUser.Id + requester.Id);
                var sideBConversation = this.conversationRepo
                    .AllWithDeleted()
                    .FirstOrDefault(c => c.Id == requester.Id + targetUser.Id);
                if (sideAConversation.ApplicationUserId == targetUser.Id)
                {
                    targetUser.Conversations.Add(sideAConversation);
                    requester.Conversations.Add(sideBConversation);
                }
                else
                {
                    targetUser.Conversations.Add(sideBConversation);
                    requester.Conversations.Add(sideAConversation);
                }
                sideAConversation.IsDeleted = false;
                sideBConversation.IsDeleted = false;
                sideAConversation.ModifiedOn = DateTime.UtcNow;
                sideBConversation.ModifiedOn = DateTime.UtcNow;
            }

            requestDb.IsApproved = true;

            try
            {
                await this.userRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException(GlobalErrorMessages.InvalidRequest);
            }
        }
    }
}
