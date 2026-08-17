namespace ProManLog.Core.DTOs;
public class LigneChiffresDto
{
    public string Periode { get; set; } = string.Empty;
    public decimal ChiffreAffaire { get; set; }
    public decimal Salaire { get; set; }
    public decimal Frais { get; set; }
    public decimal Loyer { get; set; }
    public decimal Facture { get; set; }
    public decimal Essence { get; set; }
    public decimal TotalCharges { get; set; }
    public decimal TvaEncaissee { get; set; }
    public decimal TvaPayee { get; set; }
    public decimal TvaAReverser { get; set; }
    public decimal ResultatAvantImpot { get; set; }
}

public class ChiffresDto
{
    public decimal TauxTva { get; set; }
    public List<LigneChiffresDto> Lignes { get; set; } = new();
    public LigneChiffresDto Total { get; set; } = new();
}
