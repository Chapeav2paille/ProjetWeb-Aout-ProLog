export interface Vehicule {
    idVehicule: number;
    immatriculation: string;
    typeVehicule: string;
    capaciteKg: number;
    kilometrage: number;
    statut: string;
    dernierEntretien: string | null;
    prochainEntretien: string | null;
    prochainControleTechnique: string | null;
    finAssurance: string | null;
}

export interface CreateVehiculeDto {
    immatriculation: string;
    typeVehicule: string;
    capaciteKg: number;
    kilometrage: number;
    statut: string;
    dernierEntretien: string | null;
    prochainEntretien: string | null;
    prochainControleTechnique: string | null;
    finAssurance: string | null;
}
