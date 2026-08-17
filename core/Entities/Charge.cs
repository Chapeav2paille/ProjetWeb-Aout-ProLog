namespace ProManLog.Core.Entities;
public class Charge
{
    public int IdCharge { get; set; }
    public DateTime Periode { get; set; }
    public string TypeCharge { get; set; } = string.Empty;
    public decimal Montant { get; set; }
}

public static class TypeCharge
{
    public const string Salaire = "Salaire";
    public const string Frais = "Frais";
    public const string Loyer = "Loyer";
    public const string Facture = "Facture";
    public const string Essence = "Essence";
}
