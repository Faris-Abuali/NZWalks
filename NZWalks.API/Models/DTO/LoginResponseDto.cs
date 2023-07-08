using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class LoginResponseDto
    {
        [Required]
        public required string JwtToken { get; set; }
    }
}
