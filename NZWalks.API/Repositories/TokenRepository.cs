using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // public string CreateJwtToken(IdentityUser user, List<string> roles)
        // {
        //     // Create claims
        //     var claims = new List<Claim>();
        //
        //     claims.Add(new Claim(ClaimTypes.Email, user.Email));
        //
        //     //roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        //
        //     foreach (var role in roles)
        //     {
        //         claims.Add(new Claim(ClaimTypes.Role, role));
        //     }
        //
        //     var key = configuration["Jwt:Key"];
        //
        //     var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //
        //     // Specify the key & the hashing algorithm:
        //     var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //
        //     var token = new JwtSecurityToken(
        //         configuration["Jwt:Issuer"],
        //         configuration["Jwt:Audience"],
        //         claims: claims,
        //         expires: DateTime.Now.AddMinutes(50),
        //         signingCredentials: credentials);
        //
        //     /***
        //      * JWE: JSON Web Encryption
        //      *  consists of 3 main components:
        //      *       - Protected header: A JSON object containing metadata about the encryption algorithm and other parameters.
        //      *       - Encrypted key: The symmetric or asymmetric key used for encryption.
        //      *       - Ciphertext: The encrypted payload or data.
        //      *
        //      *
        //      * JWS (JSON Web Signature)
        //      *  consists of 3 main components:
        //      *       - Protected header: A JSON object containing metadata about the signature algorithm and other parameters.
        //      *       - Payload: The data being transmitted or the JSON object.
        //      *       - Signature: The digital signature created using a private key and applied to the header and payload.
        //      * 
        //      */
        //
        //     // Since we set a SigningCredentials object, then the JWT will be signed, not encrypted.
        //     return new JwtSecurityTokenHandler().WriteToken(token);
        // }
    }
}
