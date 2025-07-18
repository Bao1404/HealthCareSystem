using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCareSystem.Models;
using HealthCareSystem.ViewModels;

namespace HealthCareSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly HealthCareSystemContext _context;

        public PatientController(HealthCareSystemContext context)
        {
            _context = context;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                .Include(p => p.User)
                .Select(p => new PatientViewModel
                {
                    UserId = p.UserId,
                    Email = p.User.Email,
                    FullName = p.User.FullName,
                    PhoneNumber = p.User.PhoneNumber,
                    Address = p.Address,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Allergies = p.Allergies,
                    Weight = p.Weight,
                    Height = p.Height,
                    EmergencyPhoneNumber = p.EmergencyPhoneNumber
                })
                .ToListAsync();

            return View(patients);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View(new PatientViewModel());
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    PasswordHash = model.Password, // Should be hashed in production
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Role = "Patient",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var patient = new Patient
                {
                    UserId = user.UserId,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    BloodType = model.BloodType,
                    Allergies = model.Allergies,
                    Weight = model.Weight,
                    Height = model.Height,
                    EmergencyPhoneNumber = model.EmergencyPhoneNumber,
                    CreatedAt = DateTime.Now,
                    Bmi = model.Height.HasValue && model.Weight.HasValue
                        ? (decimal?)(model.Weight.Value / Math.Pow(model.Height.Value / 100.0, 2))
                        : null
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _context.Patients
                .Include(p => p.User)
                .Select(p => new PatientViewModel
                {
                    UserId = p.UserId,
                    Email = p.User.Email,
                    FullName = p.User.FullName,
                    PhoneNumber = p.User.PhoneNumber,
                    Address = p.Address,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Allergies = p.Allergies,
                    Weight = p.Weight,
                    Height = p.Height,
                    EmergencyPhoneNumber = p.EmergencyPhoneNumber
                })
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel model)
        {
            if (id != model.UserId) return NotFound();

            if (ModelState.IsValid)
            {
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.UserId == id);

                if (patient == null) return NotFound();

                patient.User.Email = model.Email;
                patient.User.FullName = model.FullName;
                patient.User.PhoneNumber = model.PhoneNumber;
                patient.Address = model.Address;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Gender = model.Gender;
                patient.BloodType = model.BloodType;
                patient.Allergies = model.Allergies;
                patient.Weight = model.Weight;
                patient.Height = model.Height;
                patient.EmergencyPhoneNumber = model.EmergencyPhoneNumber;
                patient.UpdatedAt = DateTime.Now;
                patient.Bmi = model.Height.HasValue && model.Weight.HasValue
                    ? (decimal?)(model.Weight.Value / Math.Pow(model.Height.Value / 100.0, 2))
                    : null;

                _context.Update(patient.User);
                _context.Update(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _context.Patients
                .Include(p => p.User)
                .Select(p => new PatientViewModel
                {
                    UserId = p.UserId,
                    Email = p.User.Email,
                    FullName = p.User.FullName,
                    PhoneNumber = p.User.PhoneNumber,
                    Address = p.Address,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Allergies = p.Allergies,
                    Weight = p.Weight,
                    Height = p.Height,
                    EmergencyPhoneNumber = p.EmergencyPhoneNumber,
                    Bmi = p.Bmi
                })
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (patient == null) return NotFound();

            return View(patient);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _context.Patients
                .Include(p => p.User)
                .Select(p => new PatientViewModel
                {
                    UserId = p.UserId,
                    Email = p.User.Email,
                    FullName = p.User.FullName,
                    PhoneNumber = p.User.PhoneNumber,
                    Address = p.Address,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Allergies = p.Allergies,
                    Weight = p.Weight,
                    Height = p.Height,
                    EmergencyPhoneNumber = p.EmergencyPhoneNumber
                })
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var user = await _context.Users.FindAsync(id);

            if (patient != null && user != null)
            {
                _context.Patients.Remove(patient);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}