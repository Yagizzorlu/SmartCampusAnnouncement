import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe, CommonModule } from '@angular/common';
import { UserService }        from '../../../core/services/user.service';
import { UserResponse, CreateUserRequest } from '../../../core/models/user.model';
import { UserType, NotificationType, UserTypeLabel, NotificationTypeLabel } from '../../../core/models/enums';
import { EnumLabelPipe }          from '../../../shared/pipes/enum-label.pipe';
import { EmptyStateComponent }    from '../../../shared/components/empty-state.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, DatePipe, CommonModule, EnumLabelPipe, EmptyStateComponent, ConfirmDialogComponent],
  templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit {
  private svc = inject(UserService);

  users        = signal<UserResponse[]>([]);
  showForm     = signal(false);
  deactivateTarget = signal<number | null>(null);
  error        = signal<string | null>(null);
  success      = signal<string | null>(null);

  UserType             = UserType;
  NotificationType     = NotificationType;
  UserTypeLabel        = UserTypeLabel;
  NotificationTypeLabel = NotificationTypeLabel;

  form: CreateUserRequest = this.emptyForm();

  ngOnInit() { this.load(); }

  load() {
    this.svc.getAll().subscribe(d => this.users.set(d));
  }

  toggleChannel(type: NotificationType) {
    const current = this.form.notificationTypes;
    this.form.notificationTypes = current.includes(type)
      ? current.filter(t => t !== type)
      : [...current, type];
  }

  isChannelSelected(type: NotificationType): boolean {
    return this.form.notificationTypes.includes(type);
  }

  submit() {
    if (!this.form.fullName.trim() || !this.form.email.trim()) {
      this.error.set('Ad Soyad ve E-posta zorunludur.');
      return;
    }
    if (this.form.notificationTypes.length === 0) {
      this.error.set('En az bir bildirim kanalı seçin.');
      return;
    }
    if (this.isChannelSelected(NotificationType.Sms) && !this.form.phoneNumber?.trim()) {
      this.error.set('SMS kanalı seçiliyse telefon numarası zorunludur.');
      return;
    }
    this.svc.create(this.form).subscribe({
      next: () => {
        this.success.set('Kullanıcı oluşturuldu ve bildirim tercihleri kaydedildi.');
        this.showForm.set(false);
        this.form = this.emptyForm();
        this.error.set(null);
        this.load();
        setTimeout(() => this.success.set(null), 3000);
      },
      error: () => this.error.set('Kullanıcı oluşturulamadı.')
    });
  }

  confirmDeactivate(id: number) { this.deactivateTarget.set(id); }

  executeDeactivate() {
    const id = this.deactivateTarget();
    if (id === null) return;
    this.svc.deactivate(id).subscribe({
      next: () => {
        this.deactivateTarget.set(null);
        this.load();
      }
    });
  }

  emptyForm(): CreateUserRequest {
    return {
      fullName: '',
      email: '',
      phoneNumber: null,
      userType: UserType.Student,
      notificationTypes: [NotificationType.Email]
    };
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
