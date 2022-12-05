namespace RaceCorp.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Messaging;
    using RaceCorp.Web.ViewModels.Common;

    public class ApprovalService : IApprovalService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;
        private readonly IDeletableEntityRepository<Connection> connectionRepo;
        private readonly IDeletableEntityRepository<Conversation> conversationRepo;
        private readonly IEmailSender emailSender;

        public ApprovalService(
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Request> requestRepo,
            IDeletableEntityRepository<Connection> connectionRepo,
            IDeletableEntityRepository<Conversation> conversationRepo,
            IEmailSender emailSender)
        {
            this.userRepo = userRepo;
            this.requestRepo = requestRepo;
            this.connectionRepo = connectionRepo;
            this.conversationRepo = conversationRepo;
            this.emailSender = emailSender;
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
                //await this.emailSender.SendEmailAsync(
                //    GlobalConstants.AdminEmail,
                //    GlobalConstants.AdminName,
                //    targetUser.Email,
                //    string.Format(GlobalConstants.EmailJoinTeamSubject, targetUser.Team.Name),
                //    string.Format(GlobalConstants.EmailJoinTeamText, $"{requesterDb.FirstName} {requesterDb.LastName}", targetUser.Team.Name, requestDb.CreatedOn.ToString(GlobalConstants.DateStringFormat)));

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
