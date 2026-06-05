import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AnnouncementService } from '../../../core/services/announcement.service';
import {
  AnnouncementResponse,
  CreateAnnouncementRequest,
  PublishAnnouncementResponse
} from '../../../core/models/announcement.model';
import {
  AnnouncementType, TargetAudience, AnnouncementPriority, AnnouncementStatus
} from '../../../core/models/enums';
import { EnumLabelPipe }        from '../../../shared/pipes/enum-label.pipe';
import { StatusBadgeComponent } from '../../../shared/components/status-badge.component';
import { EmptyStateComponent }  from '../../../shared/components/empty-state.component';

@Component({
  selector: 'app-announcement-list',
  standalone: true,
  imports: [FormsModule, DatePipe, EnumLabelPipe, StatusBadgeComponent, EmptyStateComponent],
  templateUrl: './announcement-list.component.html'
})
export class AnnouncementListComponent implements OnInit {
  private svc = inject(AnnouncementService);

  announcements = signal<AnnouncementResponse[]>([]);
  showForm      = signal(false);
  publishResult = signal<PublishAnnouncementResponse | null>(null);
  error         = signal<string | null>(null);
  success       = signal<string | null>(null);

  AnnouncementType     = AnnouncementType;
  TargetAudience       = TargetAudience;
  AnnouncementPriority = AnnouncementPriority;
  AnnouncementStatus   = AnnouncementStatus;

  form: CreateAnnouncementRequest = this.emptyForm();

  ngOnInit() { this.load(); }

  load() {
    this.svc.getAll().subscribe(d => this.announcements.set(d));
  }

  submit() {
    if (!this.form.title.trim() || !this.form.content.trim()) {
      this.error.set('Başlık ve içerik zorunludur.');
      return;
    }
    this.svc.create(this.form).subscribe({
      next: () => {
        this.success.set('Duyuru taslak olarak oluşturuldu.');
        this.showForm.set(false);
        this.form = this.emptyForm();
        this.error.set(null);
        this.load();
        setTimeout(() => this.success.set(null), 3000);
      },
      error: () => this.error.set('Duyuru oluşturulamadı.')
    });
  }

  publish(id: number) {
    this.svc.publish(id).subscribe({
      next: (res) => {
        this.publishResult.set(res);
        this.load();
        setTimeout(() => this.publishResult.set(null), 6000);
      },
      error: () => this.error.set('Yayınlama başarısız.')
    });
  }

  archive(id: number) {
    this.svc.archive(id).subscribe({ next: () => this.load() });
  }

  emptyForm(): CreateAnnouncementRequest {
    return {
      title: '',
      content: '',
      announcementType: AnnouncementType.Exam,
      targetAudience: TargetAudience.All,
      priority: null,
      expiresAt: null,
      createdBy: 'Admin'
    };
  }
}
