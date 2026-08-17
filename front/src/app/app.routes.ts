import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { Connexion } from './components/connexion/connexion';
import { TableauBordComponent } from './components/tableau-bord/tableau-bord';
import { Prestations } from './components/prestations/prestations';
import { Clients } from './components/clients/clients';
import { FicheClient } from './components/fiche-client/fiche-client';
import { Flotte } from './components/flotte/flotte';
import { RessourcesHumaines } from './components/ressources-humaines/ressources-humaines';
import { Chiffres } from './components/chiffres/chiffres';
import { HistoriqueComponent } from './components/historique/historique';

export const routes: Routes = [
  { path: 'connexion', component: Connexion },
  { path: '', redirectTo: 'tableau-bord', pathMatch: 'full' },
  { path: 'tableau-bord', component: TableauBordComponent, canActivate: [authGuard] },
  { path: 'prestations', component: Prestations, canActivate: [authGuard] },
  { path: 'clients', component: Clients, canActivate: [authGuard] },
  { path: 'clients/:id', component: FicheClient, canActivate: [authGuard] },
  { path: 'flotte', component: Flotte, canActivate: [authGuard] },
  { path: 'ressources-humaines', component: RessourcesHumaines, canActivate: [authGuard] },
  { path: 'chiffres', component: Chiffres, canActivate: [authGuard] },
  { path: 'historique', component: HistoriqueComponent, canActivate: [authGuard] },
];
