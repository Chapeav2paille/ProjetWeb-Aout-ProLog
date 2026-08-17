import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Historique } from '../models/historique.model';

@Injectable({ providedIn: 'root' })
export class HistoriqueService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Historique';

  getAll(du?: string, au?: string, typeAction?: string): Observable<Historique[]> {
    let params = new HttpParams();
    if (du) params = params.set('du', du);
    if (au) params = params.set('au', au);
    if (typeAction) params = params.set('typeAction', typeAction);
    return this.http.get<Historique[]>(this.apiUrl, { params });
  }
}
