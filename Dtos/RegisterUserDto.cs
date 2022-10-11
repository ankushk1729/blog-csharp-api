using System.ComponentModel.DataAnnotations;
namespace SM.Dtos
{
    public record RegisterUserDto {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}