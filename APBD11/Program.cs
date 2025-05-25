using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<IMedicamentService,MedicamentService>();
builder.Services.AddScoped<IPatientService,PatientService>();
builder.Services.AddScoped<IPrescriptionService,PrescriptionService>();
builder.Services.AddScoped<IDoctorService,DoctorService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();