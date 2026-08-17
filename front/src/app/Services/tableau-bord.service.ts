import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TableauBord } from '../models/tableau-bord.model';

@Injectable({ providedIn: 'root' })
export class TableauBordService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5287/api/TableauBord';

  get(): Observable<TableauBord> {
    return this.http.get<TableauBord>(this.apiUrl);
  }
}
