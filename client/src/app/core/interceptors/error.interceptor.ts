import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);
  const modelStateErrors = [];
  return next(req).pipe(
    catchError((error : HttpErrorResponse) => {
      if(error.error.errors){
        for(const key in error.error.errors) {
          if (error.error.errors[key]) {
            modelStateErrors.push(error.error.errors[key]);
          }
          else {
            snackbar.error(error.error.title || error.error);
          }
      }}
        if(error.status === 400){
          snackbar.error(error.error.title || error.error);
        }
       if(error.status === 401){
          snackbar.error(error.error.title || error.error);
        }
       if(error.status === 404){
          router.navigateByUrl('/not-found');
        }
       if(error.status === 500){
        const navigationExtras : NavigationExtras = { state: { error: error.error } };
          router.navigateByUrl('/server-error', navigationExtras);
        }
        return throwError(() => error);
      

    })
  );
  
};
