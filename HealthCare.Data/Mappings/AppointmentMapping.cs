using HealthCare.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCare.Data.Mappings
{
    class AppointmentMapping : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.DoctorId).IsRequired();
            builder.HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(a => a.PatientId)
                .IsRequired();
            builder.HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(a => a.AppointmentDate).IsRequired();
            builder.Property(a => a.Status).IsRequired().HasConversion<int>();
            builder.Property(a => a.Notes).HasMaxLength(500);
        }
    }
}