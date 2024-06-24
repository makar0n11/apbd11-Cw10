using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
        
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? Description { get; set; }
    [MaxLength(100)]    
    public string? Type { get; set; }
}