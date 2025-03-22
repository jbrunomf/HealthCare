namespace HealthCare.Business.Models
{
    public class Patient : Entity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Document { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
