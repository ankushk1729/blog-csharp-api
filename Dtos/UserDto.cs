using System.ComponentModel.DataAnnotations;
namespace SM.Dtos
{
    public record UserDto {

        [Required]
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string Role {get; init;}

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}