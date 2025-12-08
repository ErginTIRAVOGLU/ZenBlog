
export interface ErrorModel {
  propertyName?: string | null;
  errorMessage?: string | null;
}

export const initialError: ErrorModel = {
  propertyName: '',
  errorMessage: ''
}

