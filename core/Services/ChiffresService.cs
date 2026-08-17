using System.Globalization;
using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class ChiffresService : IChiffresService
{
    private const decimal TauxTva = 0.20m;
    private const int NombreMois = 6;

    private readonly IPrestationRepository _prestationRepository;
    private readonly IChargeRepository _chargeRepository;

    public ChiffresService(IPrestationRepository prestationRepository, IChargeRepository chargeRepository)
    {
        _prestationRepository = prestationRepository;
        _chargeRepository = chargeRepository;
    }

    public async Task<ChiffresDto> GetChiffresAsync()
    {
        var premierMois = PremierJourDuMois(DateTime.Today).AddMonths(-(NombreMois - 1));

        var chiffreAffaire = await _prestationRepository.GetChiffreAffaireParMoisAsync(premierMois);
        var charges = await _chargeRepository.GetDepuisAsync(premierMois);

        var chiffres = new ChiffresDto { TauxTva = TauxTva };

        for (var rang = 0; rang < NombreMois; rang++)
        {
            var mois = premierMois.AddMonths(rang);
            var chargesDuMois = charges.Where(charge => charge.Periode.Year == mois.Year
                                                     && charge.Periode.Month == mois.Month).ToList();

            var ligne = new LigneChiffresDto
            {
                Periode = mois.ToString("MMMM yyyy", new CultureInfo("fr-FR")),
                ChiffreAffaire = chiffreAffaire
                    .Where(point => point.Annee == mois.Year && point.Mois == mois.Month)
                    .Sum(point => point.Montant),
                Salaire = Total(chargesDuMois, TypeCharge.Salaire),
                Frais = Total(chargesDuMois, TypeCharge.Frais),
                Loyer = Total(chargesDuMois, TypeCharge.Loyer),
                Facture = Total(chargesDuMois, TypeCharge.Facture),
                Essence = Total(chargesDuMois, TypeCharge.Essence)
            };

            Calculer(ligne);
            chiffres.Lignes.Add(ligne);
        }

        chiffres.Total = ConstruireTotal(chiffres.Lignes);
        return chiffres;
    }

    private static void Calculer(LigneChiffresDto ligne)
    {
        ligne.TotalCharges = ligne.Salaire + ligne.Frais + ligne.Loyer + ligne.Facture + ligne.Essence;
        ligne.TvaEncaissee = Arrondir(ligne.ChiffreAffaire * TauxTva);
        ligne.TvaPayee = Arrondir(ChargesDeductibles(ligne) * TauxTva);
        ligne.TvaAReverser = ligne.TvaEncaissee - ligne.TvaPayee;
        ligne.ResultatAvantImpot = ligne.ChiffreAffaire - ligne.TotalCharges;
    }

    private static LigneChiffresDto ConstruireTotal(List<LigneChiffresDto> lignes)
    {
        var total = new LigneChiffresDto
        {
            Periode = "Total",
            ChiffreAffaire = lignes.Sum(ligne => ligne.ChiffreAffaire),
            Salaire = lignes.Sum(ligne => ligne.Salaire),
            Frais = lignes.Sum(ligne => ligne.Frais),
            Loyer = lignes.Sum(ligne => ligne.Loyer),
            Facture = lignes.Sum(ligne => ligne.Facture),
            Essence = lignes.Sum(ligne => ligne.Essence)
        };
        Calculer(total);
        return total;
    }

    private static decimal ChargesDeductibles(LigneChiffresDto ligne) =>
        ligne.Frais + ligne.Loyer + ligne.Facture + ligne.Essence;

    private static decimal Total(IEnumerable<Charge> charges, string typeCharge) =>
        charges.Where(charge => charge.TypeCharge == typeCharge).Sum(charge => charge.Montant);

    private static decimal Arrondir(decimal montant) => Math.Round(montant, 2);

    private static DateTime PremierJourDuMois(DateTime date) => new(date.Year, date.Month, 1);
}
