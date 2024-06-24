using Microsoft.AspNetCore.Mvc;
using CodeFirst.Data;
using CodeFirst.DTOs.Requests;
using CodeFirst.DTOs.Responses;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PatientsController(ApplicationContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patient = _context.Patients.FirstOrDefault(p=>p.IdPatient==id);
        if (patient == null)
        {
            return NotFound();
        }

        var prescription = _context.Prescriptions.Where(p => p.IdPatient == id)
            .Include(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Doctor)
            .OrderBy(p => p.DueDate).ToList();

        var patientDet = new PatientResponse()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescription.Select(p=>new PrescriptionResponse
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Medicaments = p.PrescriptionMedicaments.Select(pm=> new MedicamentResponse
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Dose = pm.Dose,
                }).ToList(),
                Doctor = new DoctorResponse
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                }
            }).ToList()
        };

        return Ok(patientDet);
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequest pr)
    {
        if (pr.Medicaments.Count > 10)
        {
            return BadRequest("wiecej niz 10 lekow");
        }

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.FirstName == pr.Patient.FirstName
                                                                       && p.LastName == pr.Patient.LastName);

        if(patient == null)
        {
            patient = new Patient()
            {
                FirstName = pr.Patient.FirstName,
                LastName = pr.Patient.LastName,
                BirthDate = pr.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var m in pr.Medicaments)
        {
            var medciament = _context.Medicaments.Find(m.IdMedicament);
            if (medciament ==null)
            {
                return BadRequest();
            }
        }

        if (pr.DueDate < pr.Date)
        {
            return BadRequest();
        }

        var prescription = new Prescription()
        {
            Date = pr.Date,
            DueDate = pr.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = pr.IdDoctor
        };

        _context.Prescriptions.Add(prescription);
        _context.SaveChanges();

        foreach (var m in pr.Medicaments)
        {
            _context.PrescriptionMedicaments.Add(new PrescriptionMedicament()
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            });
        }

        _context.SaveChanges();

        return Ok(prescription);
    }

    

}