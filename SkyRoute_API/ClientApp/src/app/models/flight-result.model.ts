import { BaseResponse } from './search-init.model';

export interface FlightResult {
  id: number;
  flightNumber: string;
  timeDeparture: string;      // Received as ISO string
  timeArrival: string;        // Received as ISO string
  baseFare: number;
  cabinType: number;          // Enum value from backend
  providerName: string;

  // Origin details
  codeIATAOrigin: string;
  airportOriginName: string;
  airportOriginId: number;
  cityOriginName: string;
  cityOriginId: number;
  countryOriginName: string;
  countryOriginId: number;

  // Destination details
  codeIATADestination: string;
  airportDestinationName: string;
  airportDestinationId: number;
  cityDestinationId: number;
  cityDestinationName: string;
  countryDestinationName: string;
  countryDestinationId: number;

  // Pricing and Availability
  pricePerPerson: number;
  priceTotal: number;
  seatsTotal: number;
  seatsFree: number;
  durationMinutes: number;
}

// Wrapper for the full API response - now compatible!
export type FlightSearchResponse = BaseResponse<FlightResult[]>;
