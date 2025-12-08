import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class SweetalertService {

  confirm(title: string, text: string): Promise<boolean> {
    return Swal.fire({
      title: title,
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    }).then((result) => {
      return result.isConfirmed;
    });
  }

  alertSuccess(title: string, text: string): void {
    Swal.fire({
      title: title,
      text: text,
      icon: 'success',
      confirmButtonText: 'OK',
    });
  }

  alertError(title: string, text: string): void {
    Swal.fire({
      title: title,
      text: text,
      icon: 'error',
      confirmButtonText: 'OK',
    });
  }
}
