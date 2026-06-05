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
