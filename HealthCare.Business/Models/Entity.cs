using System.ComponentModel;

namespace HealthCare.Business.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}