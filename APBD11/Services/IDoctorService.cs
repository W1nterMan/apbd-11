using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IDoctorService
{
    Task<DoctorDTO> InsertDoctor(DoctorDTO doctorDto);
    Task<bool> DoctorExists(int idDoctor);
}