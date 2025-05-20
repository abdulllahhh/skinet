import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error) {
        const { status, error: errData } = error;

        // Handle validation errors (typically 400)
        if (errData?.errors && typeof errData.errors === 'object') {
          const modelStateErrors = [];

          for (const key in errData.errors) {
            if (errData.errors[key]) {
              modelStateErrors.push(...errData.errors[key]); // Support for arrays
            }
          }

          if (modelStateErrors.length) {
            snackbar.error(modelStateErrors.join('\n'));
          }
        } else if (status === 400 || status === 401) {
          snackbar.error(errData?.title || errData || 'An error occurred.');
        } else if (status === 404) {
          router.navigateByUrl('/not-found');
        } else if (status === 500) {
          const navigationExtras: NavigationExtras = {
            state: { error: errData },
          };
          router.navigateByUrl('/server-error', navigationExtras);
        } else {
          snackbar.error('An unexpected error occurred.');
        }
      }

      return throwError(() => error);
    })
  );
};
// This interceptor handles HTTP errors globally. It checks the status code of the error response and displays appropriate messages using a Snackbar service.
// It also navigates to specific routes based on the error type (e.g., 404 or 500 errors).