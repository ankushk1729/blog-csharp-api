using System.ComponentModel.DataAnnotations;
namespace SM.Entities
{
    public record Role {
        
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}