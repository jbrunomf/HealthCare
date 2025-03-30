using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using HealthCare.Data.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdentityUserId == userId);

                if (patient is null)
                    return NotFound("Patient not found");


                ViewBag.Patient = patient;

                var appDbContext = _context.Appointments
                    .Where(a => a.PatientId == patient.Id)
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient);
                return View(await appDbContext.ToListAsync());
            }

            return NotFound();
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [Authorize(Roles = "Admin, Paciente")]
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = _context.Patients.FirstOrDefault(p => p.IdentityUserId == userId);
            
            if (patient is null) return NotFound("Patient not found");
            
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FirstName");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", patient);
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("DoctorId,PatientId,AppointmentDate,Notes,Status,Id,CreatedAt,UpdatedAt")]
            Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                //appointment.Id = Guid.NewGuid();
                appointment.Status = Appointment.AppointmentStatus.Scheduled;
                appointment.CreatedAt = DateTime.Now;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), appointment.PatientId);
            }

            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "CRM", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Document", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "CRM", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Document", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("DoctorId,PatientId,AppointmentDate,Notes,Status,Id,CreatedAt,UpdatedAt")]
            Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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

            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "CRM", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Document", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}