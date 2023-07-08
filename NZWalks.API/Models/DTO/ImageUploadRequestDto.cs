using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public required IFormFile File { get; set; }

        [Required]
        public required string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
