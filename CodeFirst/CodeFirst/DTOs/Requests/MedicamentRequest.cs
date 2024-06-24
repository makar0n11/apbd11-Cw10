namespace CodeFirst.DTOs.Requests;

public class MedicamentRequest
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string? Details { get; set; }
}