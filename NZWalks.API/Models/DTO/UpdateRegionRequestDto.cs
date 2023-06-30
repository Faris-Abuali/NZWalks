namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        public required string Code { get; set; }

        public required string Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
