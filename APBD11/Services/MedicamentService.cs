using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class MedicamentService : IMedicamentService
{
    private readonly DatabaseContext _context;
    
    public MedicamentService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<MedicamentDTO> InsertMedicament(MedicamentDTO medicamentDto)
    {
        var medicament = DTOtoMedicament(medicamentDto);
        await _context.Medicaments.AddAsync(medicament);
        await _context.SaveChangesAsync();
        
        medicamentDto.IdMedicament = medicament.IdMedicament;
        return medicamentDto;
    }

    public Medicament DTOtoMedicament(MedicamentDTO dto)
    {
        return new Medicament()
        {
            Name = dto.Name,
            Description = dto.Description,
            IdMedicament = dto.IdMedicament,
            Type = dto.Type
        };
    }


    public async Task<bool> MedicamentsExists(MedicamentPrescriptionDTO[] medicaments)
    {
        foreach (var medicament in medicaments)
        {
            if (!await MedicamentExist(medicament.IdMedicament))
            {
                return false;
            }
        }

        return true;
    }

    public Task<bool> MedicamentExist(int medicamentId)
    {
        var query = _context.Medicaments.Select(m => m);
        var result = _context.Medicaments.AnyAsync(m => m.IdMedicament == medicamentId);
        return result;
    }
}