export interface Prestation {
    idPrestation: number;
    idClient: number;
    idVehicule: number | null;
    idEmploye: number | null;
    adresseDepart: string;
    adresseArrivee: string;
    dateHeure: string;
    typeService: string;
    prix: number;
    statut: string;
    nomClient: string;
    immatriculationVehicule: string;
    nomEmploye: string;
}

export interface CreatePrestationDto {
    idClient: number;
    idVehicule: number | null;
    idEmploye: number | null;
    adresseDepart: string;
    adresseArrivee: string;
    dateHeure: string;
    typeService: string;
    prix: number;
    statut: string;
}
