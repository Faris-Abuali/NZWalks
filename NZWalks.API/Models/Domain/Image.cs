using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped]
        public required IFormFile File { get; set; } //excluded from database mapping

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Extension { get; set; }

        public long SizeInBytes { get; set; }

        [Required]
        public string Path { get; set; } = string.Empty;
    }
}
