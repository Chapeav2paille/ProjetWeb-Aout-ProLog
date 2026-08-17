import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreatePrestationDto, Prestation } from '../models/prestation.model';

@Injectable({ providedIn: 'root' })
export class PrestationService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Prestation';

  getAll(statut?: string): Observable<Prestation[]> {
    const params = statut ? new HttpParams().set('statut', statut) : undefined;
    return this.http.get<Prestation[]>(this.apiUrl, { params });
  }
  getById(id: number): Observable<Prestation> {
    return this.http.get<Prestation>(`${this.apiUrl}/${id}`);
  }
  create(dto: CreatePrestationDto): Observable<Prestation> {
    return this.http.post<Prestation>(this.apiUrl, dto);
  }
  update(id: number, dto: CreatePrestationDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }
  changerStatut(id: number, statut: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/statut`, { statut });
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
