import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FlightService, FlightSearchRequestDTO } from '../../services/flight.service';
import { BookingService, BookingRequestDTO } from '../../services/booking.service';
import { Observable, of } from 'rxjs';

export interface SearchForm {
  from: string;
  fromId?: number;
  to: string;
  toId?: number;
  departureDate: string;
  returnDate: string;
  passengers: number;
  tripType: 'oneway' | 'roundtrip';
  class: 'economy' | 'business' | 'first';
}

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  searchForm: SearchForm = {
    from: '',
    to: '',
    departureDate: '',
    returnDate: '',
    passengers: 1,
    tripType: 'roundtrip',
    class: 'economy'
  };

  searchResults$?: Observable<any>;
  isLoading = false;
  popularAirports$?: Observable<any>;

  constructor(
    private flightService: FlightService,
    private bookingService: BookingService,
    private router: Router
  ) {
    this.popularAirports$ = this.flightService.getAirports({});
  }

  onSearch(): void {
    if (!this.searchForm.from || !this.searchForm.to || !this.searchForm.departureDate) {
      alert('Please fill in all required fields');
      return;
    }

    this.isLoading = true;

    const searchRequest: FlightSearchRequestDTO = {
      cabinType: this.searchForm.class.charAt(0).toUpperCase() + this.searchForm.class.slice(1) as 'Economy' | 'Business' | 'First',
      timeDeparture: this.searchForm.departureDate,
      airportOriginId: this.searchForm.fromId,
      airportDestinationId: this.searchForm.toId,
      minimumFreeSeats: this.searchForm.passengers,
      provider: 'All'
    };

    this.searchResults$ = this.flightService.searchFlights(searchRequest);
    this.searchResults$.subscribe({
      next: (results) => {
        console.log('Search results:', results);
        this.isLoading = false;
        // TODO: Navigate to results page or display results
      },
      error: (error) => {
        console.error('Search error:', error);
        this.isLoading = false;
        alert('Error searching for flights. Please try again.');
      }
    });
  }

  onTripTypeChange(tripType: 'oneway' | 'roundtrip'): void {
    this.searchForm.tripType = tripType;
    if (tripType === 'oneway') {
      this.searchForm.returnDate = '';
    }
  }

  swapAirports(): void {
    const temp = this.searchForm.from;
    this.searchForm.from = this.searchForm.to;
    this.searchForm.to = temp;
  }

  selectAirportByCode(airport: any, field: 'from' | 'to'): void {
    if (field === 'from') {
      this.searchForm.from = airport.codeIATA;
      this.searchForm.fromId = airport.countryID; // Use countryID as airport ID
    } else {
      this.searchForm.to = airport.codeIATA;
      this.searchForm.toId = airport.countryID; // Use countryID as airport ID
    }
  }

  bookFlight(flight: any): void {
    const bookingRequest: BookingRequestDTO = {
      flightId: flight.id,
      providerName: flight.providerName,
      passengerCount: 1, // TODO: Get from form
      passengerList: [
        {
          firstName: 'John', // TODO: Get from form
          lastName: 'Doe',
          dateOfBirth: '1990-01-15',
          nationality: 'United States',
          email: 'john.doe@example.com',
          phone: '+1 (555) 123-4567'
        }
      ]
    };

    this.bookingService.createBooking(bookingRequest).subscribe({
      next: (response) => {
        console.log('Booking successful:', response);
        alert(`Booking confirmed! Reference: ${response.referenceCode}`);
        // TODO: Navigate to booking details page
      },
      error: (error) => {
        console.error('Booking error:', error);
        alert('Error creating booking. Please try again.');
      }
    });
  }
}
