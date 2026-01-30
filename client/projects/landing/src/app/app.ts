import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('landing');
  http = inject(HttpClient);
  api = environment.apiUrl;

  send(): void {
    const eR: EchoRequest = {
      name: 'Reza Taba'
    };

    console.log(eR.name);

    this.http.post(this.api + 'echo', eR).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.error(error)
    });
  }
}

export interface EchoRequest {
  name: string;
}
