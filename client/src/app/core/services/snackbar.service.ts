import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  private _snackbar = inject(MatSnackBar); 
  error(message: string) {
    this._snackbar.open(message, 'close', {
      duration: 5000,
      panelClass: ['snack-error'],      
    });
  }
  success(message: string) {
    this._snackbar.open(message, 'close', {
      duration: 5000,
      panelClass: ['snack-success'],      
    });
  }
}
