using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIDoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        // GET: api/doctor/search
        [HttpGet("search")]
        public async Task<ActionResult<List<Doctor>>> SearchDoctors([FromQuery] string fullName,
                                                           [FromQuery] string phoneNumber,
                                                           [FromQuery] string email,
                                                           [FromQuery] string specialtyName)
        {
            var doctors = await _doctorRepository.SearchDoctorsAsync(fullName, phoneNumber, email, specialtyName);
            return Ok(doctors);
        }

    }
}
