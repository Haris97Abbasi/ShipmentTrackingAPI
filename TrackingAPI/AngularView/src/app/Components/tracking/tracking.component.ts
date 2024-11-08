import { Component, inject, OnInit } from '@angular/core';
import { TrackingService } from '../../Services/tracking.service';
import { TrackingRequest } from '../../Models/tracking-request';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tracking',
  standalone: true,
  templateUrl: './tracking.component.html',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  styleUrls: ['./tracking.component.css']
})
export class TrackingComponent implements OnInit {
  trackingData: TrackingRequest = {
    TrackingNumber: '',
    CarrierCode: ''
  };
  couriers: any[] = [];
  couriersFetched = false; // Flag to check if couriers are fetched
  responseMessage: string = '';
  isSuccess: boolean = false;
  responseData: any = null;
  responseKeys: string[] = [];
  trackInfo: any[] = []; // Array to store tracking information
  viewMode: string = 'tracking';

  trackingService = inject(TrackingService);
  trackingForm: FormGroup = new FormGroup({});
  
  constructor(private fb: FormBuilder) { }

  // This ngOnInit function is called when the component is initialized
  ngOnInit(): void {
    this.setFormState();
  }

  //This function initializes the tracking form
  setFormState() {
    this.trackingForm = this.fb.group({
      trackingnumber: [0, Validators.required],
      carriercode: ['', [Validators.required]]
    });
  }

  // This function is called when the form is submitted
  onTrack() {
    this.trackingService.track(this.trackingData).subscribe({
      next: (response: any) => { 
        this.responseMessage = `Tracking Response Received`;
        this.isSuccess = true;
        this.viewMode = 'tracking'; // Switch to tracking view

        const responseData = (response as any).data?.items?.[0];
        this.responseData = responseData;
        this.responseKeys = responseData ? Object.keys(responseData) : [];
        this.trackInfo = responseData?.origin_info?.trackinfo || [];
      },
      error: () => {
        this.responseMessage = 'Invalid Tracking ID or Carrier Code';
        this.isSuccess = false;
        this.responseData = null;
        this.responseKeys = [];
        this.trackInfo = [];
      }
    });
  }

  // This function fetches the list of couriers
  fetchCouriers() {
    this.trackingService.getCouriers().subscribe({
      next: (response: any) => {
        this.couriers = response;
        this.couriersFetched = true;
        this.viewMode = 'couriers';
      },
      error: () => {
        this.couriers = [];
        this.couriersFetched = true;
        this.viewMode = 'couriers'; 
      }
    });
  }

  // This function toggles the view between tracking and couriers
  toggleView(view: string) {
    this.viewMode = view;
    if (view === 'couriers') {
      this.fetchCouriers();
    } else {
      this.responseData = null;
      this.responseKeys = [];
      this.trackInfo = [];
    }
  }
}
