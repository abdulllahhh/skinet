import { HttpInterceptorFn } from '@angular/common/http';
import { inject} from '@angular/core';
import { delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);

  busyService.busy(); // Start loading
  return next(req).pipe(
    delay(500), // Simulate a delay of 0.5 second
    finalize(() => {
      busyService.idle(); // Stop loading
    })
  );
};
