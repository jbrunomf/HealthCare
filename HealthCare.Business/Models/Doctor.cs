namespace HealthCare.Business.Models
{
    public class Doctor : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
        public string CRM { get; set; } 
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
