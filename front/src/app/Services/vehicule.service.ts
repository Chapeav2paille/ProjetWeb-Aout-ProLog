import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateVehiculeDto, Vehicule } from '../models/vehicule.model';

@Injectable({ providedIn: 'root' })
export class VehiculeService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Vehicule';

  getAll(): Observable<Vehicule[]> {
    return this.http.get<Vehicule[]>(this.apiUrl);
  }
  getById(id: number): Observable<Vehicule> {
    return this.http.get<Vehicule>(`${this.apiUrl}/${id}`);
  }
  create(dto: CreateVehiculeDto): Observable<Vehicule> {
    return this.http.post<Vehicule>(this.apiUrl, dto);
  }
  update(id: number, dto: CreateVehiculeDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
