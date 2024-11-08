import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TrackingRequest } from '../Models/tracking-request';

@Injectable({
  providedIn: 'root'
})

// This service will be used to make the POST request to the API to track the package
export class TrackingService {

  private apiUrl = 'https://localhost:7086/api/Tracking/track';
  private getCouriersUrl = 'https://localhost:7086/api/Tracking/get-couriers';

  constructor() { }
  http = inject(HttpClient);

  track(trackingData: TrackingRequest) {
    return this.http.post(this.apiUrl, trackingData);
  }

  // New method for making the GET request to get couriers
  getCouriers() {
    return this.http.get(this.getCouriersUrl);
  }
}
