using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;
    
    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<PrescriptionDTO> InsertPrescription(PrescriptionRequestDTO prescriptionRequestDto)
    {
        var prescription = DTOtoPrescription(prescriptionRequestDto);
        
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
        
        foreach (var medDto in prescriptionRequestDto.Medicaments)
        {
            var medicament = await _context.Medicaments
                .FirstOrDefaultAsync(m => m.IdMedicament == medDto.IdMedicament);

            _context.PrescriptionMedicaments.AddAsync(new PrescriptionMedicament()
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = medDto.IdMedicament,
                Dose = medDto.Dose,
                Details = medDto.Description ?? ""
            });
        }
        
        await _context.SaveChangesAsync();
        
        return new PrescriptionDTO()
        {
            IdDoctor = prescription.IdDoctor,
            IdPrescription = prescription.IdPrescription,
            IdPatient = prescription.IdPatient,
            Date = prescription.Date,
            DueDate = prescription.DueDate
        };
    }

    public Prescription DTOtoPrescription(PrescriptionRequestDTO dto)
    {
        return new Prescription()
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdPatient = dto.Patient.IdPatient,
            IdDoctor = dto.IdDoctor
        };
    }

    public Task<Prescription> GetPrescription(int prescpriptionId)
    {
        throw new NotImplementedException();
    }
}