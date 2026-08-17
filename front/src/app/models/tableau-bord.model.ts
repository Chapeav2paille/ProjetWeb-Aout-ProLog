import { Historique } from './historique.model';
import { Prestation } from './prestation.model';
import { Vehicule } from './vehicule.model';

export interface Alerte {
    typeAlerte: string;
    message: string;
    dateEcheance: string;
    gravite: string;
}

export interface PointChiffreAffaire {
    periode: string;
    montant: number;
}

export interface TableauBord {
    prestationsEnCours: number;
    prestationsTerminees: number;
    vehiculesDisponibles: number;
    vehiculesEnMission: number;
    vehiculesEnMaintenance: number;
    chiffreAffaireDuMois: number;
    prestationsDuMois: Prestation[];
    vehicules: Vehicule[];
    evolutionChiffreAffaire: PointChiffreAffaire[];
    alertes: Alerte[];
    dernieresActivites: Historique[];
}
