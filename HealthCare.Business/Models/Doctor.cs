using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HealthCare.Business.Models
{
    public class Doctor : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }

        [Required]
        public Guid SpecialtyId { get; set; }

        public Specialty? Specialty { get; set; }

        public string CRM { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }

        // Identity
        public string? IdentityUserId { get; set; }

        [NotMapped]
        public virtual object? IdentityUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
