import { Routes } from '@angular/router';
import { TrackingComponent } from './Components/tracking/tracking.component';

export const routes: Routes = [
    {
        path:"", component: TrackingComponent 
    },
    {
        path:"tracking", component: TrackingComponent
    }
];
