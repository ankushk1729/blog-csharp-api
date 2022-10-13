using System.ComponentModel.DataAnnotations;

namespace SM.Entities
{
    public record Blog {

        [Required]
        public Guid Id {get; set;}

        [Required]
        public string Content { get; set; }

        [Required]
        public string Cover { get; set; }
        
        [Required]
        public User User {get; set;}

        [Required]
        public Guid UserId {get; set;}

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

    }
}