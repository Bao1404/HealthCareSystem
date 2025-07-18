using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CallController : ControllerBase
{
    [HttpPost("start")]
    public IActionResult StartVoiceCall([FromBody] VoiceCallRequest request)
    {
        // Generate a unique room ID (e.g., use GUID or another mechanism)
        var roomId = Guid.NewGuid().ToString();

        // Return a JSON response with roomId, doctorId, and patientId
        var response = new
        {
            RoomId = roomId,
            DoctorId = request.DoctorId,
            PatientId = request.PatientId
        };

        return Ok(response); // Return HTTP 200 with the generated data
    }
}

public class VoiceCallRequest
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
}
