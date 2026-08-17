namespace ProManLog.Core.Entities;
public class Client
{
    public int IdClient { get; set; }
    public string TypeClient { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
}
