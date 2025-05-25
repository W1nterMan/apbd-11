using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        
        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFullPatientInfo(int id)
        {
            var patient = await _patientService.GetPatient(id);
            return Ok(patient);
        }
    }
}

