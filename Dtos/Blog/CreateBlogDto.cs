using System.ComponentModel.DataAnnotations;

namespace SM.Dtos
{
    public record CreateBlogDto {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Cover { get; set; }
    }
}