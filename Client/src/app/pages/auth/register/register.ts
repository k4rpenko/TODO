import { Component, inject } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import { AuthService } from '../../../service/API/auth/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { ValidatorService } from '../../../service/guards/validator.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  Rest = inject(AuthService);
  constructor(private router: Router, private cookieService: CookieService, private validator: ValidatorService) { }

  name: string = '';
  email: string = '';
  password: string = '';
  repeating_password: string = '';
  Error: string = '';
  agree: boolean = false;

  isAgree(): void {
    this.agree = !this.agree;
  }

  onSubmit(): void {
    if (!this.email || this.email.trim() === '') {
      this.Error = 'Email is required';
      return;
    }
    
    if (!this.name || this.name.trim() === '') {
      this.Error = 'Name is required';
      return;
    }

    if (!this.password || this.password.trim() === '') {
      this.Error = 'Password is required';
      return;
    }

    if (!this.repeating_password || this.repeating_password.trim() === '') {
      this.Error = 'Confirm password is required';
      return;
    }

    if(this.password != this.repeating_password) {
      this.Error = 'passwords do not match';
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
    
    
    

    this.Rest.PostRegister(this.email, this.password, this.name).subscribe({
      next: (response) => {
        const assets_token = response.token;
        this.cookieService.set( 'access_token', assets_token, 1 / 24, '/');
        this.router.navigate(['home']);
        this.Error = '';
      },
      error: (error) => {
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
    });
  }

}
