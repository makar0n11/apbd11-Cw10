using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models;

public class PrescriptionMedicament
{
    [Key]
    public int IdMedicament { get; set; }
        
    [Key]
    public int IdPrescription { get; set; }
        
    public int Dose { get; set; }
    [MaxLength(100)]
    public string? Details { get; set; }
        
    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; } = null!;
        
    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; } = null!;
}