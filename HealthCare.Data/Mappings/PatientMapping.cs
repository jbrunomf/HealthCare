using HealthCare.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCare.Data.Mappings
{
    public class PatientMapping : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(p => p.Document).HasColumnType("varchar(14)").HasMaxLength(14).IsRequired();
            builder.Property(p => p.DateOfBirth).IsRequired();
            builder.Property(p => p.Email).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.ToTable("Patients");
        }
    }
}