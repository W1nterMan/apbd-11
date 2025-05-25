using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey(nameof(IdPrescription), nameof(IdMedicament))]

public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }

    public int Dose { get; set; }
    public string Details { get; set; }

    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
    
}