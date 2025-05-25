using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO_s;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class DoctorService : IDoctorService
{
    private readonly DatabaseContext _context;
    
    public DoctorService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<DoctorDTO> InsertDoctor(DoctorDTO doctorDto)
    {
        var doctor = DTOtoDoctor(doctorDto);
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
        
        doctorDto.IdDoctor = doctor.IdDoctor;
        return doctorDto;
    }

    public async Task<bool> DoctorExists(int idDoctor)
    {
        return await _context.Doctors.AnyAsync(d => d.IdDoctor == idDoctor);
    }

    public Doctor DTOtoDoctor(DoctorDTO dto)
    {
        return new Doctor()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }
}