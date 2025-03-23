using HealthCare.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCare.Data.Mappings
{
    public class DoctorMapping : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);

            builder.Property(d => d.Document)
                .IsRequired()
                .HasColumnType("nvarchar(14)")
                .HasMaxLength(14);

            builder.Property(d => d.CRM)
                .IsRequired()
                .HasColumnType("nvarchar(20)")
                .HasMaxLength(20);

            builder.Property(d => d.DateOfBirth)
                .IsRequired();

            builder.Property(d => d.IsActive)
                .IsRequired();

            builder.Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(d => d.Specialty)
                .WithMany()
                .HasForeignKey(d => d.SpecialtyId);

            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Doctor>(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.ToTable("Doctors");
        }
    }
}