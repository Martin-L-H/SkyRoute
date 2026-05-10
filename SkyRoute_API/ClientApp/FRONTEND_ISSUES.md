# Frontend Issues Analysis

## Issues Found:

### 1. **Empty String Parameters in HTTP Requests**
   - **Problem**: Angular's HttpClient sends empty string parameters for null/undefined values
   - **Location**: `flight.service.ts` and `flights.component.ts`
   - **Impact**: Backend receives empty strings instead of null, causing validation issues

### 2. **Missing DurationMinutes in Search Request**
   - **Problem**: `DurationMinutes` is never set in the search request from `search.component.ts`
   - **Location**: `search.component.ts` line 60
   - **Impact**: Backend doesn't receive duration filter

### 3. **Incorrect Airport ID Mapping**
   - **Problem**: Using `countryID` as airport ID instead of actual airport ID
   - **Location**: `search.component.ts` line 108-112
   - **Impact**: Wrong airport filter sent to backend

### 4. **Model Mismatch**
   - **Problem**: `FlightSearchRequestDTO` interface doesn't match backend DTO property names
   - **Location**: `flight.service.ts` line 7-13
   - **Expected**: All properties should match C# backend exactly (camelCase vs PascalCase)

### 5. **API Parameter Handling**
   - **Problem**: Query parameters with null/undefined values are sent as empty strings
   - **Location**: `api.service.ts` line 17
   - **Fix**: Filter out null/undefined parameters before sending

### 6. **Form Submission Logic**
   - **Problem**: Not properly constructing DTO before sending
   - **Location**: `search.component.ts` and `flights.component.ts`
   - **Fix**: Only include properties that have values

## Solutions to Implement:

1. Create a utility function to clean/filter DTO objects
2. Update all service methods to filter null values
3. Fix airport ID mapping
4. Update component forms to properly construct requests
5. Add DurationMinutes to search requests
