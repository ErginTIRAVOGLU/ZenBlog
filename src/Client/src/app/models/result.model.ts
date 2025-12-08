import { ErrorModel } from "./error.model";

export interface Result<T>{
  data: T | null;
  isSuccess: boolean;
  errors?: ErrorModel[] | null;
}

export const initialResult = <T>(): Result<T> => ({
  data: null,
  isSuccess: false,
  errors: null
});

