import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateEmployeDto, Employe } from '../models/employe.model';

@Injectable({ providedIn: 'root' })
export class EmployeService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Employe';

  getAll(): Observable<Employe[]> {
    return this.http.get<Employe[]>(this.apiUrl);
  }
  getById(id: number): Observable<Employe> {
    return this.http.get<Employe>(`${this.apiUrl}/${id}`);
  }
  create(dto: CreateEmployeDto): Observable<Employe> {
    return this.http.post<Employe>(this.apiUrl, dto);
  }
  update(id: number, dto: CreateEmployeDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
