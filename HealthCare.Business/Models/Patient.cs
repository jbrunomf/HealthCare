using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HealthCare.Business.Models
{
    public class Patient : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }

        // Identity
        public string? IdentityUserId { get; set; }

        [NotMapped]
        public virtual object? IdentityUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        [NotMapped] public string FullName { get; set; }

    }
}