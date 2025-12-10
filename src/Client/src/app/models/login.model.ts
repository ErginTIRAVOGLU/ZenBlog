export interface LoginModel {
  emailOrUserName: string;
  password: string;
}

export const initialLoginModel: LoginModel = {
  emailOrUserName: '',
  password: '',
}

export interface LoginResponseModel {
  token: string;
  expiration: string;
}

export const initialLoginResponseModel: LoginResponseModel = {
  token: '',
  expiration: '',
}
