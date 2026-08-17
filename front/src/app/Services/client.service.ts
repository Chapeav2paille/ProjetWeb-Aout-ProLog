import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client, CreateClientDto } from '../models/client.model';
import { Prestation } from '../models/prestation.model';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Client';

  getAll(): Observable<Client[]> {
    return this.http.get<Client[]>(this.apiUrl);
  }
  getById(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.apiUrl}/${id}`);
  }
  getPrestations(id: number): Observable<Prestation[]> {
    return this.http.get<Prestation[]>(`${this.apiUrl}/${id}/prestations`);
  }
  create(dto: CreateClientDto): Observable<Client> {
    return this.http.post<Client>(this.apiUrl, dto);
  }
  update(id: number, dto: CreateClientDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
