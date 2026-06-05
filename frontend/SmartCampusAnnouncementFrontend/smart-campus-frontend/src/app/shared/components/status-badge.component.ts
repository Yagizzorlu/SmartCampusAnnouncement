import { Component, Input } from '@angular/core';
import { NgClass } from '@angular/common';
import {
  AnnouncementStatus, AnnouncementPriority, NotificationStatus,
  AnnouncementStatusLabel, AnnouncementPriorityLabel, NotificationStatusLabel
} from '../../core/models/enums';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [NgClass],
  template: `
    <span [ngClass]="classes"
          class="inline-flex items-center px-2 py-0.5 rounded text-xs font-mono font-medium border">
      {{ label }}
    </span>
  `
})
export class StatusBadgeComponent {
  @Input() type: 'status' | 'priority' | 'notif-status' = 'status';
  @Input() value!: number;

  get label(): string {
    if (this.type === 'status')       return AnnouncementStatusLabel[this.value as AnnouncementStatus] ?? '—';
    if (this.type === 'priority')     return AnnouncementPriorityLabel[this.value as AnnouncementPriority] ?? '—';
    if (this.type === 'notif-status') return NotificationStatusLabel[this.value as NotificationStatus] ?? '—';
    return '—';
  }

  get classes(): Record<string, boolean> {
    if (this.type === 'status') {
      return {
        'text-yellow-400 bg-yellow-400/10 border-yellow-400/30': this.value === AnnouncementStatus.Draft,
        'text-green-400  bg-green-400/10  border-green-400/30':  this.value === AnnouncementStatus.Published,
        'text-slate-400  bg-slate-400/10  border-slate-400/30':  this.value === AnnouncementStatus.Archived,
      };
    }
    if (this.type === 'priority') {
      return {
        'text-slate-400  bg-slate-400/10  border-slate-400/30':  this.value === AnnouncementPriority.Low,
        'text-blue-400   bg-blue-400/10   border-blue-400/30':   this.value === AnnouncementPriority.Normal,
        'text-orange-400 bg-orange-400/10 border-orange-400/30': this.value === AnnouncementPriority.High,
        'text-red-400    bg-red-400/10    border-red-400/30':    this.value === AnnouncementPriority.Urgent,
      };
    }
    if (this.type === 'notif-status') {
      return {
        'text-green-400 bg-green-400/10 border-green-400/30': this.value === NotificationStatus.Sent,
        'text-red-400   bg-red-400/10   border-red-400/30':   this.value === NotificationStatus.Failed,
      };
    }
    return {};
  }
}
