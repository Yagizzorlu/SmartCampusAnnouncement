import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe, KeyValuePipe } from '@angular/common';  // KeyValuePipe zorunlu
import { AnnouncementService }  from '../../core/services/announcement.service';
import { UserService }          from '../../core/services/user.service';
import { NotificationService }  from '../../core/services/notification.service';
import { AnnouncementResponse } from '../../core/models/announcement.model';
import { UserResponse }         from '../../core/models/user.model';
import { NotificationLogSummaryResponse } from '../../core/models/notification.model';
import { AnnouncementStatus }   from '../../core/models/enums';
import { StatusBadgeComponent } from '../../shared/components/status-badge.component';
import { EnumLabelPipe }        from '../../shared/pipes/enum-label.pipe';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, DatePipe, KeyValuePipe, StatusBadgeComponent, EnumLabelPipe],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  private announcementSvc = inject(AnnouncementService);
  private userSvc         = inject(UserService);
  private notificationSvc = inject(NotificationService);

  announcements = signal<AnnouncementResponse[]>([]);
  users         = signal<UserResponse[]>([]);
  summary       = signal<NotificationLogSummaryResponse | null>(null);

  AnnouncementStatus = AnnouncementStatus;

  get publishedCount() {
    return this.announcements().filter(a => a.status === AnnouncementStatus.Published).length;
  }

  get recentAnnouncements() {
    return [...this.announcements()]
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 5);
  }

  ngOnInit() {
    this.announcementSvc.getAll().subscribe(d => this.announcements.set(d));
    this.userSvc.getAll().subscribe(d => this.users.set(d));
    this.notificationSvc.getSummary().subscribe(d => this.summary.set(d));
  }
}
