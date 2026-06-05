import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  NotificationLogResponse,
  NotificationLogSummaryResponse
} from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/notification-logs`;

  getLogs(): Observable<NotificationLogResponse[]> {
    return this.http.get<NotificationLogResponse[]>(this.base);
  }

  getSummary(): Observable<NotificationLogSummaryResponse> {
    return this.http.get<NotificationLogSummaryResponse>(`${this.base}/summary`);
  }
}
