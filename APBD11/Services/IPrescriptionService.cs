using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IPrescriptionService
{
    Task<PrescriptionDTO> InsertPrescription(PrescriptionRequestDTO prescriptionRequestDto);
    Task<Prescription> GetPrescription(int prescpriptionId);
}