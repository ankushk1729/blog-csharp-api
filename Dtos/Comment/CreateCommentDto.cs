using System.ComponentModel.DataAnnotations;

namespace SM.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public Guid BlogId { get; set; }
    }
}