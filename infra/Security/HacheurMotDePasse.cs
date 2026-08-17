using System.Security.Cryptography;
using System.Text;
using ProManLog.Core.Interfaces;

namespace ProManLog.Infrastructure.Security;

public class HacheurMotDePasse : IHacheurMotDePasse
{
    public string Hacher(string motDePasse)
    {
        var octets = SHA256.HashData(Encoding.UTF8.GetBytes(motDePasse));
        return Convert.ToHexString(octets).ToLowerInvariant();
    }

    public bool Verifier(string motDePasse, string hash)
    {
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(Hacher(motDePasse)),
            Encoding.UTF8.GetBytes(hash));
    }
}
