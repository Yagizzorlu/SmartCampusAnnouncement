import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserResponse, CreateUserRequest } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/users`;

  getAll(): Observable<UserResponse[]> {
    return this.http.get<UserResponse[]>(this.base);
  }

  getById(id: number): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.base}/${id}`);
  }

  create(req: CreateUserRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(this.base, req);
  }

  // Backend DELETE /api/users/{id} kullanıcıyı pasifleştirir, fiziksel silmez
  deactivate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
