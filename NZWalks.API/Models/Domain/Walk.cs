namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? ImageUrl { get; set; }

        public Guid DifficultyId { get; set; }

        public Guid RegionId { get; set; }

        // Navigation Properties: This will tell Entity Framework to associate these entities with Walk entity
        public required Difficulty Difficulty { get; set; }

        public required Region Region { get; set; }

    }
}
