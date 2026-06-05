import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe, KeyValuePipe, CommonModule } from '@angular/common';
import { NotificationService }  from '../../../core/services/notification.service';
import { NotificationLogResponse, NotificationLogSummaryResponse } from '../../../core/models/notification.model';
import { NotificationStatus, NotificationType }   from '../../../core/models/enums';
import { EnumLabelPipe }        from '../../../shared/pipes/enum-label.pipe';
import { StatusBadgeComponent } from '../../../shared/components/status-badge.component';
import { EmptyStateComponent }  from '../../../shared/components/empty-state.component';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [DatePipe, DecimalPipe, KeyValuePipe, CommonModule, EnumLabelPipe, StatusBadgeComponent, EmptyStateComponent],
  templateUrl: './notification-list.component.html'
})
export class NotificationListComponent implements OnInit {
  private svc = inject(NotificationService);

  logs    = signal<NotificationLogResponse[]>([]);
  summary = signal<NotificationLogSummaryResponse | null>(null);

  NotificationStatus = NotificationStatus;
  NotificationType = NotificationType;

  ngOnInit() {
    this.svc.getLogs().subscribe(d => this.logs.set(d));
    this.svc.getSummary().subscribe(d => this.summary.set(d));
  }

  get successRate(): number {
    const s = this.summary();
    if (!s || s.totalLogs === 0) return 0;
    return (s.totalSent / s.totalLogs) * 100;
  }

  getChannelClass(type: NotificationType): string {
    switch (type) {
      case NotificationType.Email: return 'bg-blue-500/10 text-blue-400 border-blue-500/30';
      case NotificationType.Sms:   return 'bg-amber-500/10 text-amber-400 border-amber-500/30';
      case NotificationType.Push:  return 'bg-purple-500/10 text-purple-400 border-purple-500/30';
      default: return 'bg-[#0f172a] text-[#94a3b8] border-[#334155]';
    }
  }
}
