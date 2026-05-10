# Frontend Fix - Visual Guide

## Problem: HttpClient Empty Strings

### The Problem Visualized

```
┌─────────────────────────────────────────────────────────────────┐
│                    BEFORE THE FIX                               │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  User fills search form:                                         │
│  ┌──────────────────────────────────────┐                       │
│  │ From: JFK                            │                       │
│  │ To: [empty]                          │                       │
│  │ Date: 2024-01-15                     │                       │
│  │ Cabin: [empty]                       │                       │
│  │ Passengers: 1                        │                       │
│  └──────────────────────────────────────┘                       │
│                      ↓                                            │
│  Angular Component creates DTO:                                  │
│  {                                                                │
│    AirportDestinationId: null,                                  │
│    TimeDeparture: "2024-01-15",                                 │
│    cabinType: null,                                             │
│    minimumFreeSeats: 1,                                         │
│    DurationMinutes: null                                        │
│  }                                                               │
│                      ↓                                            │
│  ❌ HttpClient converts to query string:                         │
│  ?AirportDestinationId=                                         │
│  &TimeDeparture=2024-01-15                                      │
│  &cabinType=                                                    │
│  &minimumFreeSeats=1                                            │
│  &DurationMinutes=                                              │
│                      ↓                                            │
│  💥 Backend receives empty strings:                              │
│  {                                                                │
│    AirportDestinationId: "",  // ❌ Expected null/number        │
│    TimeDeparture: "2024-01-15",                                 │
│    cabinType: "",  // ❌ Expected null/string value             │
│    minimumFreeSeats: 1,                                         │
│    DurationMinutes: ""  // ❌ Expected null/number              │
│  }                                                               │
│                      ↓                                            │
│  🚫 Validation fails!                                            │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## The Solution: DtoUtilityService

```
┌─────────────────────────────────────────────────────────────────┐
│                    AFTER THE FIX                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  User fills search form:                                         │
│  ┌──────────────────────────────────────┐                       │
│  │ From: JFK                            │                       │
│  │ To: [empty]                          │                       │
│  │ Date: 2024-01-15                     │                       │
│  │ Cabin: [empty]                       │                       │
│  │ Passengers: 1                        │                       │
│  └──────────────────────────────────────┘                       │
│                      ↓                                            │
│  Angular Component creates DTO:                                  │
│  {                                                                │
│    AirportDestinationId: undefined,  // Changed null → undefined│
│    TimeDeparture: "2024-01-15",                                 │
│    cabinType: undefined,  // Changed null → undefined           │
│    minimumFreeSeats: 1,                                         │
│    DurationMinutes: undefined  // Added missing property         │
│  }                                                               │
│                      ↓                                            │
│  ✅ DtoUtilityService filters:                                   │
│  toQueryParams(obj) → {                                         │
│    // Removes all undefined values!                             │
│    TimeDeparture: "2024-01-15",                                 │
│    minimumFreeSeats: 1                                          │
│  }                                                               │
│                      ↓                                            │
│  ✅ ApiService sends clean query:                                │
│  ?TimeDeparture=2024-01-15                                      │
│  &minimumFreeSeats=1                                            │
│                      ↓                                            │
│  ✅ Backend receives clean data:                                 │
│  {                                                                │
│    AirportDestinationId: null,  // ✅ Not provided              │
│    TimeDeparture: "2024-01-15",                                 │
│    cabinType: null,  // ✅ Not provided                         │
│    minimumFreeSeats: 1,                                         │
│    DurationMinutes: null  // ✅ Not provided                    │
│  }                                                               │
│                      ↓                                            │
│  ✅ Validation passes!                                           │
│  ✅ Search executes successfully!                               │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## Architecture Diagram

```
User Interface (Angular Component)
           │
           │ User Input
           ↓
┌──────────────────────────────────────┐
│   SearchComponent                    │
│   ├─ searchForm (UI state)          │
│   ├─ onSearch() {                   │
│   │    creates DTO with undefined   │
│   │  }                              │
└────────────┬─────────────────────────┘
             │ Passes DTO to Service
             ↓
┌──────────────────────────────────────┐
│   FlightService                      │
│   ├─ searchFlights(requestDTO)      │
│   └─ calls ApiService.get()         │
└────────────┬─────────────────────────┘
             │ Passes params to ApiService
             ↓
┌──────────────────────────────────────┐
│   ApiService                         │
│   ├─ get(endpoint, params)          │
│   ├─ cleanParams = DtoUtility       │
│   │    .toQueryParams(params)       │
│   └─ http.get(endpoint, cleanParams)│
└────────────┬─────────────────────────┘
             │ FILTERS undefined values
             ↓
┌──────────────────────────────────────┐
│   DtoUtilityService                  │
│   ├─ toQueryParams(obj)             │
│   ├─ Removes null/undefined/""      │
│   └─ Returns filtered object        │
└────────────┬─────────────────────────┘
             │ Only params with values
             ↓
┌──────────────────────────────────────┐
│   HttpClient                         │
│   └─ Sends clean query string       │
└────────────┬─────────────────────────┘
             │ Clean params only
             ↓
┌──────────────────────────────────────┐
│   Backend API                        │
│   ├─ Receives clean request         │
│   ├─ Validation passes              │
│   └─ Returns results                │
└──────────────────────────────────────┘
```

---

## Component Changes Summary

### SearchComponent Changes

```typescript
// BEFORE ❌
onSearch() {
  const searchRequest = {
    cabinType: this.class ? ... : null,  // ❌ null
    TimeDeparture: this.departureDate || null,  // ❌ null
    // ❌ DurationMinutes missing
  };
}

// AFTER ✅
onSearch() {
  const searchRequest = {
    cabinType: this.class ? ... : undefined,  // ✅ undefined
    TimeDeparture: this.departureDate || undefined,  // ✅ undefined
    DurationMinutes: undefined,  // ✅ Added
  };
}
```

### Airport Selection Changes

```typescript
// BEFORE ❌
selectAirportByCode(airport: any) {
  this.searchForm.fromId = airport.countryID;  // ❌ Wrong ID type
}

// AFTER ✅
selectAirportByCode(airport: any) {
  this.searchForm.fromId = airport.id || airport.countryID;  // ✅ Correct
}
```

---

## Testing Verification Checklist

### Browser DevTools Network Tab

When searching, you should see:

✅ **Clean Query String**:
```
https://localhost:7259/api/flights?TimeDeparture=2024-01-15&minimumFreeSeats=1&Provider=All
```

❌ **NOT This** (old buggy version):
```
https://localhost:7259/api/flights?cabinType=&TimeDeparture=2024-01-15&cabinType=&minimumFreeSeats=1&Provider=All
```

---

## Performance Impact

| Aspect | Before | After | Impact |
|--------|--------|-------|--------|
| Query String Length | Longer (empty params) | Shorter | 📉 Better |
| Backend Processing | More validation errors | Less errors | 📈 Better |
| Network Payload | Larger | Smaller | 📉 Better |
| API Success Rate | Lower | Higher | 📈 Better |

---

## Success Indicators

After the fix, you should see:

✅ Search works with partial form inputs
✅ No empty string parameters in Network tab
✅ Backend returns flight results
✅ No 400 Bad Request errors
✅ Airport selection maps correctly
✅ Duration filter works when specified
