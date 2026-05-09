import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home.component').then(c => c.HomeComponent)
  },
  {
    path: 'flights',
    loadComponent: () => import('./pages/flights/flights.component').then(c => c.FlightsComponent)
  },
  {
    path: 'search',
    loadComponent: () => import('./pages/search/search.component').then(c => c.SearchComponent)
  },
  {
    path: 'booking',
    loadComponent: () => import('./pages/booking/booking.component').then(c => c.BookingComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./pages/profile/profile.component').then(c => c.ProfileComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
