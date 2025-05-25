using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class PatientService : IPatientService
{
    private readonly DatabaseContext _context;
    
    public PatientService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<bool> PatientExists(int patientIdPatient)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == patientIdPatient);
    }

    public async Task<PatientDTO> insertPatient(PatientDTO patientDto)
    {
        var patient = DTOtoPatient(patientDto);
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        patientDto.IdPatient = patient.IdPatient;
        return patientDto;


    }

    public async Task<PatientFullDTO> GetPatient(int id)
    {
        var patient = await _context.Patients.Where(p => p.IdPatient == id)
            .Select(p => new PatientFullDTO()
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.BirthDate,
                Prescriptions = p.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionPatientDTO()
                    {
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        IdPrescription = pr.IdPrescription,
                        Doctor = new DoctorPatientDTO
                        {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName
                        }, 
                        Medicaments = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentPatientDTO()
                            {
                                IdMedicament = pm.Medicament.IdMedicament,
                                Name = pm.Medicament.Name,
                                Description = pm.Medicament.Description,
                                Dose = pm.Dose
                            })
                            .ToList()
                    })
                    .ToList()
            }).FirstOrDefaultAsync();
        return patient;
    }

    public Patient DTOtoPatient(PatientDTO patientDto)
    {
        return new Patient()
        {
            IdPatient = patientDto.IdPatient,
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            BirthDate = patientDto.Birthday
        };
    }
}