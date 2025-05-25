using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IPatientService
{
    Task<bool> PatientExists(int patientIdPatient);
    Task<PatientDTO> insertPatient(PatientDTO patientDto);
    Task<PatientFullDTO> GetPatient(int id);
}