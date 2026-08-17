import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chiffres } from '../models/chiffres.model';

@Injectable({ providedIn: 'root' })
export class ChiffresService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/Chiffres';

  get(): Observable<Chiffres> {
    return this.http.get<Chiffres>(this.apiUrl);
  }
}
