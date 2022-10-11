using System.ComponentModel.DataAnnotations;
namespace SM.Entities
{
    public record User {

        [Required]
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}