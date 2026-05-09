export interface Airport {
  code: string;
  name: string;
  city: string;
  country: string;
  timezone: string;
}

export interface Flight {
  id: string;
  flightNumber: string;
  airline: string;
  from: Airport;
  to: Airport;
  departureTime: string;
  arrivalTime: string;
  duration: number;
  price: number;
  currency: string;
  availableSeats: number;
  aircraft: string;
  status: 'on-time' | 'delayed' | 'cancelled' | 'boarding' | 'departed' | 'arrived';
  stops: number;
  layover?: {
    airport: Airport;
    duration: number;
  }[];
}

export interface FlightSearchRequest {
  from: string;
  to: string;
  departureDate: string;
  returnDate?: string;
  passengers: number;
  tripType: 'oneway' | 'roundtrip';
  class: 'economy' | 'business' | 'first';
  directOnly?: boolean;
  maxPrice?: number;
  airlines?: string[];
}

export interface FlightSearchResponse {
  flights: Flight[];
  totalResults: number;
  searchId: string;
  filters: {
    airlines: string[];
    priceRange: {
      min: number;
      max: number;
    };
    durationRange: {
      min: number;
      max: number;
    };
  };
}
