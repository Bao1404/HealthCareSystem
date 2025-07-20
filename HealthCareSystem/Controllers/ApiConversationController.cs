using BusinessObjects;
using HealthCareSystem.Controllers.dto;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;

namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;

        public ApiConversationController(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetConversationsByDoctor(int doctorId)
        {
            try
            {
                var conversations = await _conversationRepository.GetConversationsByDoctorId(doctorId);

                var result = conversations.Select(c => new ConversationDto
                {
                    ConversationId = c.ConversationId,
                    PatientUserId = c.PatientUserId,
                    DoctorUserId = c.DoctorUserId,
                    UpdatedAt = c.UpdatedAt,
                    PatientUser = new UserDto
                    {
                        UserId = c.PatientUser.UserId,
                        Email = c.PatientUser.Email,
                        FullName = c.PatientUser.FullName,
                        Role = c.PatientUser.Role,
                        PhoneNumber = c.PatientUser.PhoneNumber,
                        AvatarUrl = c.PatientUser.AvatarUrl
                    },
                    DoctorUser = new UserDto
                    {
                        UserId = c.DoctorUser.UserId,
                        Email = c.DoctorUser.Email,
                        FullName = c.DoctorUser.FullName,
                        Role = c.DoctorUser.Role,
                        PhoneNumber = c.DoctorUser.PhoneNumber,
                        AvatarUrl = c.DoctorUser.AvatarUrl
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("patient/{patientID}")]
        public async Task<IActionResult> GetConversationsByPatient(int patientID)
        {
            try
            {
                var conversations = await _conversationRepository.GetConversationsByPatientId(patientID);

                var result = conversations.Select(c => new ConversationDto
                {
                    ConversationId = c.ConversationId,
                    PatientUserId = c.PatientUserId,
                    DoctorUserId = c.DoctorUserId,
                    UpdatedAt = c.UpdatedAt,
                    PatientUser = new UserDto
                    {
                        UserId = c.PatientUser.UserId,
                        Email = c.PatientUser.Email,
                        FullName = c.PatientUser.FullName,
                        Role = c.PatientUser.Role,
                        PhoneNumber = c.PatientUser.PhoneNumber,
                        AvatarUrl = c.PatientUser.AvatarUrl
                    },
                    DoctorUser = new UserDto
                    {
                        UserId = c.DoctorUser.UserId,
                        Email = c.DoctorUser.Email,
                        FullName = c.DoctorUser.FullName,
                        Role = c.DoctorUser.Role,
                        PhoneNumber = c.DoctorUser.PhoneNumber,
                        AvatarUrl = c.DoctorUser.AvatarUrl
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

    }
}
