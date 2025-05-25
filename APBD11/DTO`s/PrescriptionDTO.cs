namespace WebApplication1.DTO_s;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdDoctor { get; set; }
    public int IdPatient { get; set; }
}