using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.DTO_s;
using WebApplication1.Models;
using WebApplication1.Services;

namespace APBD11.Tests;

public class UnitTests
{
    [Fact]
    public async Task AddPrescriptionTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer("Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Integrated Security=False; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Database=tutorial11")
            .Options;

        using var context = new DatabaseContext(options);
        
        var patientService = new PatientService(context);
        var prescriptionService = new PrescriptionService(context);
        var medicamentService = new MedicamentService(context);
        var doctorService = new DoctorService(context);
        
        //clear
        context.Prescriptions.RemoveRange(context.Prescriptions);
        context.Patients.RemoveRange(context.Patients);
        context.Medicaments.RemoveRange(context.Medicaments);
        context.Doctors.RemoveRange(context.Doctors);
        await context.SaveChangesAsync();
        
        //add minimaldata
        var medicament = await medicamentService.InsertMedicament(new MedicamentDTO()
        {
            Name = "RealMedicament",
            Description = ":(",
            Type = "happybiotic",
        });

        var doctor = await doctorService.InsertDoctor(new DoctorDTO()
        {
            FirstName = "Solid",
            LastName = "Snake",
            Email = "metalgearsolid@snake.eat"
        });
        
        var controller = new PrescriptionsController(patientService, prescriptionService, medicamentService,doctorService);
        
        var request = new PrescriptionRequestDTO
        {
            Patient = new PatientDTO
            {
                FirstName = "John",
                LastName = "Signalis",
                Birthday = DateTime.Now.AddYears(-25)
            },
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(5),
            Medicaments = new[]
            {
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(", Dose = 228, Type = "happybiotic"}
            },
            IdDoctor = doctor.IdDoctor
        };
        
        var result = await controller.PostPrescription(request);
        
        Assert.IsType<CreatedResult>(result);
    }
    
    [Fact]
    public async Task AddPrescriptionWithoutExistingMedication()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer("Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Integrated Security=False; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Database=tutorial11")
            .Options;

        using var context = new DatabaseContext(options);
        
        var patientService = new PatientService(context);
        var prescriptionService = new PrescriptionService(context);
        var medicamentService = new MedicamentService(context);
        var doctorService = new DoctorService(context);
        
        //clear
        context.Prescriptions.RemoveRange(context.Prescriptions);
        context.Patients.RemoveRange(context.Patients);
        context.Medicaments.RemoveRange(context.Medicaments);
        context.Doctors.RemoveRange(context.Doctors);
        await context.SaveChangesAsync();
        
        //add minimaldata
        var medicament = await medicamentService.InsertMedicament(new MedicamentDTO()
        {
            Name = "RealMedicament",
            Description = ":(",
            Type = "happybiotic",
        });

        var doctor = await doctorService.InsertDoctor(new DoctorDTO()
        {
            FirstName = "Solid",
            LastName = "Snake",
            Email = "metalgearsolid@snake.eat"
        });
        
        var controller = new PrescriptionsController(patientService, prescriptionService, medicamentService,doctorService);
        
        var request = new PrescriptionRequestDTO
        {
            Patient = new PatientDTO
            {
                FirstName = "John",
                LastName = "Signalis",
                Birthday = DateTime.Now.AddYears(-25)
            },
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(5),
            Medicaments = new[]
            {
                //non existent
                new MedicamentPrescriptionDTO() { IdMedicament = 228, Description = ">:)", Dose = 1337, Type = "evilmechotic"}
            },
            IdDoctor = doctor.IdDoctor
        };
        
        var result = await controller.PostPrescription(request);
        
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task AddPrescriptionAddingNewClientIfNotEsixstCheck()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer("Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Integrated Security=False; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Database=tutorial11")
            .Options;

        using var context = new DatabaseContext(options);
        
        var patientService = new PatientService(context);
        var prescriptionService = new PrescriptionService(context);
        var medicamentService = new MedicamentService(context);
        var doctorService = new DoctorService(context);
        
        //clear
        context.Prescriptions.RemoveRange(context.Prescriptions);
        context.Patients.RemoveRange(context.Patients);
        context.Medicaments.RemoveRange(context.Medicaments);
        context.Doctors.RemoveRange(context.Doctors);
        await context.SaveChangesAsync();
        
        //add minimaldata
        var medicament = await medicamentService.InsertMedicament(new MedicamentDTO()
        {
            Name = "RealMedicament",
            Description = ":(",
            Type = "happybiotic",
        });

        var doctor = await doctorService.InsertDoctor(new DoctorDTO()
        {
            FirstName = "Solid",
            LastName = "Snake",
            Email = "metalgearsolid@snake.eat"
        });
        
        var controller = new PrescriptionsController(patientService, prescriptionService, medicamentService,doctorService);
        
        var request = new PrescriptionRequestDTO
        {
            Patient = new PatientDTO
            {
                FirstName = "Robin",
                LastName = "Crosscode",
                Birthday = DateTime.Now.AddYears(-39)
            },
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(5),
            Medicaments = new[]
            {
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(", Dose = 228, Type = "happybiotic"}
            },
            IdDoctor = doctor.IdDoctor
        };
        
        var actionResult = await controller.PostPrescription(request);
        var createdResult = Assert.IsType<CreatedResult>(actionResult);
        var returnedDto = Assert.IsAssignableFrom<PrescriptionDTO>(createdResult.Value);
        
        Assert.True(await patientService.PatientExists(returnedDto.IdPatient));
    }
    
    [Fact]
    public async Task AddPrescriptionWithAboveMaximumMedicaments()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer("Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Integrated Security=False; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Database=tutorial11")
            .Options;

        using var context = new DatabaseContext(options);
        
        var patientService = new PatientService(context);
        var prescriptionService = new PrescriptionService(context);
        var medicamentService = new MedicamentService(context);
        var doctorService = new DoctorService(context);
        
        //clear
        context.Prescriptions.RemoveRange(context.Prescriptions);
        context.Patients.RemoveRange(context.Patients);
        context.Medicaments.RemoveRange(context.Medicaments);
        context.Doctors.RemoveRange(context.Doctors);
        await context.SaveChangesAsync();
        
        //add minimaldata
        var medicament = await medicamentService.InsertMedicament(new MedicamentDTO()
        {
            Name = "RealMedicament",
            Description = ":(",
            Type = "happybiotic",
        });

        var doctor = await doctorService.InsertDoctor(new DoctorDTO()
        {
            FirstName = "Solid",
            LastName = "Snake",
            Email = "metalgearsolid@snake.eat"
        });
        
        var controller = new PrescriptionsController(patientService, prescriptionService, medicamentService,doctorService);
        
        var request = new PrescriptionRequestDTO
        {
            Patient = new PatientDTO
            {
                FirstName = "John",
                LastName = "Signalis",
                Birthday = DateTime.Now.AddYears(-25)
            },
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(5),
            Medicaments = new[]
            {
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(", Dose = 222, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(2", Dose = 223, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(3", Dose = 248, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(4", Dose = 2258, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(5", Dose = 268, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(6", Dose = 278, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(7", Dose = 288, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(8", Dose = 298, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(9", Dose = 218, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(0", Dose = 128, Type = "happybiotic"},
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(11", Dose = 2118, Type = "happybiotic"}

            },
            IdDoctor = doctor.IdDoctor
        };
        
        var result = await controller.PostPrescription(request);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task AddPrescriptionWithNotAppropriateDate()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer("Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Integrated Security=False; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Database=tutorial11")
            .Options;

        using var context = new DatabaseContext(options);
        
        var patientService = new PatientService(context);
        var prescriptionService = new PrescriptionService(context);
        var medicamentService = new MedicamentService(context);
        var doctorService = new DoctorService(context);
        
        //clear
        context.Prescriptions.RemoveRange(context.Prescriptions);
        context.Patients.RemoveRange(context.Patients);
        context.Medicaments.RemoveRange(context.Medicaments);
        context.Doctors.RemoveRange(context.Doctors);
        await context.SaveChangesAsync();
        
        //add minimaldata
        var medicament = await medicamentService.InsertMedicament(new MedicamentDTO()
        {
            Name = "RealMedicament",
            Description = ":(",
            Type = "happybiotic",
        });

        var doctor = await doctorService.InsertDoctor(new DoctorDTO()
        {
            FirstName = "Solid",
            LastName = "Snake",
            Email = "metalgearsolid@snake.eat"
        });
        
        var controller = new PrescriptionsController(patientService, prescriptionService, medicamentService,doctorService);
        
        var request = new PrescriptionRequestDTO
        {
            Patient = new PatientDTO
            {
                FirstName = "John",
                LastName = "Signalis",
                Birthday = DateTime.Now.AddYears(-25)
            },
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(-55),
            Medicaments = new[]
            {
                new MedicamentPrescriptionDTO() { IdMedicament = medicament.IdMedicament, Description = ":(", Dose = 228, Type = "happybiotic"}
            },
            IdDoctor = doctor.IdDoctor
        };
        
        var result = await controller.PostPrescription(request);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    
}
