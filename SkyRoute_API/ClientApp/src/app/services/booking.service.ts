import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// Backend DTO interfaces matching C# DTOs
export interface BookingRequestDTO {
  flightId: number;
  providerName: string;
  passengerCount: number;
  passengerList: PassengerRequestDTO[];
}

export interface BookingResponseDTO {
  referenceCode: string;
  flightDepartureTime: string;
  flightArrivalTime: string;
  durationMinutes: number;
  provider: string;
  cabinType: string;
  passengerCount: number;
  flightNumber: string;
  priceTotal: number;
  airportOriginName: string;
  airportOriginCode: string;
  cityOrigin: string;
  countryOrigin: string;
  airportDestinyName: string;
  airportDestinyCode: string;
  cityDestiny: string;
  countryDestiny: string;
}

export interface PassengerRequestDTO {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  nationality: string;
  passportNumber?: string;
  email: string;
  phone: string;
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  constructor(private apiService: ApiService) {}

  createBooking(bookingRequest: BookingRequestDTO): Observable<BookingResponseDTO> {
    return this.apiService.post<BookingResponseDTO>('/bookings', bookingRequest);
  }

  getBookingById(bookingId: string): Observable<BookingResponseDTO> {
    return this.apiService.get<BookingResponseDTO>(`/bookings/${bookingId}`);
  }

  getUserBookings(userId: string): Observable<BookingResponseDTO[]> {
    return this.apiService.get<BookingResponseDTO[]>(`/bookings/user/${userId}`);
  }
}
