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
