using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APIBlog.Models;
using Microsoft.IdentityModel.Tokens;
namespace APIBlog.Security;

public class SecutiryService : ISecurityService
{
    private readonly IConfiguration _configuration;

    public SecutiryService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HashingSHA256(string text)
    {
        using (SHA256 sha256Has = SHA256.Create())
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] textHashValue = sha256Has.ComputeHash(textBytes);

            StringBuilder builder = new StringBuilder();

            foreach (var textHashByte in textHashValue)
            {
                builder.Append(textHashByte.ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public string GetJwt(User user)
    {
        //Crear info de usuario para el token
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.Name),
            new Claim(ClaimTypes.Role, user.Role.Rol)
        };

        var key = _configuration["Jwt:Key"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtConfig = new JwtSecurityToken
        (
            claims : userClaims,
            expires : DateTime.Now.AddMinutes(30),
            signingCredentials : credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtConfig);

        return tokenString;


    }


}

