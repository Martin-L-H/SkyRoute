import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// Backend DTO interfaces matching the C# DTOs
export interface FlightSearchRequestDTO {
  cabinType?: 'Economy' | 'Premium' | 'Business' | 'First' | null;  // lowercase c matches backend
  TimeDeparture?: string | null;  // Capital T matches backend
  AirportOriginId?: number | null;  // Capital A matches backend
  AirportDestinationId?: number | null;  // Capital A matches backend
  minimumFreeSeats?: number | null;  // lowercase m matches backend
  Provider?: string | null;  // Capital P matches backend
  DurationMinutes?: number | null;  // Capital D matches backend
}

export interface FlightSearchResponseDTO {
  id: number;
  flightNumber: string;
  timeDeparture: string;
  timeArrival: string;
  baseFare: number;
  cabinType: string;
  providerName: string;
  codeIATAOrigin: string;
  cityOrigin: string;
  countryOrigin: string;
  codeIATADestination: string;
  cityDestination: string;
  countryDestination: string;
  pricePerPerson: number;
  priceTotal: number;
  seatsTotal: number;
  seatsFree: number;
  durationMinutes: number;
}

export interface AirportSearchRequestDTO {
  codeIATA?: string;
  cityName?: string;
  countryName?: string;
}

export interface AirportSearchResponseDTO {
  name: string;
  codeIATA: string;
  cityName: string;
  countryName: string;
  countryID: number;
}

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  constructor(private apiService: ApiService) {}

  searchFlights(searchRequest: FlightSearchRequestDTO): Observable<FlightSearchResponseDTO[]> {
    return this.apiService.get<FlightSearchResponseDTO[]>('/flights', searchRequest);
  }

  getAirports(searchRequest: AirportSearchRequestDTO): Observable<AirportSearchResponseDTO[]> {
    return this.apiService.get<AirportSearchResponseDTO[]>('/airports', searchRequest);
  }

  getAirportByIATA(iataCode: string): Observable<AirportSearchResponseDTO[]> {
    return this.apiService.get<AirportSearchResponseDTO[]>(`/airports/iata`, { codeIATA: iataCode });
  }
}
