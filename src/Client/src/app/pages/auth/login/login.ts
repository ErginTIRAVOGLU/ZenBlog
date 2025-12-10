import { initialLoginModel, LoginModel } from './../../../models/login.model';
import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../services/auth-service';

declare const alertify: any;

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  readonly authService = inject(AuthService);
  readonly LoginModel = signal<LoginModel>({ ...initialLoginModel });

  login() {
    const loginData = this.LoginModel();

    if (!loginData.emailOrUserName || !loginData.password) {
      alertify.error('Please fill in all fields.');
      return;
    }

    this.authService.login(loginData).subscribe({
      next: (result) => {
        if (result.isSuccess && result.data) {
          localStorage.setItem('token', result.data.token);
          localStorage.setItem('expiration', result.data.expiration);
          alertify.success('Login successful!');
          window.location.href = '/admin/dashboard';
        } else {
          alertify.error('Login failed. Please check your credentials.');
          console.log('Login failed: ' + result.errors?.map(e => e.errorMessage).join(', '));
        }
      },
      error: (err) => {
        console.error('Login error:', err);
        if (err.error) {
          console.log('Error response:', err.error);
          if (err.error.errors && Array.isArray(err.error.errors)) {
            const errorMessages = err.error.errors.map((e: any) => e.errorMessage).join(', ');
            alertify.error(errorMessages);
          } else {
            alertify.error('An error occurred during login. Please try again.');
          }
        } else {
          alertify.error('An error occurred during login. Please try again.');
        }
      },
    });
  }
}
