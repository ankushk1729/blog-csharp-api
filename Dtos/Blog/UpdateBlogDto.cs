using System.ComponentModel.DataAnnotations;

namespace SM.Dtos
{
    public record UpdateBlogDto {

        public string? Title { get; set; }
        
        public string? Content { get; set; }

        public string? Cover { get; set; }
    }
}