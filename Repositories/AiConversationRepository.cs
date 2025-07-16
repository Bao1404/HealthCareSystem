using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AiConversationRepository : IAiConversationRepository
    {
        public Task CreateConversation(Aiconversation conversation) => AiConversationDAO.CreateConversation(conversation);
        public Task<Aiconversation> GetConversationByUserId(int userId) => AiConversationDAO.GetConversationByUserId(userId);
    }
}
