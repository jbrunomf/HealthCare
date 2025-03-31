using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Business.Models;
using HealthCare.Data.Context;
using HealthCare.Data.Migrations;
using Microsoft.AspNetCore.Authorization;
using NuGet.ProjectModel;

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
            if (!User.Identity.IsAuthenticated) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdentityUserId == userId);

            if (patient is null)
                return NotFound("Patient not found");


            ViewBag.Patient = patient;

            var appointments = _context.Appointments
                .Where(a => a.PatientId == patient.Id)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalSchedule);
            return View(await appointments.ToListAsync());
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
                .Include(a => a.MedicalSchedule)
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
            [Bind("DoctorId,PatientId,AppointmentDate,Notes,Status,Id,CreatedAt,UpdatedAt,MedicalScheduleId")]
            Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                //Validar se o schedule está disponível realmente
                var schedule = await _context.MedicalSchedules.FindAsync(appointment.MedicalScheduleId);

                if (schedule is not { IsAvailable: true })
                    return BadRequest("O horário selecionado não está disponível");

                schedule.IsAvailable = false;
                appointment.Status = Appointment.AppointmentStatus.Scheduled;
                appointment.CreatedAt = DateTime.Now;
                _context.Update(schedule);
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

            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FirstName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("DoctorId,PatientId,AppointmentDate,Notes,Status,Id,CreatedAt,UpdatedAt,MedicalScheduleId")]
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
                    var lstSchedules =
                        _context.MedicalSchedules.Where(s => s.Appointments.Contains(appointment));

                    var pastSchedule = lstSchedules.FirstOrDefault(s => s.IsAvailable == false);

                    if (pastSchedule is null)
                        throw new Exception("Schedule not found");


                    var actualSchedule = await _context.MedicalSchedules.FindAsync(appointment.MedicalScheduleId);

                    if (actualSchedule is null)
                    {
                        throw new Exception("Schedule not found");
                    }

                    pastSchedule.IsAvailable = true;
                    _context.Update(pastSchedule);
                    await _context.SaveChangesAsync();

                    _context.Update(actualSchedule);
                    actualSchedule.IsAvailable = false;
                    await _context.SaveChangesAsync();

                    appointment.UpdatedAt = DateTime.Now;
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
                //Only change status
                appointment.Status = Appointment.AppointmentStatus.Cancelled;
                _context.Update(appointment);
                //_context.Appointments.Remove(appointment);

                var schedule = await _context.MedicalSchedules.FindAsync(appointment.MedicalScheduleId);
                if (schedule is not null)
                {
                    schedule.IsAvailable = true;
                    _context.Update(schedule);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        public List<MedicalSchedule> GetAvailableSlots(Guid doctorId, DateTime date)
        {
            var slots = _context.MedicalSchedules
                .Where(s => s.DoctorId == doctorId && s.StartTime.Date == date.Date && s.IsAvailable)
                .OrderBy(s => s.StartTime)
                .ToList();

            return slots;
        }
    }
}