export interface ConnexionDto {
    nomUtilisateur: string;
    motDePasse: string;
}

export interface UtilisateurConnecte {
    token: string;
    nomUtilisateur: string;
    nomComplet: string;
    role: string;
}
