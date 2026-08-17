namespace ProManLog.Core.Entities;
public class Historique
{
    public int IdHistorique { get; set; }
    public DateTime DateAction { get; set; }
    public string TypeAction { get; set; } = string.Empty;
    public string TypeEntite { get; set; } = string.Empty;
    public int? IdEntite { get; set; }
    public string Description { get; set; } = string.Empty;
}
