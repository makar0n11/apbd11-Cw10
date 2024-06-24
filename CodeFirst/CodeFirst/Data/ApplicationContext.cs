using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PrescriptionMedicament>().HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new() { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "JD@mail.com"},
            new() { IdDoctor = 2, FirstName = "Ann", LastName = "Smith", Email = "AS@mail.com"},
            new() { IdDoctor = 3, FirstName = "Jack", LastName = "Taylor", Email = "JackiJack@gmail.com"}
        });
        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new() { IdPatient = 1, FirstName = "Maciek", LastName = "Kowlaski", BirthDate = DateTime.Parse("01.01.2001")},
            new() { IdPatient = 2, FirstName = "Anna", LastName = "Lewandowska", BirthDate = DateTime.Parse("02.02.2002") },
            new() { IdPatient = 3, FirstName = "Jacek", LastName = "Placek", BirthDate = DateTime.Parse("03.03.2003") }
        });
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new() {IdMedicament = 1, Description = "Na apetyt", Name = "Apetyter", Type = "Suplement"},
            new() {IdMedicament = 2, Description = "Na ból", Name = "NOBOL", Type = "Przeciwbolowy"},
            new() {IdMedicament = 3, Description = "Placebo", Name = "Niepie", Type = "nic nie robi"}
        });
        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new() {IdPrescription = 1,IdDoctor = 1,IdPatient = 1,
                Date = DateTime.Parse("05.05.2024"),DueDate = DateTime.Parse("10.05.2024")},
            new() {IdPrescription = 2,IdDoctor = 2,IdPatient = 2, 
                Date = DateTime.Parse("06.06.2024"),DueDate = DateTime.Parse("12.06.2024")},
            new() {IdPrescription = 3,IdDoctor = 3,IdPatient = 3, 
                Date = DateTime.Parse("07.07.2024"),DueDate = DateTime.Parse("15.07.2024")}
        });
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new() {IdPrescription = 1, IdMedicament = 1,Details = "Dawac dzieciakowi jak nie chce zrec", Dose = 2},
            new() {IdPrescription = 2, IdMedicament = 2,Details = "nie wychodzic z domu przez tydzien", Dose = 4},
            new() {IdPrescription = 3, IdMedicament = 3,Details = "Prank", Dose = 1}
        });

    }
}