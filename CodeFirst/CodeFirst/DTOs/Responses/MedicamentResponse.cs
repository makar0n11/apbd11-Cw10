namespace CodeFirst.DTOs.Responses;

public class MedicamentResponse
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Type { get; set; }
    public int Dose { get; set; }
    public string? Details { get; set; }
}