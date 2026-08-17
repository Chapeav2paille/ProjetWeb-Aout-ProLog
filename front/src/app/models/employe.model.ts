export interface Employe {
    idEmploye: number;
    nom: string;
    prenom: string;
    poste: string;
    dateEmbauche: string;
    email: string;
    telephone: string;
    statut: string;
    disponibilite: string;
    numeroPermis: string;
    categoriesPermis: string;
    expirationPermis: string | null;
}

export interface CreateEmployeDto {
    nom: string;
    prenom: string;
    poste: string;
    dateEmbauche: string;
    email: string;
    telephone: string;
    statut: string;
    disponibilite: string;
    numeroPermis: string;
    categoriesPermis: string;
    expirationPermis: string | null;
}
