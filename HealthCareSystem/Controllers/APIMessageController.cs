using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;


namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIMessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public APIMessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversationId(int conversationId)
        {
            var messages = await _messageRepository.GetMessagesByConversationId(conversationId);

            var result = messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                Content = m.Content,
                SentAt = m.SentAt,
                Sender = new SenderDTO
                {
                    UserId = m.Sender.UserId,
                    FullName = m.Sender.FullName,
                    Role = m.Sender.Role,
                    AvatarUrl = m.Sender.AvatarUrl
                }
            }).ToList();

            return Ok(result);
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            var message = new Message
            {
                ConversationId = dto.ConversationId,
                SenderId = dto.SenderId,
                Content = dto.Content,
                MessageType = dto.MessageType,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _messageRepository.CreateMessage(message);
            return Ok(message);
        }





    }
}
