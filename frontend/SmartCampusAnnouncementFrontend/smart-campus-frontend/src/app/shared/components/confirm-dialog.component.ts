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
