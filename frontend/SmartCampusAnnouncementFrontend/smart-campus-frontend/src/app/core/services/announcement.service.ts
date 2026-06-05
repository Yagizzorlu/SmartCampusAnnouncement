import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AnnouncementResponse,
  CreateAnnouncementRequest,
  PublishAnnouncementResponse
} from '../models/announcement.model';

@Injectable({ providedIn: 'root' })
export class AnnouncementService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/announcements`;

  getAll(): Observable<AnnouncementResponse[]> {
    return this.http.get<AnnouncementResponse[]>(this.base);
  }

  getById(id: number): Observable<AnnouncementResponse> {
    return this.http.get<AnnouncementResponse>(`${this.base}/${id}`);
  }

  create(req: CreateAnnouncementRequest): Observable<AnnouncementResponse> {
    return this.http.post<AnnouncementResponse>(this.base, req);
  }

  publish(id: number): Observable<PublishAnnouncementResponse> {
    return this.http.post<PublishAnnouncementResponse>(`${this.base}/${id}/publish`, {});
  }

  archive(id: number): Observable<AnnouncementResponse> {
    return this.http.post<AnnouncementResponse>(`${this.base}/${id}/archive`, {});
  }
}
