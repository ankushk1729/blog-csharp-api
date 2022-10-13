using System.ComponentModel.DataAnnotations;

namespace SM.Dtos
{
    public record BlogDto {

        [Required]
        public Guid Id {get; set;}

        [Required]
        public string Content { get; set; }

        [Required]
        public string Cover { get; set; }
        
        [Required]
        public UserDto User {get; set;}

        [Required]
        public Guid UserId {get; set;}

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

    }
}