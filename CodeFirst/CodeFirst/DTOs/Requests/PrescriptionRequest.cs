namespace CodeFirst.DTOs.Requests;

public class PrescriptionRequest
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdDoctor { get; set; }
    public PatientRequest Patient { get; set; } = null!;
    public ICollection<MedicamentRequest>? Medicaments { get; set; }
}