using WebApplication1.DTO_s;

namespace WebApplication1.Services;

public interface IMedicamentService
{
    public Task<bool> MedicamentsExists(MedicamentPrescriptionDTO[] medicaments);
    public Task<bool> MedicamentExist(int medicamentId);
}