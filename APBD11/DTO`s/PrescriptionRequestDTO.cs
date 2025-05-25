using WebApplication1.Models;

namespace WebApplication1.DTO_s;

public class PrescriptionRequestDTO
{
    public PatientDTO Patient { get; set; }
    public MedicamentPrescriptionDTO[] Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdDoctor { get; set; }
}

public class PatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
}

public class MedicamentPrescriptionDTO
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}
