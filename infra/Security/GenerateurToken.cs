using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Infrastructure.Security;

public class GenerateurToken : IGenerateurToken
{
    private readonly IConfiguration _configuration;

    public GenerateurToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generer(Utilisateur utilisateur)
    {
        var cle = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Cle"]!));
        var identifiants = new SigningCredentials(cle, SecurityAlgorithms.HmacSha256);
        var dureeMinutes = int.Parse(_configuration["Jwt:DureeMinutes"]!);

        var revendications = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utilisateur.IdUtilisateur.ToString()),
            new Claim(ClaimTypes.Name, utilisateur.NomUtilisateur),
            new Claim(ClaimTypes.Role, utilisateur.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Emetteur"],
            audience: _configuration["Jwt:Audience"],
            claims: revendications,
            expires: DateTime.UtcNow.AddMinutes(dureeMinutes),
            signingCredentials: identifiants);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
