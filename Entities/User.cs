using System.ComponentModel.DataAnnotations;
namespace SM.Entities
{
    public record User {

        [Required]
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Username should be unique")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password length should be more than 8 characters")]
        public string Password { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}