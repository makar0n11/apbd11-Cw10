using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
        
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
        
    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
}