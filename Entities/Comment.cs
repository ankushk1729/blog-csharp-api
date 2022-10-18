using System.ComponentModel.DataAnnotations;
namespace SM.Entities
{
    public record Comment {
        
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        public string Text { get; set; }

        public User User { get; set; }

        public Guid? UserId { get; set; }

        public Blog Blog { get; set; }

        public Guid? BlogId { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}