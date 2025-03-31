using HealthCare.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCare.Data.Mappings
{
    public class MedicalScheduleMapping : IEntityTypeConfiguration<MedicalSchedule>
    {
        public void Configure(EntityTypeBuilder<MedicalSchedule> builder)
        {
            builder.ToTable("MedicalSchedules");
            builder.HasKey(ms => ms.Id);
            builder.Property(ms => ms.Id)
                .ValueGeneratedOnAdd();
            builder.HasOne(ms => ms.Doctor)
                .WithMany(d => d.MedicalSchedules)
                .HasForeignKey(ms => ms.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(ms => ms.StartTime)
                .IsRequired();
            builder.Property(ms => ms.EndTime)
                .IsRequired();
            builder.Property(ms => ms.IsAvailable)
                .IsRequired();
        }
    }
}