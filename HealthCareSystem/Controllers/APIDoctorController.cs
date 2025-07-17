using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Repositories;
namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIDoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

    

    }
}
