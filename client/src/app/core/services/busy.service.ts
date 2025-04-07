import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  loading = false;
  busuyRequestCount = 0;

  busy() { 
    this.busuyRequestCount++;
    this.loading = true;
  }
  idle() {
    this.busuyRequestCount--;
    if (this.busuyRequestCount <= 0) {
      this.busuyRequestCount = 0;
      this.loading = false;
    }
  }
}
