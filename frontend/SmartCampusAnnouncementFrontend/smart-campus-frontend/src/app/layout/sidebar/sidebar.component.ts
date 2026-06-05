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
