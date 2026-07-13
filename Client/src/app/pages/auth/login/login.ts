import { Component, inject } from '@angular/core';
import { AuthService } from '../../../service/API/auth/auth.service';
import {Router, RouterLink} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import { ValidatorService } from '../../../service/guards/validator.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  Rest = inject(AuthService);
  constructor(private router: Router, private cookieService: CookieService, private validator: ValidatorService) { }

  email: string = '';
  password: string = '';
  Error: string = '';

  onSubmit(): void {

    if (!this.email || this.email.trim() === '') {
      this.Error = 'Email is required';
      return;
    }
    

    if (!this.password || this.password.trim() === '') {
      this.Error = 'Password is required';
      return;
    }

    if (!this.validator.isValidEmail(this.email)) {
      this.Error = 'Please enter a valid email address.';
      return;
    }

    if (!this.validator.isValidPassword(this.password)) {
      this.Error = 'The password must contain at least 6 characters, including letters and numbers.';
      return;
    }
    
    this.Rest.PostLogin(this.email, this.password).subscribe({
      next: (response) => {
        const assets_token = response.token;
        this.cookieService.set( 'access_token', assets_token, 1 / 24, '/');
        this.router.navigate(['home']);
        this.Error = '';
      },
      error: (error) => {
        if (error.status == 429) {
          this.Error = 'Too many requests. Please try again later.';
        }
        else  {
          if (error.status === 429) {
            this.Error = error.error.error;
            return;
          }

          if (error.error?.error) {
            this.Error = error.error.error;
            return;
          }

          if (error.error?.errors?.Name?.length) {
            this.Error = error.error.errors.Name[0];
            return;
          }
        }
      }
    });
  }

}
