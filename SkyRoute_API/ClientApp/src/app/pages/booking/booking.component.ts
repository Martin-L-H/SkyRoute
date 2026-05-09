import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Booking {
  id: string;
  confirmationCode: string;
  flight: {
    flightNumber: string;
    airline: string;
    from: string;
    to: string;
    departureTime: string;
    arrivalTime: string;
  };
  passengers: number;
  totalPrice: number;
  status: 'confirmed' | 'pending' | 'cancelled';
  bookingDate: string;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent {
  bookings: Booking[] = [
    {
      id: '1',
      confirmationCode: 'ABC123',
      flight: {
        flightNumber: 'AA123',
        airline: 'American Airlines',
        from: 'JFK',
        to: 'LAX',
        departureTime: '2024-01-15T08:00:00',
        arrivalTime: '2024-01-15T11:30:00'
      },
      passengers: 2,
      totalPrice: 598,
      status: 'confirmed',
      bookingDate: '2024-01-01T10:00:00'
    },
    {
      id: '2',
      confirmationCode: 'XYZ789',
      flight: {
        flightNumber: 'UA456',
        airline: 'United Airlines',
        from: 'LAX',
        to: 'ORD',
        departureTime: '2024-01-20T14:00:00',
        arrivalTime: '2024-01-20T20:15:00'
      },
      passengers: 1,
      totalPrice: 379,
      status: 'pending',
      bookingDate: '2024-01-02T15:30:00'
    }
  ];

  selectedBooking: Booking | null = null;

  selectBooking(booking: Booking): void {
    this.selectedBooking = this.selectedBooking?.id === booking.id ? null : booking;
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'confirmed':
        return 'status-confirmed';
      case 'pending':
        return 'status-pending';
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

  cancelBooking(bookingId: string): void {
    console.log('Cancelling booking:', bookingId);
    // TODO: Implement booking cancellation
  }

  modifyBooking(bookingId: string): void {
    console.log('Modifying booking:', bookingId);
    // TODO: Implement booking modification
  }
}
