using Microsoft.EntityFrameworkCore;
using RaceCorp.Data.Common.Repositories;
using RaceCorp.Data.Models;
using RaceCorp.Services.Data.Contracts;
using RaceCorp.Services.Mapping;
using RaceCorp.Web.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceCorp.Services.Data
{
    public class MessageService : IMessageService
    {
        private readonly IDeletableEntityRepository<Message> messageRepo;

        public MessageService(IDeletableEntityRepository<Message> messageRepo)
        {
            this.messageRepo = messageRepo;
        }

        public List<T> GetMessages<T>(string userId, string interlocutorId)
        {
            return this.messageRepo
                .AllAsNoTracking()
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .Where(m =>
                (m.RevceiverId == userId && m.SenderId == interlocutorId) ||
                (m.RevceiverId == interlocutorId && m.SenderId == userId))
                .To<T>()
                .ToList();
        }
    }
}
