using System.ComponentModel.DataAnnotations;
namespace SM.Entities
{
    public record UserRole {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

    }
}