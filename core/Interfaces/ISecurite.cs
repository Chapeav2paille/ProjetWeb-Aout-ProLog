using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IHacheurMotDePasse
{
    string Hacher(string motDePasse);
    bool Verifier(string motDePasse, string hash);
}

public interface IGenerateurToken
{
    string Generer(Utilisateur utilisateur);
}
