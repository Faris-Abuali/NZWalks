namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? ImageUrl { get; set; }

        //public Guid DifficultyId { get; set; } // No need, exists on DifficultyDto

        //public Guid RegionId { get; set; } // No need, exists on RegionDto

        public RegionDto? Region { get; set; }

        public DifficultyDto? Difficulty { get; set; }
    }
}
