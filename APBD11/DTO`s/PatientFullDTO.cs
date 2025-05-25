namespace WebApplication1.DTO_s;

public class PatientFullDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public List<PrescriptionPatientDTO> Prescriptions { get; set; }
}

public class PrescriptionPatientDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentPatientDTO> Medicaments { get; set; }
    public DoctorPatientDTO Doctor { get; set; }
}

public class DoctorPatientDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}

public class MedicamentPatientDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}