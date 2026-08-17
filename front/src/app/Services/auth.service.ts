import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { ConnexionDto, UtilisateurConnecte } from '../models/auth.model';

const CLE_TOKEN = 'promanlog_token';
const CLE_UTILISATEUR = 'promanlog_utilisateur';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Auth';

  private token = signal<string | null>(localStorage.getItem(CLE_TOKEN));
  private nomComplet = signal<string>(localStorage.getItem(CLE_UTILISATEUR) ?? '');

  estConnecte = computed(() => this.token() !== null);
  utilisateur = this.nomComplet.asReadonly();

  connexion(dto: ConnexionDto): Observable<UtilisateurConnecte> {
    return this.http.post<UtilisateurConnecte>(`${this.apiUrl}/connexion`, dto).pipe(
      tap((utilisateur) => {
        localStorage.setItem(CLE_TOKEN, utilisateur.token);
        localStorage.setItem(CLE_UTILISATEUR, utilisateur.nomComplet);
        this.token.set(utilisateur.token);
        this.nomComplet.set(utilisateur.nomComplet);
      })
    );
  }

  deconnexion(): void {
    localStorage.removeItem(CLE_TOKEN);
    localStorage.removeItem(CLE_UTILISATEUR);
    this.token.set(null);
    this.nomComplet.set('');
  }

  getToken(): string | null {
    return this.token();
  }
}
