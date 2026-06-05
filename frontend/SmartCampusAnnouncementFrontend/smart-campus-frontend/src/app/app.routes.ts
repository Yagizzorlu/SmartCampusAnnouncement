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
