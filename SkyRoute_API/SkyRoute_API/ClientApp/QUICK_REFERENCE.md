# Frontend Issues - Quick Reference

## Problems Fixed ✅

| Issue | Component | Fix | Status |
|-------|-----------|-----|--------|
| Empty string parameters | ApiService | Filter null/undefined in get() | ✅ Fixed |
| Missing DurationMinutes | SearchComponent | Added to search request | ✅ Fixed |
| Null instead of undefined | SearchComponent, FlightsComponent | Changed all null → undefined | ✅ Fixed |
| Wrong airport ID mapping | SearchComponent | Use airport.id instead of countryID | ✅ Fixed |

## Files Changed

### New Files (1):
- `src/app/services/dto-utility.service.ts` - DTO cleaning utility

### Modified Files (3):
- `src/app/services/api.service.ts` - Parameter filtering
- `src/app/pages/search/search.component.ts` - Search form improvements
- `src/app/pages/flights/flights.component.ts` - Flight search improvements

### Verified Files (1):
- `src/app/services/flight.service.ts` - Already correct ✅

## The Core Fix

**DtoUtilityService** filters out null/undefined/empty values from query parameters:

```typescript
// Only parameters with values are sent to backend
toQueryParams({ cabinType: undefined, timeDeparture: "2024-01-15", ... })
// Result: { timeDeparture: "2024-01-15" }
```

## How to Test

1. Open Angular app
2. Try searching for flights with partial form (leave some fields empty)
3. Check browser DevTools Network tab
4. Query parameters should only show filled-in values
5. Backend should no longer receive empty strings

## What Happens Now

**Before**: Backend received empty strings for optional fields
**After**: Backend receives only the fields that have values

Example:
- Before: `?cabinType=&TimeDeparture=2024-01-15&Provider=`
- After: `?TimeDeparture=2024-01-15`

## Next: Backend DTO Handling

Once frontend is working:
- Check backend C# DTOs are nullable where needed
- Ensure validation only requires necessary fields
- Handle cases where optional parameters are missing
