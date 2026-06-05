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
