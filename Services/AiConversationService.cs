using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AiConversationService
    {
        private readonly IAiConversationRepository _repository;
        public AiConversationService()
        {
            _repository = new AiConversationRepository();
        }
        Task CreateConversation(Aiconversation conversation) => _repository.CreateConversation(conversation);
    }
}
