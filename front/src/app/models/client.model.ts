export interface Client {
    idClient: number;
    typeClient: string;
    nom: string;
    contact: string;
    email: string;
    telephone: string;
    adresse: string;
}

export interface CreateClientDto {
    typeClient: string;
    nom: string;
    contact: string;
    email: string;
    telephone: string;
    adresse: string;
}
