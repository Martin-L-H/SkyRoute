import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlightService, FlightSearchRequestDTO, FlightSearchResponseDTO } from '../../services/flight.service';
import { Observable } from 'rxjs';

export interface Flight {
  id: string;
  flightNumber: string;
  airline: string;
  from: string;
  to: string;
  departureTime: string;
  arrivalTime: string;
  price: number;
  status: 'on-time' | 'delayed' | 'cancelled';
  aircraft: string;
}

export interface FlightSearchForm {
  provider: string;
  cabinType: string;
  timeDeparture: string;
  airportOriginId: number | null;
  airportDestinationId: number | null;
  minimumFreeSeats: number | null;
  durationMinutes: number | null;
}

@Component({
  selector: 'app-flights',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.scss']
})
export class FlightsComponent {
  searchForm: FlightSearchForm = {
    provider: 'All',
    cabinType: '',
    timeDeparture: '',
    airportOriginId: null,
    airportDestinationId: null,
    minimumFreeSeats: null,
    durationMinutes: null
  };

  providers = ['All', 'GlobalAir', 'BudgetWings'];
  cabinTypes = [
    { value: '', label: 'Any Class' },
    { value: 'Economy', label: 'Economy' },
    { value: 'Premium', label: 'Premium' },
    { value: 'Business', label: 'Business' },
    { value: 'First', label: 'First' }
  ];

  flights$?: Observable<FlightSearchResponseDTO[]>;
  isLoading = false;
  selectedFlight: FlightSearchResponseDTO | null = null;

  constructor(private flightService: FlightService) {}

  searchFlights(): void {
    this.isLoading = true;

    const searchRequest: FlightSearchRequestDTO = {
      provider: this.searchForm.provider,
      cabinType: this.searchForm.cabinType as any || undefined,
      timeDeparture: this.searchForm.timeDeparture || undefined,
      airportOriginId: this.searchForm.airportOriginId || undefined,
      airportDestinationId: this.searchForm.airportDestinationId || undefined,
      minimumFreeSeats: this.searchForm.minimumFreeSeats || undefined,
      durationMinutes: this.searchForm.durationMinutes || 0
    };

    this.flights$ = this.flightService.searchFlights(searchRequest);
    this.flights$.subscribe({
      next: (results) => {
        console.log('Flight search results:', results);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Flight search error:', error);
        this.isLoading = false;
        alert('Error searching for flights. Please try again.');
      }
    });
  }

  clearFilters(): void {
    this.searchForm = {
      provider: 'All',
      cabinType: '',
      timeDeparture: '',
      airportOriginId: null,
      airportDestinationId: null,
      minimumFreeSeats: null,
      durationMinutes: null
    };
    this.flights$ = undefined;
  }

  selectFlight(flight: FlightSearchResponseDTO): void {
    this.selectedFlight = this.selectedFlight?.id === flight.id ? null : flight;
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'on-time':
        return 'status-on-time';
      case 'delayed':
        return 'status-delayed';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return '';
    }
  }

  formatTime(dateString: string): string {
    return new Date(dateString).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  bookFlight(flight: FlightSearchResponseDTO): void {
    console.log('Booking flight:', flight.id);
    // TODO: Implement booking functionality
  }
}
