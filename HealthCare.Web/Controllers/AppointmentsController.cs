using System.Security.Claims;
using HealthCare.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Business.Models;
using HealthCare.Business.Notifications;
using HealthCare.Data.Context;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IMedicalScheduleService _medicalScheduleService;
        private readonly INotifier _notifier;

        public AppointmentsController(AppDbContext context, IEmailService emailService, IPatientService patientService,
            IAppointmentService appointmentService, IMedicalScheduleService medicalScheduleService, INotifier notifier)
        {
            _context = context;
            _emailService = emailService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _medicalScheduleService = medicalScheduleService;
            _notifier = notifier;
        }

        // GET: Appointments
        [Authorize(Roles = "Admin,Patient,Doctor,Paciente")]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _patientService.FindAsync(userId);

            if (patient is null)
                return NotFound("Patient not found");

            ViewBag.Patient = patient;

            var appointments = _context.Appointments
                .Where(a => a.PatientId == patient.Id)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalSchedule);

            foreach (var notification in _notifier.GetNotificationAsync())
            {
                ModelState.AddModelError(string.Empty, notification.Message);
            }

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

        [Authorize(Roles = "Admin,Patient,Paciente")]
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
                var schedule = await _medicalScheduleService.GetMedicalScheduleByIdAsync(appointment.MedicalScheduleId);

                if (schedule is not { IsAvailable: true })
                    return BadRequest("O horário selecionado não está disponível");

                var isScheduleValid = await _medicalScheduleService.MarkAsUnavailable(schedule);

                if (isScheduleValid)
                {
                    await _appointmentService.CreateAsync(appointment);
                    var patient = await _context.Patients.FindAsync(appointment.PatientId);
                    if (patient != null)
                    {
                        await _emailService.SendEmailAsync(patient.Email, "Medical Schedule", "Medical Schedule Done!");
                    }
                }

                if (!_notifier.HasNotifications()) return RedirectToAction(nameof(Index), appointment.PatientId);

                foreach (var notification in _notifier.GetNotificationAsync())
                {
                    ModelState.AddModelError(string.Empty, notification.Message);
                }
            }

            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FirstName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var appointment = await _appointmentService.FindAsync(id);

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
                    var pastSchedule = await _medicalScheduleService.GetLastValidScheduleForAppointment(appointment);

                    if (pastSchedule is null)
                        throw new Exception("Schedule not found");

                    var actualSchedule = await _medicalScheduleService.FindAsync(appointment.MedicalScheduleId);

                    if (actualSchedule is null)
                        throw new Exception("Schedule not found");

                    await _medicalScheduleService.MarkAsAvailable(pastSchedule);

                    await _medicalScheduleService.MarkAsUnavailable(actualSchedule);

                    await _appointmentService.UpdateAsync(appointment);
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

            var appointment = await _appointmentService.FindAsync(id.Value);
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
            var appointment = await _appointmentService.FindAsync(id);
            if (appointment != null)
            {
                await _appointmentService.MarkAsCancelled(appointment);

                var schedule = await _medicalScheduleService.FindAsync(appointment.MedicalScheduleId);

                if (schedule is not null)
                {
                    await _medicalScheduleService.MarkAsAvailable(schedule);
                }
            }

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