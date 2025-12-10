import { LoginModel, LoginResponseModel } from './../models/login.model';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly #http=inject(HttpClient);
  readonly baseUrl = 'https://localhost:7179/api/auth/login';

  login(LoginModel: LoginModel) {
    return this.#http.post<Result<LoginResponseModel>>(this.baseUrl, LoginModel);
  }
}
