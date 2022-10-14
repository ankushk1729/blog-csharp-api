using System.ComponentModel.DataAnnotations;
using SM.Entities;
namespace SM.Dtos
{
    public record CommentDto {
        
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Text { get; set; }

        public UserDto User { get; set; }

        public Guid? UserId { get; set; }

        public Guid? BlogId { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}