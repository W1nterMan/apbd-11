
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO_s;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicamentService _medicamentService;
        private readonly IDoctorService _doctorService;

        public PrescriptionsController(IPatientService patientService, IPrescriptionService prescriptionService, IMedicamentService medicamentService, IDoctorService doctorService)
        {
            _doctorService = doctorService;
            _prescriptionService = prescriptionService;
            _patientService = patientService;
            _medicamentService = medicamentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPrescription([FromBody] PrescriptionRequestDTO prescriptionRequestDto)
        {
            if (!await _patientService.PatientExists(prescriptionRequestDto.Patient.IdPatient))
            {
                await _patientService.insertPatient(prescriptionRequestDto.Patient);
            }

            if (!await _medicamentService.MedicamentsExists(prescriptionRequestDto.Medicaments))
            {
                return NotFound("No such medicaments");
            }

            if (!await _doctorService.DoctorExists(prescriptionRequestDto.IdDoctor))
            {
                return NotFound("No such doctor exists");
            }

            if (prescriptionRequestDto.Medicaments.Length > 10)
            {
                return BadRequest("No more than 10 medicaments");
            }

            if (prescriptionRequestDto.DueDate < prescriptionRequestDto.Date)
            {
                return BadRequest("DueDate is earlier than Date");
            }

            var prescription = await _prescriptionService.InsertPrescription(prescriptionRequestDto);
            return Created("", prescription);
        }
    }
    
    
}

