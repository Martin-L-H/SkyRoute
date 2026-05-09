export interface Passenger {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: 'male' | 'female' | 'other';
  nationality: string;
  passportNumber?: string;
  email: string;
  phone: string;
  frequentFlyerNumber?: string;
  specialRequests?: string[];
}

export interface Booking {
  id: string;
  confirmationCode: string;
  bookingDate: string;
  status: 'confirmed' | 'pending' | 'cancelled' | 'completed';
  flights: {
    flight: any;
    passengers: Passenger[];
    seatAssignments: {
      passengerId: string;
      seat: string;
      class: 'economy' | 'business' | 'first';
    }[];
  }[];
  totalPrice: number;
  currency: string;
  paymentStatus: 'paid' | 'pending' | 'refunded';
  contactInfo: {
    email: string;
    phone: string;
  };
  additionalServices: {
    travelInsurance: boolean;
    extraBaggage: number;
    mealPreference: string;
  };
}

export interface BookingRequest {
  flightIds: string[];
  passengers: Passenger[];
  contactInfo: {
    email: string;
    phone: string;
  };
  additionalServices: {
    travelInsurance: boolean;
    extraBaggage: number;
    mealPreference: string;
  };
  paymentMethod: {
    type: 'credit-card' | 'debit-card' | 'paypal';
    details: any;
  };
}
