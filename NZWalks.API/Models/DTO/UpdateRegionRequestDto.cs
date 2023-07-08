using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [MinLength(3, ErrorMessage = "Code should be 3 characters long")]
        [MaxLength(3, ErrorMessage = "Code should be 3 characters long")]
        public required string Code { get; set; }

        [MaxLength(100, ErrorMessage = "Name should be a maximum of 100 characters")]
        public required string Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
