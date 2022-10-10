using System.ComponentModel.DataAnnotations;
namespace GSMS.Dtos
{
    public record CreateItemDto {

        [Required]
        public string name { get; init; }

        [Required]
        [Range(0, 1000)]
        public int quantity { get; init; }   
    }
}