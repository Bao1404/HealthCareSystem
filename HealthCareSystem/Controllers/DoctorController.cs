using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCareSystem.Models;
using HealthCareSystem.ViewModels;

namespace HealthCareSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HealthCareSystemContext _context;

        public DoctorController(HealthCareSystemContext context)
        {
            _context = context;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;

            var todayCount = await _context.Appointments
                .Where(a => a.AppointmentDateTime.Date == today)
                .CountAsync();

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .ToListAsync();

            ViewBag.TodayCount = todayCount;

            return View(doctors);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            ViewBag.Specialties = _context.Specialties.ToList();
            return View(new DoctorViewModel());
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    PasswordHash = model.Password, // Hash in production
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Role = "Doctor",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    SpecialtyId = model.SpecialtyId,
                    Qualifications = model.Qualifications,
                    Experience = model.Experience,
                    Bio = model.Bio,
                    Rating = model.Rating
                };
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Specialties = _context.Specialties.ToList();
            return View(model);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctor == null) return NotFound();

            var model = new DoctorViewModel
            {
                UserId = doctor.UserId,
                Email = doctor.User.Email,
                FullName = doctor.User.FullName,
                PhoneNumber = doctor.User.PhoneNumber,
                SpecialtyId = doctor.SpecialtyId ?? 0,
                Qualifications = doctor.Qualifications,
                Experience = doctor.Experience,
                Bio = doctor.Bio,
                Rating = doctor.Rating
            };

            ViewBag.Specialties = _context.Specialties.ToList();
            return View(model);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorViewModel model)
        {
            if (id != model.UserId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null) return NotFound();

                    user.Email = model.Email;
                    user.FullName = model.FullName;
                    user.PhoneNumber = model.PhoneNumber;

                    var doctor = await _context.Doctors
                        .FirstOrDefaultAsync(d => d.UserId == id);
                    if (doctor == null) return NotFound();

                    doctor.SpecialtyId = model.SpecialtyId;
                    doctor.Qualifications = model.Qualifications;
                    doctor.Experience = model.Experience;
                    doctor.Bio = model.Bio;
                    doctor.Rating = model.Rating;

                    _context.Update(user);
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Specialties = _context.Specialties.ToList();
            return View(model);
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctor == null) return NotFound();

            var model = new DoctorViewModel
            {
                UserId = doctor.UserId,
                Email = doctor.User.Email,
                FullName = doctor.User.FullName,
                PhoneNumber = doctor.User.PhoneNumber
            };

            return View(model);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctor == null) return NotFound();

            var model = new DoctorViewModel
            {
                UserId = doctor.UserId,
                Email = doctor.User.Email,
                FullName = doctor.User.FullName,
                PhoneNumber = doctor.User.PhoneNumber,
                SpecialtyId = doctor.SpecialtyId ?? 0,
                Qualifications = doctor.Qualifications,
                Experience = doctor.Experience,
                Bio = doctor.Bio,
                Rating = doctor.Rating
            };

            ViewBag.SpecialtyName = doctor.Specialty?.Name;
            return View(model);
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.UserId == id);
        }
    }
}