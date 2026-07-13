import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { VerifyTokenService } from './service/guards/verify-token.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('Client');
  private verifyToken = inject(VerifyTokenService);

  ngOnInit(): void {
    this.verifyToken.start();
  }
}
