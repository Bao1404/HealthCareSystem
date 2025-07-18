using BusinessObjects;
using HealthCareSystem.DTO;
using HealthCareSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Services.Interface;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HealthCareSystem.Controllers
{
    [Route("api/chatbox")]
    [ApiController]
    public class AIChatBox : ControllerBase
    {
        private readonly IAiConversationService _aiConversationService;
        private readonly IAiMessageService _aiMessageService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenAIOptions _option;
        public AIChatBox(IAiConversationService aiConversationService, IAiMessageService aiMessageService, IOptions<OpenAIOptions> option, IHttpClientFactory httpClientFactory)
        {
            _aiConversationService = aiConversationService;
            _aiMessageService = aiMessageService;
            _option = option.Value;
            _httpClientFactory = httpClientFactory;
        }
        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] AiChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message is required!");

            if (request.UserId == 0)
                return Unauthorized("User is not logged in.");

            // Get or create conversation
            var conversation = await _aiConversationService.GetConversationByUserId(request.UserId);
            if (conversation == null)
            {
                conversation = new Aiconversation
                {
                    UserId = request.UserId,
                    StartedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };
                await _aiConversationService.CreateConversation(conversation);
            }

            // Prepare history for Gemini API
            var parts = new List<object>
            {
                new { text = "You are a health AI assistant. Respond briefly to health, medical, wellness, or fitness-related queries." }
            };

            foreach (var msg in await _aiMessageService.GetMessagesByUserId(request.UserId))
            {
                parts.Add(new { text = msg.Content });
            }

            parts.Add(new { text = request.Message });

            var payload = new
            {
                contents = new[] { new { parts = parts } }
            };

            // Create client from IHttpClientFactory
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var geminiEndpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_option.ApiKey}";

            // Send request to Gemini API
            var response = await client.PostAsync(geminiEndpoint, content);
            response.EnsureSuccessStatusCode();  // Ensure the response is successful

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // Parse the response from Gemini API
            var aiReply = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";

            // Save messages to database
            var now = DateTime.Now;
            await _aiMessageService.SaveMessage(new[]
            {
                new Aimessage
                {
                    UserId = request.UserId,
                    Sender = "User",
                    Content = request.Message,
                    SentAt = now,
                    IsRead = true
                },
                new Aimessage
                {
                    UserId = request.UserId,
                    Sender = "AI",
                    Content = aiReply,
                    SentAt = now,
                    IsRead = false
                }
            });

            // Update the conversation timestamp
            conversation.UpdatedAt = now;
            await _aiConversationService.UpdateConversation(conversation);

            return Ok(new { aiReply });
        }

        [HttpGet("messages/{userId}")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            var messages = await _aiMessageService.GetMessagesByUserId(userId);
            if (messages == null)
            {
                return NotFound("No messages found for this user.");
            }

            return Ok(messages);
        }
    }
}
