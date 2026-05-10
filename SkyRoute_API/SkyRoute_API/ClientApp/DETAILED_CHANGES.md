# Frontend Detailed Changes

## 1. New Service: DtoUtilityService

**Location**: `src/app/services/dto-utility.service.ts`

**Purpose**: Utility service to clean DTO objects by removing null, undefined, and empty string values before sending to backend.

**Key Methods**:
- `cleanObject<T>()`: Removes null/undefined/empty values from objects
- `toQueryParams<T>()`: Converts DTO to query parameters, filtering out empty values

**Usage**: 
- Used by `ApiService` to clean query parameters
- Ensures only meaningful values are sent to the backend

---

## 2. Updated: ApiService

**Location**: `src/app/services/api.service.ts`

**Before**:
```typescript
get<T>(endpoint: string, params?: any): Observable<T> {
  return this.http.get<T>(`${this.baseUrl}${endpoint}`, { 
    params,  // Sends ALL params including null/undefined
    ...this.httpOptions 
  });
}
```

**After**:
```typescript
get<T>(endpoint: string, params?: any): Observable<T> {
  // Clean params to remove null/undefined/empty string values
  const cleanParams = params ? this.dtoUtility.toQueryParams(params) : undefined;

  return this.http.get<T>(`${this.baseUrl}${endpoint}`, { 
    params: cleanParams,  // Only sends params with values
    ...this.httpOptions 
  });
}
```

**Changes**:
- Injected `DtoUtilityService`
- Calls `toQueryParams()` to filter parameters
- Result: Only meaningful parameters are sent to backend

---

## 3. Updated: SearchComponent

**Location**: `src/app/pages/search/search.component.ts`

**Method**: `onSearch()`

**Before**:
```typescript
const searchRequest: FlightSearchRequestDTO = {
  cabinType: this.searchForm.class ? this.searchForm.class.charAt(0).toUpperCase() + this.searchForm.class.slice(1) as any : null,
  TimeDeparture: this.searchForm.departureDate || null,
  AirportOriginId: this.searchForm.fromId || null,
  AirportDestinationId: this.searchForm.toId || null,
  minimumFreeSeats: this.searchForm.passengers || null,
  Provider: 'All'
  // DurationMinutes was missing!
};
```

**After**:
```typescript
const searchRequest: FlightSearchRequestDTO = {
  cabinType: this.searchForm.class ? this.searchForm.class.charAt(0).toUpperCase() + this.searchForm.class.slice(1) as any : undefined,
  TimeDeparture: this.searchForm.departureDate || undefined,
  AirportOriginId: this.searchForm.fromId || undefined,
  AirportDestinationId: this.searchForm.toId || undefined,
  minimumFreeSeats: this.searchForm.passengers || undefined,
  DurationMinutes: undefined,  // Added missing property
  Provider: 'All'
};
```

**Changes**:
- Changed all `null` to `undefined` (better for HttpClient filtering)
- Added missing `DurationMinutes` property

---

**Method**: `selectAirportByCode()`

**Before**:
```typescript
selectAirportByCode(airport: any, field: 'from' | 'to'): void {
  if (field === 'from') {
    this.searchForm.from = airport.codeIATA;
    this.searchForm.fromId = airport.countryID; // WRONG! Using countryID as airport ID
  } else {
    this.searchForm.to = airport.codeIATA;
    this.searchForm.toId = airport.countryID; // WRONG! Using countryID as airport ID
  }
}
```

**After**:
```typescript
selectAirportByCode(airport: any, field: 'from' | 'to'): void {
  if (field === 'from') {
    this.searchForm.from = airport.codeIATA;
    // Use the airport's ID if available, otherwise use codeIATA as fallback
    this.searchForm.fromId = airport.id || airport.countryID;
  } else {
    this.searchForm.to = airport.codeIATA;
    // Use the airport's ID if available, otherwise use codeIATA as fallback
    this.searchForm.toId = airport.id || airport.countryID;
  }
}
```

**Changes**:
- Now uses `airport.id` (correct airport identifier)
- Falls back to `countryID` if id is not available
- Better comment explaining the logic

---

## 4. Updated: FlightsComponent

**Location**: `src/app/pages/flights/flights.component.ts`

**Method**: `searchFlights()`

**Before**:
```typescript
const searchRequest: FlightSearchRequestDTO = {
  Provider: this.searchForm.provider || null,
  cabinType: this.searchForm.cabinType ? this.searchForm.cabinType as any : null,
  TimeDeparture: this.searchForm.timeDeparture || null,
  AirportOriginId: this.searchForm.airportOriginId || null,
  AirportDestinationId: this.searchForm.airportDestinationId || null,
  minimumFreeSeats: this.searchForm.minimumFreeSeats || null,
  DurationMinutes: this.searchForm.durationMinutes || null
};
```

**After**:
```typescript
const searchRequest: FlightSearchRequestDTO = {
  Provider: this.searchForm.provider || undefined,
  cabinType: this.searchForm.cabinType ? this.searchForm.cabinType as any : undefined,
  TimeDeparture: this.searchForm.timeDeparture || undefined,
  AirportOriginId: this.searchForm.airportOriginId || undefined,
  AirportDestinationId: this.searchForm.airportDestinationId || undefined,
  minimumFreeSeats: this.searchForm.minimumFreeSeats || undefined,
  DurationMinutes: this.searchForm.durationMinutes || undefined
};
```

**Changes**:
- Changed all `null` to `undefined`
- Consistent with other components

---

## 5. Verified: FlightService

**Location**: `src/app/services/flight.service.ts`

**Status**: ✅ No changes needed - already correct

**DTO Definition**:
```typescript
export interface FlightSearchRequestDTO {
  cabinType?: 'Economy' | 'Premium' | 'Business' | 'First' | null;
  TimeDeparture?: string | null;
  AirportOriginId?: number | null;
  AirportDestinationId?: number | null;
  minimumFreeSeats?: number | null;
  Provider?: string | null;
  DurationMinutes?: number | null;  // ✅ Already present
}
```

---

## Impact Summary

### Query Parameter Behavior

**Example Search Request**:
```typescript
{
  cabinType: undefined,
  TimeDeparture: "2024-01-15",
  AirportOriginId: undefined,
  AirportDestinationId: 5,
  minimumFreeSeats: 2,
  DurationMinutes: undefined,
  Provider: "All"
}
```

**Before Fix** (Backend receives):
```
Query: ?cabinType=&TimeDeparture=2024-01-15&AirportOriginId=&AirportDestinationId=5&minimumFreeSeats=2&DurationMinutes=&Provider=All
```

**After Fix** (Backend receives):
```
Query: ?TimeDeparture=2024-01-15&AirportDestinationId=5&minimumFreeSeats=2&Provider=All
```

Only meaningful parameters are sent! ✅
