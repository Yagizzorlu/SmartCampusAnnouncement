# Smart Campus Announcement — Angular Frontend Kılavuzu

> Backend: ASP.NET Core · Varsayılan port: `http://localhost:5238`
> Frontend: Angular 17 Standalone · Port: `http://localhost:4200`
> Stil: TailwindCSS (Angular Material veya Lucide kurma)

---

## 0. Backend Port Notu

`launchSettings.json` incelendi — backend default profil `http://localhost:5238` üzerinde çalışıyor.

**Demo/video için backend'i 5000'de sabitlemek istersen:**
```powershell
dotnet run --project SmartCampusAnnouncement.WebAPI --urls="http://localhost:5000"
```

**Yoksa environment'ı 5238 olarak bırak (daha pratik):**
```ts
// src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5238/api'
};
```

Bu kılavuzda `5238` kullanılmaktadır. Değiştirmek istersen sadece bu dosyayı güncelle.

---

## 1. Proje Kurulumu

```bash
ng new smart-campus-frontend --routing=true --style=scss --standalone=true --skip-git=true

cd smart-campus-frontend

# TailwindCSS (SADECE bunları kur — Material, Lucide, date-fns kurma)
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init
```

### `tailwind.config.js`
```js
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        campus: {
          bg:      '#0f172a',
          surface: '#1e293b',
          border:  '#334155',
          accent:  '#3b82f6',
          accentHv:'#2563eb',
          text:    '#f1f5f9',
          muted:   '#94a3b8',
        }
      },
      fontFamily: {
        mono: ['"IBM Plex Mono"', 'monospace'],
      }
    }
  },
  plugins: []
}
```

### `src/styles.scss`
```scss
@import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Mono:wght@400;500;600&display=swap');

@tailwind base;
@tailwind components;
@tailwind utilities;

* { box-sizing: border-box; }

body {
  background-color: #0f172a;
  color: #f1f5f9;
  font-family: 'IBM Plex Mono', monospace;
  margin: 0;
}

::-webkit-scrollbar { width: 6px; }
::-webkit-scrollbar-track { background: #1e293b; }
::-webkit-scrollbar-thumb { background: #334155; border-radius: 3px; }
```

### `src/environments/environment.ts`
```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5238/api'
};
```

---

## 2. Klasör Yapısı

```
src/app/
├── core/
│   ├── models/
│   │   ├── enums.ts
│   │   ├── user.model.ts
│   │   ├── announcement.model.ts
│   │   ├── notification.model.ts
│   │   └── demo.model.ts
│   └── services/
│       ├── user.service.ts
│       ├── announcement.service.ts
│       ├── notification.service.ts
│       ├── demo.service.ts
│       └── loading.service.ts
│
├── shared/
│   ├── components/
│   │   ├── status-badge.component.ts
│   │   ├── empty-state.component.ts
│   │   └── confirm-dialog.component.ts
│   └── pipes/
│       └── enum-label.pipe.ts
│
├── layout/
│   ├── shell/
│   │   ├── shell.component.ts
│   │   └── shell.component.html
│   └── sidebar/
│       ├── sidebar.component.ts
│       └── sidebar.component.html
│
└── features/
    ├── dashboard/
    │   ├── dashboard.component.ts
    │   └── dashboard.component.html
    ├── demo/
    │   ├── demo.component.ts
    │   └── demo.component.html
    ├── notifications/
    │   └── notification-list/
    │       ├── notification-list.component.ts
    │       └── notification-list.component.html
    ├── users/
    │   └── user-list/
    │       ├── user-list.component.ts
    │       └── user-list.component.html
    └── announcements/
        └── announcement-list/
            ├── announcement-list.component.ts
            └── announcement-list.component.html
```

---

## 3. Core Models

### `src/app/core/models/enums.ts`
```ts
export enum UserType {
  Student = 1,
  Teacher = 2
}

export enum AnnouncementType {
  Exam     = 1,
  Event    = 2,
  FoodMenu = 3,
  Library  = 4
}

export enum TargetAudience {
  Students = 1,
  Teachers = 2,
  All      = 3
}

export enum NotificationType {
  Email = 1,
  Sms   = 2,
  Push  = 3
}

export enum AnnouncementStatus {
  Draft     = 1,
  Published = 2,
  Archived  = 3
}

export enum AnnouncementPriority {
  Low    = 1,
  Normal = 2,
  High   = 3,
  Urgent = 4
}

export enum NotificationStatus {
  Sent   = 1,
  Failed = 2
}

export const UserTypeLabel: Record<UserType, string> = {
  [UserType.Student]: 'Öğrenci',
  [UserType.Teacher]: 'Öğretmen'
};

export const AnnouncementTypeLabel: Record<AnnouncementType, string> = {
  [AnnouncementType.Exam]:     'Sınav',
  [AnnouncementType.Event]:    'Etkinlik',
  [AnnouncementType.FoodMenu]: 'Yemekhane',
  [AnnouncementType.Library]:  'Kütüphane'
};

export const TargetAudienceLabel: Record<TargetAudience, string> = {
  [TargetAudience.Students]: 'Öğrenciler',
  [TargetAudience.Teachers]: 'Öğretmenler',
  [TargetAudience.All]:      'Herkes'
};

export const NotificationTypeLabel: Record<NotificationType, string> = {
  [NotificationType.Email]: 'E-posta',
  [NotificationType.Sms]:   'SMS',
  [NotificationType.Push]:  'Mobil Bildirim'
};

export const AnnouncementStatusLabel: Record<AnnouncementStatus, string> = {
  [AnnouncementStatus.Draft]:     'Taslak',
  [AnnouncementStatus.Published]: 'Yayında',
  [AnnouncementStatus.Archived]:  'Arşivlendi'
};

export const AnnouncementPriorityLabel: Record<AnnouncementPriority, string> = {
  [AnnouncementPriority.Low]:    'Düşük',
  [AnnouncementPriority.Normal]: 'Normal',
  [AnnouncementPriority.High]:   'Yüksek',
  [AnnouncementPriority.Urgent]: 'Acil'
};

export const NotificationStatusLabel: Record<NotificationStatus, string> = {
  [NotificationStatus.Sent]:   'Gönderildi',
  [NotificationStatus.Failed]: 'Başarısız'
};
```

---

### `src/app/core/models/user.model.ts`
```ts
import { UserType, NotificationType } from './enums';

export interface UserResponse {
  id: number;
  fullName: string;
  email: string;
  phoneNumber: string | null;
  userType: UserType;
  isActive: boolean;
  notificationTypes: NotificationType[];
  createdAt: string;
}

export interface CreateUserRequest {
  fullName: string;
  email: string;
  phoneNumber: string | null;
  userType: UserType;
  notificationTypes: NotificationType[];
}
```

---

### `src/app/core/models/announcement.model.ts`
```ts
import { AnnouncementType, TargetAudience, AnnouncementStatus, AnnouncementPriority } from './enums';

export interface AnnouncementResponse {
  id: number;
  title: string;
  content: string;
  announcementType: AnnouncementType;
  targetAudience: TargetAudience;
  status: AnnouncementStatus;
  priority: AnnouncementPriority;
  publishedAt: string | null;
  expiresAt: string | null;
  createdBy: string;
  createdAt: string;
}

export interface CreateAnnouncementRequest {
  title: string;
  content: string;
  announcementType: AnnouncementType;
  targetAudience: TargetAudience;
  priority: AnnouncementPriority | null;
  expiresAt: string | null;
  createdBy: string | null;
}

export interface PublishAnnouncementResponse {
  announcementId: number;
  totalRecipients: number;
  sentCount: number;
  failedCount: number;
  publishedAt: string;
}
```

---

### `src/app/core/models/notification.model.ts`
```ts
import { UserType, AnnouncementType, NotificationType, NotificationStatus } from './enums';

export interface NotificationLogResponse {
  id: number;
  appUserId: number;
  recipientFullName: string;
  recipientUserType: UserType;
  announcementId: number;
  announcementTitle: string;
  announcementType: AnnouncementType;
  notificationType: NotificationType;
  status: NotificationStatus;
  message: string;
  errorMessage: string | null;
  sentAt: string;
}

export interface NotificationLogSummaryResponse {
  totalLogs: number;
  totalSent: number;
  totalFailed: number;
  byChannel: Record<string, number>;
  byAnnouncementType: Record<string, number>;
}
```

---

### `src/app/core/models/demo.model.ts`
```ts
import { UserResponse } from './user.model';
import { AnnouncementResponse, PublishAnnouncementResponse } from './announcement.model';
import { NotificationLogResponse } from './notification.model';

export interface DemoPatterns {
  factory: string;
  observer: string;
  singleton: string;
}

export interface DemoScenarioResponse {
  scenario: string;
  patterns: DemoPatterns;
  usersCreated: UserResponse[];
  announcementCreated: AnnouncementResponse;
  publishResult: PublishAnnouncementResponse;
  notificationLogs: NotificationLogResponse[];
}
```

---

## 4. Core Services

### `src/app/core/services/user.service.ts`
```ts
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
```

---

### `src/app/core/services/announcement.service.ts`
```ts
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
```

---

### `src/app/core/services/notification.service.ts`
```ts
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
```

---

### `src/app/core/services/demo.service.ts`
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DemoScenarioResponse } from '../models/demo.model';

@Injectable({ providedIn: 'root' })
export class DemoService {
  private http = inject(HttpClient);

  runScenario(): Observable<DemoScenarioResponse> {
    return this.http.post<DemoScenarioResponse>(
      `${environment.apiUrl}/demo/run-scenario`, {}
    );
  }
}
```

---

### `src/app/core/services/loading.service.ts`
```ts
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  loading = signal(false);
  start() { this.loading.set(true); }
  stop()  { this.loading.set(false); }
}
```

---

## 5. HTTP Interceptor

### `src/app/core/interceptors/loading.interceptor.ts`
```ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingSvc = inject(LoadingService);
  loadingSvc.start();
  return next(req).pipe(finalize(() => loadingSvc.stop()));
};
```

---

## 6. `app.config.ts`

```ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([loadingInterceptor]))
  ]
};
```

---

## 7. `app.routes.ts`

```ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layout/shell/shell.component').then(m => m.ShellComponent),
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'users',
        loadComponent: () =>
          import('./features/users/user-list/user-list.component').then(m => m.UserListComponent)
      },
      {
        path: 'announcements',
        loadComponent: () =>
          import('./features/announcements/announcement-list/announcement-list.component')
            .then(m => m.AnnouncementListComponent)
      },
      {
        path: 'notifications',
        loadComponent: () =>
          import('./features/notifications/notification-list/notification-list.component')
            .then(m => m.NotificationListComponent)
      },
      {
        path: 'demo',
        loadComponent: () =>
          import('./features/demo/demo.component').then(m => m.DemoComponent)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
```

---

## 8. `app.component.ts`

```ts
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `<router-outlet />`
})
export class AppComponent {}
```

---

## 9. Layout

### `src/app/layout/shell/shell.component.ts`
```ts
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LoadingService } from '../../core/services/loading.service';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: './shell.component.html'
})
export class ShellComponent {
  loading = inject(LoadingService);
}
```

### `src/app/layout/shell/shell.component.html`
```html
<div class="flex h-screen bg-[#0f172a] overflow-hidden">
  <app-sidebar class="flex-shrink-0" />

  <main class="flex-1 overflow-y-auto">
    <header class="sticky top-0 z-10 border-b border-[#334155] bg-[#0f172a]/90 backdrop-blur px-6 py-4 flex items-center justify-between">
      <span class="text-sm font-mono text-[#94a3b8]">Smart Campus — Duyuru Yönetimi</span>
      @if (loading.loading()) {
        <div class="flex items-center gap-2">
          <div class="w-3 h-3 border border-[#3b82f6] border-t-transparent rounded-full animate-spin"></div>
          <span class="text-[10px] font-mono text-[#94a3b8]">Yükleniyor...</span>
        </div>
      }
    </header>

    <div class="p-6">
      <router-outlet />
    </div>
  </main>
</div>
```

---

### `src/app/layout/sidebar/sidebar.component.ts`
```ts
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  path: string;
  label: string;
  icon: string;
  exact?: boolean;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  navItems: NavItem[] = [
    { path: '/dashboard',     label: 'Dashboard',    icon: '⬡', exact: true },
    { path: '/users',         label: 'Kullanıcılar', icon: '◈' },
    { path: '/announcements', label: 'Duyurular',    icon: '◉' },
    { path: '/notifications', label: 'Bildirimler',  icon: '◎' },
    { path: '/demo',          label: 'Demo Senaryo', icon: '▶' },
  ];
}
```

### `src/app/layout/sidebar/sidebar.component.html`
```html
<nav class="w-56 h-full bg-[#0f172a] border-r border-[#334155] flex flex-col py-6 px-3">
  <div class="px-3 mb-8">
    <span class="font-mono text-[#3b82f6] font-semibold text-sm tracking-widest uppercase">SmartCampus</span>
    <p class="text-[10px] text-[#475569] mt-0.5 font-mono">BİL 3204 Final</p>
  </div>

  <ul class="space-y-1 flex-1">
    @for (item of navItems; track item.path) {
      <li>
        <a
          [routerLink]="item.path"
          routerLinkActive="bg-[#1e293b] text-[#f1f5f9] border-l-2 border-[#3b82f6]"
          [routerLinkActiveOptions]="{ exact: item.exact ?? false }"
          class="flex items-center gap-3 px-3 py-2.5 text-[#94a3b8] hover:text-[#f1f5f9] hover:bg-[#1e293b] transition-colors text-sm font-mono border-l-2 border-transparent rounded-r"
        >
          <span>{{ item.icon }}</span>
          {{ item.label }}
        </a>
      </li>
    }
  </ul>

  <div class="px-3 pt-4 border-t border-[#334155]">
    <p class="text-[10px] text-[#475569] font-mono">Observer · Factory · Singleton</p>
  </div>
</nav>
```

---

## 10. Shared Components

### `src/app/shared/components/status-badge.component.ts`
```ts
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
```

---

### `src/app/shared/components/empty-state.component.ts`
```ts
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  template: `
    <div class="flex flex-col items-center justify-center py-16 text-center">
      <div class="text-4xl mb-4 opacity-30">{{ icon }}</div>
      <p class="text-[#94a3b8] font-mono text-sm">{{ message }}</p>
    </div>
  `
})
export class EmptyStateComponent {
  @Input() icon = '◎';
  @Input() message = 'Kayıt bulunamadı.';
}
```

---

### `src/app/shared/components/confirm-dialog.component.ts`
```ts
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  template: `
    @if (open) {
      <div class="fixed inset-0 z-50 flex items-center justify-center bg-black/60">
        <div class="bg-[#1e293b] border border-[#334155] rounded p-6 w-80 shadow-xl">
          <p class="text-[#f1f5f9] font-mono text-sm mb-6">{{ message }}</p>
          <div class="flex gap-3 justify-end">
            <button (click)="cancel.emit()"
              class="px-4 py-2 text-xs font-mono text-[#94a3b8] border border-[#334155] rounded hover:bg-[#334155] transition-colors">
              İptal
            </button>
            <button (click)="confirm.emit()"
              class="px-4 py-2 text-xs font-mono bg-[#dc2626] text-white rounded hover:bg-red-700 transition-colors">
              Onayla
            </button>
          </div>
        </div>
      </div>
    }
  `
})
export class ConfirmDialogComponent {
  @Input() open = false;
  @Input() message = 'Bu işlemi onaylıyor musunuz?';
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel  = new EventEmitter<void>();
}
```

---

## 11. Shared Pipe

### `src/app/shared/pipes/enum-label.pipe.ts`
```ts
import { Pipe, PipeTransform } from '@angular/core';
import {
  UserType, AnnouncementType, TargetAudience, NotificationType,
  UserTypeLabel, AnnouncementTypeLabel, TargetAudienceLabel, NotificationTypeLabel
} from '../../core/models/enums';

type LabelMap = 'userType' | 'announcementType' | 'targetAudience' | 'notificationType';

@Pipe({ name: 'enumLabel', standalone: true, pure: true })
export class EnumLabelPipe implements PipeTransform {
  transform(value: number, map: LabelMap): string {
    switch (map) {
      case 'userType':         return UserTypeLabel[value as UserType]                 ?? String(value);
      case 'announcementType': return AnnouncementTypeLabel[value as AnnouncementType] ?? String(value);
      case 'targetAudience':   return TargetAudienceLabel[value as TargetAudience]     ?? String(value);
      case 'notificationType': return NotificationTypeLabel[value as NotificationType] ?? String(value);
    }
  }
}
```

---

## 12. AŞAMA 1 — Dashboard

> Import notu: `dashboard.component.ts` → `src/app/features/dashboard/` içinde.
> Bu path'ten `core` ve `shared`'e giden relative path: `../../core/...` ve `../../shared/...`

### `src/app/features/dashboard/dashboard.component.ts`
```ts
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
```

### `src/app/features/dashboard/dashboard.component.html`
```html
<div class="space-y-6">
  <div>
    <h2 class="text-lg font-mono font-semibold text-[#f1f5f9]">Dashboard</h2>
    <p class="text-xs text-[#94a3b8] mt-1">Sisteme genel bakış</p>
  </div>

  <!-- Stats -->
  <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
    <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
      <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-2">Toplam Duyuru</p>
      <p class="text-3xl font-mono font-semibold text-[#f1f5f9]">{{ announcements().length }}</p>
    </div>
    <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
      <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-2">Yayında</p>
      <p class="text-3xl font-mono font-semibold text-green-400">{{ publishedCount }}</p>
    </div>
    <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
      <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-2">Kullanıcılar</p>
      <p class="text-3xl font-mono font-semibold text-[#f1f5f9]">{{ users().length }}</p>
    </div>
    <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
      <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-2">Bildirim Gönderildi</p>
      <p class="text-3xl font-mono font-semibold text-[#3b82f6]">{{ summary()?.totalSent ?? 0 }}</p>
    </div>
  </div>

  <div class="grid grid-cols-1 lg:grid-cols-2 gap-4">
    <!-- Son Duyurular -->
    <div class="bg-[#1e293b] border border-[#334155] rounded">
      <div class="px-4 py-3 border-b border-[#334155] flex items-center justify-between">
        <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest">Son Duyurular</h3>
        <a routerLink="/announcements" class="text-[10px] font-mono text-[#3b82f6] hover:underline">Tümü →</a>
      </div>
      <div class="divide-y divide-[#334155]">
        @for (a of recentAnnouncements; track a.id) {
          <div class="px-4 py-3 flex items-center justify-between">
            <div class="min-w-0">
              <p class="text-sm text-[#f1f5f9] truncate">{{ a.title }}</p>
              <p class="text-[10px] text-[#94a3b8] font-mono mt-0.5">
                {{ a.announcementType | enumLabel:'announcementType' }} · {{ a.createdAt | date:'dd MMM, HH:mm' }}
              </p>
            </div>
            <app-status-badge type="status" [value]="a.status" />
          </div>
        } @empty {
          <p class="px-4 py-8 text-xs text-[#94a3b8] font-mono text-center">Henüz duyuru yok.</p>
        }
      </div>
    </div>

    <!-- Kanala Göre Bildirimler -->
    <div class="bg-[#1e293b] border border-[#334155] rounded">
      <div class="px-4 py-3 border-b border-[#334155]">
        <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest">Kanala Göre Bildirimler</h3>
      </div>
      <div class="p-4 space-y-3">
        @if (summary(); as s) {
          @for (entry of s.byChannel | keyvalue; track entry.key) {
            <div class="flex items-center gap-3">
              <span class="text-xs font-mono text-[#94a3b8] w-28 shrink-0">{{ entry.key }}</span>
              <div class="flex-1 h-2 bg-[#334155] rounded-full overflow-hidden">
                <div class="h-full bg-[#3b82f6] rounded-full"
                     [style.width.%]="s.totalLogs > 0 ? (+entry.value / s.totalLogs) * 100 : 0">
                </div>
              </div>
              <span class="text-xs font-mono text-[#f1f5f9] w-6 text-right">{{ entry.value }}</span>
            </div>
          }
        } @else {
          <p class="text-xs text-[#94a3b8] font-mono text-center py-4">Veri yok.</p>
        }
      </div>
    </div>
  </div>
</div>
```

---

## 13. AŞAMA 1 — Demo Sayfası

> `demo.component.ts` → `src/app/features/demo/` içinde.
> Relative path: `../../core/...` ve `../../shared/...`

### `src/app/features/demo/demo.component.ts`
```ts
import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { DemoService }          from '../../core/services/demo.service';
import { DemoScenarioResponse } from '../../core/models/demo.model';
import { EnumLabelPipe }        from '../../shared/pipes/enum-label.pipe';
import { StatusBadgeComponent } from '../../shared/components/status-badge.component';

@Component({
  selector: 'app-demo',
  standalone: true,
  imports: [DatePipe, EnumLabelPipe, StatusBadgeComponent],
  templateUrl: './demo.component.html'
})
export class DemoComponent {
  private svc = inject(DemoService);

  result  = signal<DemoScenarioResponse | null>(null);
  running = signal(false);
  error   = signal<string | null>(null);

  run() {
    this.running.set(true);
    this.error.set(null);
    this.result.set(null);
    this.svc.runScenario().subscribe({
      next: (r) => {
        this.result.set(r);
        this.running.set(false);
      },
      error: () => {
        this.error.set('Demo çalıştırılamadı. Backend bağlantısını kontrol edin (localhost:5238).');
        this.running.set(false);
      }
    });
  }
}
```

### `src/app/features/demo/demo.component.html`
```html
<div class="space-y-6">
  <div>
    <h2 class="text-lg font-mono font-semibold text-[#f1f5f9]">Demo Senaryo</h2>
    <p class="text-xs text-[#94a3b8] mt-1">Factory + Observer + Singleton desenlerini tek butonla gösterir.</p>
  </div>

  <!-- Senaryo Açıklaması + Buton -->
  <div class="bg-[#1e293b] border border-[#334155] rounded p-6">
    <div class="flex items-start justify-between gap-6">
      <div>
        <p class="text-sm font-mono text-[#f1f5f9] mb-3">Senaryo Adımları</p>
        <ol class="space-y-1.5 text-xs text-[#94a3b8] font-mono list-decimal list-inside">
          <li>Öğrenci ve Öğretmen kullanıcıları oluşturulur</li>
          <li>Sınav duyurusu <span class="text-blue-400">AnnouncementFactory</span> ile üretilir</li>
          <li>Duyuru yayınlanır → <span class="text-purple-400">Observer</span> tetiklenir</li>
          <li><span class="text-purple-400">StudentObserver</span> + <span class="text-purple-400">TeacherObserver</span> bildirim gönderir</li>
          <li><span class="text-orange-400">AppLogger.Instance</span> (Singleton) tüm adımları kaydeder</li>
        </ol>
      </div>
      <button (click)="run()" [disabled]="running()"
        class="shrink-0 px-6 py-3 bg-[#3b82f6] text-white text-sm font-mono rounded hover:bg-[#2563eb] disabled:opacity-50 disabled:cursor-not-allowed transition-colors flex items-center gap-2">
        @if (running()) {
          <span class="w-3 h-3 border border-white border-t-transparent rounded-full animate-spin"></span>
          Çalışıyor...
        } @else {
          ▶ Senaryoyu Çalıştır
        }
      </button>
    </div>
  </div>

  @if (error()) {
    <div class="bg-red-400/10 border border-red-400/30 text-red-400 text-xs font-mono px-4 py-3 rounded">
      {{ error() }}
    </div>
  }

  @if (result(); as r) {
    <!-- Tasarım Desenleri -->
    <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
      <div class="px-4 py-3 border-b border-[#334155] bg-[#334155]/20">
        <h3 class="text-xs font-mono font-semibold text-[#3b82f6] uppercase tracking-widest">Kullanılan Tasarım Desenleri</h3>
      </div>
      <div class="p-4 space-y-3">
        <div class="flex gap-3 items-start">
          <span class="text-[10px] font-mono px-2 py-0.5 rounded bg-blue-500/10 border border-blue-500/30 text-blue-400 shrink-0 mt-0.5">FACTORY</span>
          <p class="text-xs font-mono text-[#94a3b8]">{{ r.patterns.factory }}</p>
        </div>
        <div class="flex gap-3 items-start">
          <span class="text-[10px] font-mono px-2 py-0.5 rounded bg-purple-500/10 border border-purple-500/30 text-purple-400 shrink-0 mt-0.5">OBSERVER</span>
          <p class="text-xs font-mono text-[#94a3b8]">{{ r.patterns.observer }}</p>
        </div>
        <div class="flex gap-3 items-start">
          <span class="text-[10px] font-mono px-2 py-0.5 rounded bg-orange-500/10 border border-orange-500/30 text-orange-400 shrink-0 mt-0.5">SINGLETON</span>
          <p class="text-xs font-mono text-[#94a3b8]">{{ r.patterns.singleton }}</p>
        </div>
      </div>
    </div>

    <!-- Yayın Sonucu -->
    <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Toplam Alıcı</p>
        <p class="text-2xl font-mono text-[#f1f5f9]">{{ r.publishResult.totalRecipients }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Gönderildi</p>
        <p class="text-2xl font-mono text-green-400">{{ r.publishResult.sentCount }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Başarısız</p>
        <p class="text-2xl font-mono text-red-400">{{ r.publishResult.failedCount }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Yayın Saati</p>
        <p class="text-sm font-mono text-[#f1f5f9]">{{ r.publishResult.publishedAt | date:'HH:mm:ss' }}</p>
      </div>
    </div>

    <!-- Oluşturulan Kullanıcılar -->
    <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
      <div class="px-4 py-3 border-b border-[#334155]">
        <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest">Oluşturulan Kullanıcılar</h3>
      </div>
      <table class="w-full">
        <thead>
          <tr class="border-b border-[#334155]">
            <th class="px-4 py-2.5 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Ad</th>
            <th class="px-4 py-2.5 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">E-posta</th>
            <th class="px-4 py-2.5 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Tip</th>
            <th class="px-4 py-2.5 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Kanallar</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-[#334155]">
          @for (u of r.usersCreated; track u.id) {
            <tr>
              <td class="px-4 py-2.5 text-sm text-[#f1f5f9]">{{ u.fullName }}</td>
              <td class="px-4 py-2.5 text-xs font-mono text-[#94a3b8]">{{ u.email }}</td>
              <td class="px-4 py-2.5 text-xs font-mono text-[#94a3b8]">{{ u.userType | enumLabel:'userType' }}</td>
              <td class="px-4 py-2.5">
                <div class="flex gap-1 flex-wrap">
                  @for (nt of u.notificationTypes; track nt) {
                    <span class="text-[10px] font-mono px-1.5 py-0.5 rounded bg-[#0f172a] border border-[#334155] text-[#94a3b8]">
                      {{ nt | enumLabel:'notificationType' }}
                    </span>
                  }
                </div>
              </td>
            </tr>
          }
        </tbody>
      </table>
    </div>

    <!-- Bildirim Logları -->
    <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
      <div class="px-4 py-3 border-b border-[#334155]">
        <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest">
          Bildirim Logları ({{ r.notificationLogs.length }})
        </h3>
      </div>
      <div class="divide-y divide-[#334155]">
        @for (log of r.notificationLogs; track log.id) {
          <div class="px-4 py-3 flex items-start justify-between gap-3">
            <div class="flex-1 min-w-0">
              <div class="flex items-center gap-2 mb-1">
                <span class="text-xs font-mono text-[#f1f5f9]">{{ log.recipientFullName }}</span>
                <span class="text-[10px] text-[#475569]">·</span>
                <span class="text-[10px] font-mono text-[#94a3b8]">{{ log.notificationType | enumLabel:'notificationType' }}</span>
              </div>
              <p class="text-xs text-[#94a3b8] font-mono break-all">{{ log.message }}</p>
            </div>
            <app-status-badge type="notif-status" [value]="log.status" />
          </div>
        }
      </div>
    </div>
  }
</div>
```

---

## 14. AŞAMA 1 — Bildirim Logları

> `notification-list.component.ts` → `src/app/features/notifications/notification-list/` içinde.
> Relative path: `../../../core/...` ve `../../../shared/...`

### `src/app/features/notifications/notification-list/notification-list.component.ts`
```ts
import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';  // DecimalPipe zorunlu (number pipe)
import { NotificationService }  from '../../../core/services/notification.service';
import { NotificationLogResponse, NotificationLogSummaryResponse } from '../../../core/models/notification.model';
import { NotificationStatus }   from '../../../core/models/enums';
import { EnumLabelPipe }        from '../../../shared/pipes/enum-label.pipe';
import { StatusBadgeComponent } from '../../../shared/components/status-badge.component';
import { EmptyStateComponent }  from '../../../shared/components/empty-state.component';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [DatePipe, DecimalPipe, EnumLabelPipe, StatusBadgeComponent, EmptyStateComponent],
  templateUrl: './notification-list.component.html'
})
export class NotificationListComponent implements OnInit {
  private svc = inject(NotificationService);

  logs    = signal<NotificationLogResponse[]>([]);
  summary = signal<NotificationLogSummaryResponse | null>(null);

  NotificationStatus = NotificationStatus;

  ngOnInit() {
    this.svc.getLogs().subscribe(d => this.logs.set(d));
    this.svc.getSummary().subscribe(d => this.summary.set(d));
  }

  get successRate(): number {
    const s = this.summary();
    if (!s || s.totalLogs === 0) return 0;
    return (s.totalSent / s.totalLogs) * 100;
  }
}
```

### `src/app/features/notifications/notification-list/notification-list.component.html`
```html
<div class="space-y-6">
  <div>
    <h2 class="text-lg font-mono font-semibold text-[#f1f5f9]">Bildirim Logları</h2>
    <p class="text-xs text-[#94a3b8] mt-1">{{ logs().length }} kayıt</p>
  </div>

  <!-- Özet Kartlar -->
  @if (summary(); as s) {
    <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Toplam</p>
        <p class="text-2xl font-mono text-[#f1f5f9]">{{ s.totalLogs }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Gönderildi</p>
        <p class="text-2xl font-mono text-green-400">{{ s.totalSent }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Başarısız</p>
        <p class="text-2xl font-mono text-red-400">{{ s.totalFailed }}</p>
      </div>
      <div class="bg-[#1e293b] border border-[#334155] rounded p-4">
        <p class="text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Başarı Oranı</p>
        <!-- DecimalPipe ile number formatlama -->
        <p class="text-2xl font-mono text-[#3b82f6]">{{ successRate | number:'1.0-0' }}%</p>
      </div>
    </div>
  }

  <!-- Log Tablosu -->
  <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
    @if (logs().length > 0) {
      <div class="overflow-x-auto">
        <table class="w-full min-w-[700px]">
          <thead>
            <tr class="border-b border-[#334155]">
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Alıcı</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Duyuru Tipi</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Duyuru</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Kanal</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Durum</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Tarih</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[#334155]">
            @for (log of logs(); track log.id) {
              <tr class="hover:bg-[#334155]/30 transition-colors">
                <td class="px-4 py-3">
                  <p class="text-sm text-[#f1f5f9]">{{ log.recipientFullName }}</p>
                  <p class="text-[10px] text-[#94a3b8] font-mono">{{ log.recipientUserType | enumLabel:'userType' }}</p>
                </td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">
                  {{ log.announcementType | enumLabel:'announcementType' }}
                </td>
                <td class="px-4 py-3 text-xs text-[#94a3b8] max-w-[180px] truncate">
                  {{ log.announcementTitle }}
                </td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">
                  {{ log.notificationType | enumLabel:'notificationType' }}
                </td>
                <td class="px-4 py-3">
                  <app-status-badge type="notif-status" [value]="log.status" />
                </td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">
                  {{ log.sentAt | date:'dd.MM.yyyy HH:mm' }}
                </td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    } @else {
      <app-empty-state icon="◎" message="Henüz bildirim logu yok. Demo senaryosunu çalıştırın." />
    }
  </div>
</div>
```

---

## 15. AŞAMA 2 — Kullanıcılar

> `user-list.component.ts` → `src/app/features/users/user-list/` içinde.
> Relative path: `../../../core/...` ve `../../../shared/...`

### `src/app/features/users/user-list/user-list.component.ts`
```ts
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { UserService }        from '../../../core/services/user.service';
import { UserResponse, CreateUserRequest } from '../../../core/models/user.model';
import { UserType, NotificationType, UserTypeLabel, NotificationTypeLabel } from '../../../core/models/enums';
import { EnumLabelPipe }          from '../../../shared/pipes/enum-label.pipe';
import { EmptyStateComponent }    from '../../../shared/components/empty-state.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, DatePipe, EnumLabelPipe, EmptyStateComponent, ConfirmDialogComponent],
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
        this.success.set('Kullanıcı başarıyla oluşturuldu.');
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

  private emptyForm(): CreateUserRequest {
    return {
      fullName: '',
      email: '',
      phoneNumber: null,
      userType: UserType.Student,
      notificationTypes: [NotificationType.Email]
    };
  }
}
```

### `src/app/features/users/user-list/user-list.component.html`
```html
<div class="space-y-6">
  <div class="flex items-center justify-between">
    <div>
      <h2 class="text-lg font-mono font-semibold text-[#f1f5f9]">Kullanıcılar</h2>
      <p class="text-xs text-[#94a3b8] mt-1">{{ users().length }} kullanıcı kayıtlı</p>
    </div>
    <button (click)="showForm.set(!showForm())"
      class="px-4 py-2 bg-[#3b82f6] text-white text-xs font-mono rounded hover:bg-[#2563eb] transition-colors">
      + Yeni Kullanıcı
    </button>
  </div>

  @if (success()) {
    <div class="bg-green-400/10 border border-green-400/30 text-green-400 text-xs font-mono px-4 py-3 rounded">
      {{ success() }}
    </div>
  }
  @if (error()) {
    <div class="bg-red-400/10 border border-red-400/30 text-red-400 text-xs font-mono px-4 py-3 rounded">
      {{ error() }}
    </div>
  }

  @if (showForm()) {
    <div class="bg-[#1e293b] border border-[#334155] rounded p-5">
      <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest mb-4">Yeni Kullanıcı</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Ad Soyad *</label>
          <input [(ngModel)]="form.fullName" type="text" placeholder="Ahmet Yılmaz"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono placeholder-[#475569] focus:border-[#3b82f6] focus:outline-none" />
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">E-posta *</label>
          <input [(ngModel)]="form.email" type="email" placeholder="ahmet@uni.edu.tr"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono placeholder-[#475569] focus:border-[#3b82f6] focus:outline-none" />
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Telefon</label>
          <input [(ngModel)]="form.phoneNumber" type="tel" placeholder="+90 555 000 00 00"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono placeholder-[#475569] focus:border-[#3b82f6] focus:outline-none" />
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Kullanıcı Tipi *</label>
          <!-- [ngValue] kullanıyoruz — number olarak gider, string değil -->
          <select [(ngModel)]="form.userType"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono focus:border-[#3b82f6] focus:outline-none">
            <option [ngValue]="UserType.Student">Öğrenci</option>
            <option [ngValue]="UserType.Teacher">Öğretmen</option>
          </select>
        </div>

        <div class="md:col-span-2">
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-2">Bildirim Kanalları *</label>
          <div class="flex gap-3 flex-wrap">
            @for (ch of [NotificationType.Email, NotificationType.Sms, NotificationType.Push]; track ch) {
              <button type="button" (click)="toggleChannel(ch)"
                [class.border-[#3b82f6]]="isChannelSelected(ch)"
                [class.text-[#3b82f6]]="isChannelSelected(ch)"
                [class.border-[#334155]]="!isChannelSelected(ch)"
                [class.text-[#94a3b8]]="!isChannelSelected(ch)"
                class="px-4 py-2 rounded border text-xs font-mono transition-colors hover:border-[#3b82f6]">
                {{ ch | enumLabel:'notificationType' }}
              </button>
            }
          </div>
        </div>
      </div>

      <div class="flex gap-3 mt-5">
        <button (click)="submit()"
          class="px-5 py-2 bg-[#3b82f6] text-white text-xs font-mono rounded hover:bg-[#2563eb] transition-colors">
          Kaydet
        </button>
        <button (click)="showForm.set(false); form = emptyForm(); error.set(null)"
          class="px-5 py-2 border border-[#334155] text-[#94a3b8] text-xs font-mono rounded hover:bg-[#334155] transition-colors">
          İptal
        </button>
      </div>
    </div>
  }

  <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
    @if (users().length > 0) {
      <table class="w-full">
        <thead>
          <tr class="border-b border-[#334155]">
            <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Ad Soyad</th>
            <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">E-posta</th>
            <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Tip</th>
            <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Kanallar</th>
            <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Kayıt</th>
            <th class="px-4 py-3 text-center text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">İşlem</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-[#334155]">
          @for (u of users(); track u.id) {
            <tr class="hover:bg-[#334155]/30 transition-colors">
              <td class="px-4 py-3 text-sm text-[#f1f5f9]">{{ u.fullName }}</td>
              <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">{{ u.email }}</td>
              <td class="px-4 py-3">
                <span class="text-xs font-mono"
                  [class.text-blue-400]="u.userType === UserType.Student"
                  [class.text-purple-400]="u.userType === UserType.Teacher">
                  {{ u.userType | enumLabel:'userType' }}
                </span>
              </td>
              <td class="px-4 py-3">
                <div class="flex gap-1 flex-wrap">
                  @for (nt of u.notificationTypes; track nt) {
                    <span class="text-[10px] font-mono px-1.5 py-0.5 rounded bg-[#0f172a] border border-[#334155] text-[#94a3b8]">
                      {{ nt | enumLabel:'notificationType' }}
                    </span>
                  }
                </div>
              </td>
              <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">{{ u.createdAt | date:'dd.MM.yyyy' }}</td>
              <td class="px-4 py-3 text-center">
                <button (click)="confirmDeactivate(u.id)"
                  class="text-xs font-mono text-red-400 hover:text-red-300 hover:underline">
                  Pasifleştir
                </button>
              </td>
            </tr>
          }
        </tbody>
      </table>
    } @else {
      <app-empty-state icon="◈" message="Henüz kullanıcı eklenmedi." />
    }
  </div>
</div>

<app-confirm-dialog
  [open]="deactivateTarget() !== null"
  message="Bu kullanıcıyı pasifleştirmek istediğinizden emin misiniz?"
  (confirm)="executeDeactivate()"
  (cancel)="deactivateTarget.set(null)"
/>
```

---

## 16. AŞAMA 2 — Duyurular

> `announcement-list.component.ts` → `src/app/features/announcements/announcement-list/` içinde.
> Relative path: `../../../core/...` ve `../../../shared/...`

### `src/app/features/announcements/announcement-list/announcement-list.component.ts`
```ts
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

  private emptyForm(): CreateAnnouncementRequest {
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
```

### `src/app/features/announcements/announcement-list/announcement-list.component.html`
```html
<div class="space-y-6">
  <div class="flex items-center justify-between">
    <div>
      <h2 class="text-lg font-mono font-semibold text-[#f1f5f9]">Duyurular</h2>
      <p class="text-xs text-[#94a3b8] mt-1">{{ announcements().length }} duyuru</p>
    </div>
    <button (click)="showForm.set(!showForm())"
      class="px-4 py-2 bg-[#3b82f6] text-white text-xs font-mono rounded hover:bg-[#2563eb] transition-colors">
      + Yeni Duyuru
    </button>
  </div>

  @if (publishResult(); as pr) {
    <div class="bg-green-400/10 border border-green-400/30 rounded p-4">
      <p class="text-xs font-mono text-green-400 font-semibold mb-3">Duyuru Yayınlandı!</p>
      <div class="grid grid-cols-3 gap-3">
        <div>
          <p class="text-[10px] text-[#94a3b8] font-mono">Toplam Alıcı</p>
          <p class="text-xl font-mono text-[#f1f5f9]">{{ pr.totalRecipients }}</p>
        </div>
        <div>
          <p class="text-[10px] text-[#94a3b8] font-mono">Gönderildi</p>
          <p class="text-xl font-mono text-green-400">{{ pr.sentCount }}</p>
        </div>
        <div>
          <p class="text-[10px] text-[#94a3b8] font-mono">Başarısız</p>
          <p class="text-xl font-mono text-red-400">{{ pr.failedCount }}</p>
        </div>
      </div>
    </div>
  }

  @if (success()) {
    <div class="bg-green-400/10 border border-green-400/30 text-green-400 text-xs font-mono px-4 py-3 rounded">
      {{ success() }}
    </div>
  }
  @if (error()) {
    <div class="bg-red-400/10 border border-red-400/30 text-red-400 text-xs font-mono px-4 py-3 rounded">
      {{ error() }}
    </div>
  }

  @if (showForm()) {
    <div class="bg-[#1e293b] border border-[#334155] rounded p-5">
      <h3 class="text-xs font-mono font-semibold text-[#f1f5f9] uppercase tracking-widest mb-4">Yeni Duyuru</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

        <div class="md:col-span-2">
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Başlık *</label>
          <input [(ngModel)]="form.title" type="text" placeholder="Duyuru başlığı"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono placeholder-[#475569] focus:border-[#3b82f6] focus:outline-none" />
        </div>

        <div class="md:col-span-2">
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">İçerik *</label>
          <textarea [(ngModel)]="form.content" rows="3" placeholder="Duyuru içeriği..."
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono placeholder-[#475569] focus:border-[#3b82f6] focus:outline-none resize-none">
          </textarea>
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Duyuru Tipi *</label>
          <!-- [ngValue] — number olarak gider -->
          <select [(ngModel)]="form.announcementType"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono focus:border-[#3b82f6] focus:outline-none">
            <option [ngValue]="AnnouncementType.Exam">Sınav</option>
            <option [ngValue]="AnnouncementType.Event">Etkinlik</option>
            <option [ngValue]="AnnouncementType.FoodMenu">Yemekhane</option>
            <option [ngValue]="AnnouncementType.Library">Kütüphane</option>
          </select>
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Hedef Kitle *</label>
          <!-- [ngValue] — number olarak gider -->
          <select [(ngModel)]="form.targetAudience"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono focus:border-[#3b82f6] focus:outline-none">
            <option [ngValue]="TargetAudience.All">Herkes</option>
            <option [ngValue]="TargetAudience.Students">Öğrenciler</option>
            <option [ngValue]="TargetAudience.Teachers">Öğretmenler</option>
          </select>
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Öncelik</label>
          <!-- [ngValue] ile null da dahil -->
          <select [(ngModel)]="form.priority"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono focus:border-[#3b82f6] focus:outline-none">
            <option [ngValue]="null">— Otomatik —</option>
            <option [ngValue]="AnnouncementPriority.Low">Düşük</option>
            <option [ngValue]="AnnouncementPriority.Normal">Normal</option>
            <option [ngValue]="AnnouncementPriority.High">Yüksek</option>
            <option [ngValue]="AnnouncementPriority.Urgent">Acil</option>
          </select>
        </div>

        <div>
          <label class="block text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest mb-1">Son Tarih</label>
          <input [(ngModel)]="form.expiresAt" type="datetime-local"
            class="w-full bg-[#0f172a] border border-[#334155] rounded px-3 py-2 text-sm text-[#f1f5f9] font-mono focus:border-[#3b82f6] focus:outline-none" />
        </div>
      </div>

      <div class="flex gap-3 mt-5">
        <button (click)="submit()"
          class="px-5 py-2 bg-[#3b82f6] text-white text-xs font-mono rounded hover:bg-[#2563eb] transition-colors">
          Taslak Oluştur
        </button>
        <button (click)="showForm.set(false); form = emptyForm(); error.set(null)"
          class="px-5 py-2 border border-[#334155] text-[#94a3b8] text-xs font-mono rounded hover:bg-[#334155] transition-colors">
          İptal
        </button>
      </div>
    </div>
  }

  <div class="bg-[#1e293b] border border-[#334155] rounded overflow-hidden">
    @if (announcements().length > 0) {
      <div class="overflow-x-auto">
        <table class="w-full min-w-[750px]">
          <thead>
            <tr class="border-b border-[#334155]">
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Başlık</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Tip</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Kitle</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Durum</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Öncelik</th>
              <th class="px-4 py-3 text-left text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">Tarih</th>
              <th class="px-4 py-3 text-center text-[10px] font-mono text-[#94a3b8] uppercase tracking-widest">İşlem</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[#334155]">
            @for (a of announcements(); track a.id) {
              <tr class="hover:bg-[#334155]/30 transition-colors">
                <td class="px-4 py-3 text-sm text-[#f1f5f9] max-w-[180px] truncate">{{ a.title }}</td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">{{ a.announcementType | enumLabel:'announcementType' }}</td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">{{ a.targetAudience | enumLabel:'targetAudience' }}</td>
                <td class="px-4 py-3"><app-status-badge type="status" [value]="a.status" /></td>
                <td class="px-4 py-3"><app-status-badge type="priority" [value]="a.priority" /></td>
                <td class="px-4 py-3 text-xs font-mono text-[#94a3b8]">{{ a.createdAt | date:'dd.MM.yy HH:mm' }}</td>
                <td class="px-4 py-3 text-center">
                  @if (a.status === AnnouncementStatus.Draft) {
                    <button (click)="publish(a.id)"
                      class="text-xs font-mono text-green-400 hover:underline">
                      Yayınla
                    </button>
                  }
                  @if (a.status === AnnouncementStatus.Published) {
                    <button (click)="archive(a.id)"
                      class="text-xs font-mono text-[#94a3b8] hover:text-[#f1f5f9] hover:underline">
                      Arşivle
                    </button>
                  }
                  @if (a.status === AnnouncementStatus.Archived) {
                    <span class="text-xs font-mono text-[#475569]">—</span>
                  }
                </td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    } @else {
      <app-empty-state icon="◉" message="Henüz duyuru oluşturulmadı." />
    }
  </div>
</div>
```

---

## 17. Uygulama Sırası

```
Aşama 1 (önce bunlar — demo için yeterli):
  ✓ npm install + tailwind kurulumu
  ✓ environments/environment.ts
  ✓ app.config.ts + app.routes.ts + app.component.ts
  ✓ core/models/ (tüm .ts dosyaları)
  ✓ core/services/ (tüm .ts dosyaları)
  ✓ core/interceptors/loading.interceptor.ts
  ✓ shared/components/ (status-badge, empty-state, confirm-dialog)
  ✓ shared/pipes/enum-label.pipe.ts
  ✓ layout/shell/ + layout/sidebar/
  ✓ features/dashboard/
  ✓ features/demo/
  ✓ features/notifications/notification-list/
  → ng build && ng serve

Aşama 2 (Aşama 1 çalışıyorsa):
  ✓ features/users/user-list/
  ✓ features/announcements/announcement-list/
  → ng build && ng serve
```

---

## 18. Hızlı Kontrol Listesi

```
[ ] environment.ts → apiUrl: 'http://localhost:5238/api'
[ ] Tüm select'lerde [ngValue] kullanıldı (UserType, AnnouncementType, TargetAudience, AnnouncementPriority)
[ ] dashboard.component.ts → KeyValuePipe import edildi
[ ] notification-list.component.ts → DecimalPipe import edildi
[ ] user.service.ts → method adı: deactivate() (delete değil)
[ ] user-list.component.html → buton etiketi: "Pasifleştir"
[ ] Her features klasöründeki import path'leri: ../../../core/... veya ../../core/...
[ ] Angular Material, Lucide, date-fns kurulmadı
[ ] ng build hatasız geçiyor
```

---

## 19. Renk Referansı

| Token    | Hex       | Kullanım                      |
|----------|-----------|-------------------------------|
| bg       | `#0f172a` | Sayfa arka planı              |
| surface  | `#1e293b` | Kart, tablo, form arka planı  |
| border   | `#334155` | Tüm kenarlıklar               |
| accent   | `#3b82f6` | Buton, aktif nav, link        |
| text     | `#f1f5f9` | Ana metin                     |
| muted    | `#94a3b8` | İkincil metin, label          |
| faint    | `#475569` | Placeholder                   |
| success  | `green-400` | Yayında, gönderildi         |
| danger   | `red-400`   | Hata, pasifleştir            |
| warning  | `orange-400`| Yüksek öncelik              |
| urgent   | `red-400`   | Acil öncelik                |
