export interface LigneChiffres {
    periode: string;
    chiffreAffaire: number;
    salaire: number;
    frais: number;
    loyer: number;
    facture: number;
    essence: number;
    totalCharges: number;
    tvaEncaissee: number;
    tvaPayee: number;
    tvaAReverser: number;
    resultatAvantImpot: number;
}

export interface Chiffres {
    tauxTva: number;
    lignes: LigneChiffres[];
    total: LigneChiffres;
}
