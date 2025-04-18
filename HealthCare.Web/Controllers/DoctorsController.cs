﻿using System.Security.Claims;
using HealthCare.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAppointmentService _service;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMedicalScheduleService _medicalScheduleService;
        private readonly INotifier _notifier;
        private readonly IEmailService _emailService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public DoctorsController(AppDbContext context, IAppointmentService service,
            UserManager<IdentityUser> userManager,
            IMedicalScheduleService medicalScheduleService, INotifier notifier, IEmailService emailService,
            IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
        {
            _context = context;
            _service = service;
            _userManager = userManager;
            _medicalScheduleService = medicalScheduleService;
            _notifier = notifier;
            _emailService = emailService;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Doctors.Include(d => d.Specialty);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Description");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "FirstName,LastName,Document,SpecialtyId,CRM,Email,DateOfBirth,IsActive,Id,CreatedAt,UpdatedAt,IdentityUserId, Username, Password")]
            Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                //Identity
                var identityUser = new IdentityUser { UserName = doctor.Email, Email = doctor.Email };
                var result = await _userManager.CreateAsync(identityUser, doctor.Password);

                if (result.Succeeded)
                {
                    doctor.CreatedAt = DateTime.Now;
                    await _userManager.AddToRoleAsync(identityUser, "Doctor");
                    doctor.IdentityUserId = identityUser.Id;

                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Description", doctor.SpecialtyId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Description", doctor.SpecialtyId);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("FirstName,LastName,Document,SpecialtyId,CRM,Email,DateOfBirth,IsActive,Id,CreatedAt,UpdatedAt")]
            Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    doctor.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "Id", "Description", doctor.SpecialtyId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(Guid id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Appointments()
        {
            if (!User.Identity.IsAuthenticated) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = _doctorService.Find(x => x.IdentityUserId == userId).Result.FirstOrDefault();

            if (doctor is null)
                throw new Exception("Doctor not found");

            ViewBag.Doctor = doctor;

            var appointments = _context.Appointments
                .Where(a => a.DoctorId == doctor.Id)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalSchedule);

            foreach (var notification in _notifier.GetNotificationAsync())
            {
                ModelState.AddModelError(string.Empty, notification.Message);
            }

            return View(await appointments.ToListAsync());
        }
    }
}