using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        [MinLength(2)]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public required string Description { get; set; }

        [Range(0, 50)]
        public double LengthInKm { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
