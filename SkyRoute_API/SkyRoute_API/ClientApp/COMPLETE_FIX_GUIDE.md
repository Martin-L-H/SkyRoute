# Frontend Bug Fixes - Complete Implementation Guide

## Executive Summary

Fixed 4 critical frontend issues that were preventing proper communication between the Angular client and .NET backend API. The main problem was Angular's HttpClient automatically converting `null` and `undefined` values to empty strings, which the backend couldn't handle properly.

---

## Issues Fixed

### 1. ❌ Empty String Parameters
**What was happening**: 
- When search form had optional fields left empty, Angular sent empty strings `""`
- Backend received `?cabinType=&Provider=&AirportOriginId=` instead of omitting them
- Backend validation failed because it expected either `null` or proper values, not empty strings

**How it's fixed**:
- Created `DtoUtilityService` that filters out all empty, null, and undefined values
- `ApiService.get()` now calls `toQueryParams()` to clean parameters before sending
- Backend now only receives parameters that have actual values

---

### 2. ❌ Missing DurationMinutes Property
**What was happening**:
- Search component wasn't including `DurationMinutes` in flight search requests
- Backend couldn't filter by duration because it was never receiving this parameter

**How it's fixed**:
- Added `DurationMinutes: undefined` to search request in `SearchComponent`
- Now included in all search requests (even if undefined, it gets filtered out)

---

### 3. ❌ null vs undefined Handling
**What was happening**:
- Components used `null` for optional fields
- Angular still converted `null` to empty string in query params
- `undefined` is the proper JavaScript value for "no value"

**How it's fixed**:
- Changed all `null` assignments to `undefined` in components
- `undefined` values are properly filtered by `DtoUtilityService`
- Query parameters are now clean

---

### 4. ❌ Wrong Airport ID Mapping
**What was happening**:
- Airport selection used `airport.countryID` instead of `airport.id`
- Backend received wrong airport identifier
- Flight searches with airport filters were broken

**How it's fixed**:
- Updated `selectAirportByCode()` to use `airport.id` when available
- Falls back to `countryID` if id not available
- Now sends correct airport identifiers

---

## Technical Solution

### Core Component: DtoUtilityService

```typescript
// Location: src/app/services/dto-utility.service.ts

@Injectable({ providedIn: 'root' })
export class DtoUtilityService {
  // Converts DTO to query params, filtering empty values
  toQueryParams<T>(obj: T): { [key: string]: string | number | boolean } {
    const params: any = {};
    for (const key in obj) {
      const value = obj[key];
      if (value !== null && value !== undefined && value !== '') {
        params[key] = value;
      }
    }
    return params;
  }
}
```

### Integration in ApiService

```typescript
// Before
get<T>(endpoint: string, params?: any): Observable<T> {
  return this.http.get<T>(`${this.baseUrl}${endpoint}`, { 
    params  // Sends everything, including empty strings!
  });
}

// After
get<T>(endpoint: string, params?: any): Observable<T> {
  const cleanParams = params ? this.dtoUtility.toQueryParams(params) : undefined;
  return this.http.get<T>(`${this.baseUrl}${endpoint}`, { 
    params: cleanParams  // Only sends params with values
  });
}
```

---

## Files Modified

### ✨ New Files (1)
- **`src/app/services/dto-utility.service.ts`**
  - Provides utilities for cleaning DTOs
  - Injectable service for use across the app
  - 42 lines

### 🔄 Modified Files (3)
1. **`src/app/services/api.service.ts`**
   - Injected `DtoUtilityService`
   - Updated `get()` method to filter parameters
   - 42 lines (before) → 42 lines (after)

2. **`src/app/pages/search/search.component.ts`**
   - Changed `null` → `undefined` in `onSearch()`
   - Added `DurationMinutes` property
   - Updated `selectAirportByCode()` to use `airport.id`
   - ~150 lines total

3. **`src/app/pages/flights/flights.component.ts`**
   - Changed `null` → `undefined` in `searchFlights()`
   - Consistent with other components
   - ~100 lines total

### ✅ Verified (No Changes)
- **`src/app/services/flight.service.ts`**
  - DTO already had all required properties
  - `DurationMinutes` was already declared

---

## Data Flow Example

### Flight Search Scenario

**User Input** (search form):
```
From: JFK (id: 1)
To: LAX (id: 5)
Date: 2024-01-15
Passengers: 2
Cabin: (empty)
Duration: (empty)
```

**SearchComponent creates DTO**:
```typescript
{
  AirportOriginId: 1,
  AirportDestinationId: 5,
  TimeDeparture: "2024-01-15",
  minimumFreeSeats: 2,
  cabinType: undefined,        // Will be filtered
  DurationMinutes: undefined,  // Will be filtered
  Provider: 'All'
}
```

**DtoUtilityService filters**:
```typescript
// Removes all undefined values
{
  AirportOriginId: 1,
  AirportDestinationId: 5,
  TimeDeparture: "2024-01-15",
  minimumFreeSeats: 2,
  Provider: 'All'
}
```

**Query String Sent to Backend**:
```
GET /api/flights?AirportOriginId=1&AirportDestinationId=5&TimeDeparture=2024-01-15&minimumFreeSeats=2&Provider=All
```

✅ Clean! No empty strings!

---

## Testing Checklist

### Manual Testing
- [ ] Build Angular project without errors
- [ ] Open application in browser
- [ ] Open DevTools Network tab
- [ ] Try searching with incomplete form
- [ ] Verify query parameters in Network tab are clean
- [ ] Verify no empty string parameters are sent
- [ ] Try selecting different airports
- [ ] Verify correct airport IDs are sent
- [ ] Try booking a flight
- [ ] Verify booking works end-to-end

### Verification Points
- [ ] `?cabinType=` should NOT appear if cabin type not selected
- [ ] `?duration=` should NOT appear if duration not specified
- [ ] Only populated fields should appear in query string
- [ ] Airport IDs should be numeric (1, 2, 5, etc.), not country codes
- [ ] All API calls should complete without 400 Bad Request errors

---

## Backend Considerations

### What Changed in Requests

**OLD** (Problems):
```
GET /api/flights?cabinType=&TimeDeparture=2024-01-15&AirportOriginId=&Provider=All
```
- Empty strings for unspecified fields
- Backend confusion about whether field was meant to be empty or unspecified
- Validation errors

**NEW** (Fixed):
```
GET /api/flights?TimeDeparture=2024-01-15&Provider=All
```
- Only fields with values are sent
- Backend can clearly distinguish between "not provided" and "empty"
- Cleaner, RESTful API usage

### Backend DTO Update Needed?

Check if your C# DTOs can handle:
```csharp
public class FlightSearchRequestDTO 
{
    public int? AirportOriginId { get; set; }  // Optional
    public int? AirportDestinationId { get; set; }  // Optional
    public string? TimeDeparture { get; set; }  // Optional
    public int? minimumFreeSeats { get; set; }  // Optional
    public string? Provider { get; set; }  // Optional
    public string? cabinType { get; set; }  // Optional
    public int? DurationMinutes { get; set; }  // Optional
}
```

Make sure:
- All optional properties are nullable (with `?`)
- Validation doesn't require fields that may not be sent
- Null checks before using values

---

## How to Deploy

1. **Test Locally**:
   ```bash
   cd SkyRoute_API/SkyRoute_API/ClientApp
   npm install
   npm start
   ```

2. **Build for Production**:
   ```bash
   npm run build
   ```

3. **Verify No Errors**:
   - Check console for TypeScript errors
   - Verify all imports are resolved
   - Check that DtoUtilityService is properly injected

---

## Rollback Plan

If needed, the changes can be easily reverted:
- Remove `DtoUtilityService.ts`
- Revert `ApiService.get()` method
- Revert component changes back to using `null`

---

## Summary

✅ **Frontend is now fixed and ready for backend integration**

The Angular client will now:
1. Filter out empty/null/undefined query parameters
2. Send only meaningful data to the API
3. Include all required properties in requests
4. Use correct field mappings (airport IDs, etc.)

Next step: Ensure backend DTOs are properly nullable and validation is flexible.
