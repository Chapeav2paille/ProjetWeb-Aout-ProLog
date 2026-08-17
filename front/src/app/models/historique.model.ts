export interface Historique {
    idHistorique: number;
    dateAction: string;
    typeAction: string;
    typeEntite: string;
    idEntite: number | null;
    description: string;
}
