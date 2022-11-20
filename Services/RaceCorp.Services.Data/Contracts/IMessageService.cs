namespace RaceCorp.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RaceCorp.Web.ViewModels.Common;

    public interface IMessageService
    {
        List<T> GetMessages<T>(string authorId, string interlocutorId);
    }
}
