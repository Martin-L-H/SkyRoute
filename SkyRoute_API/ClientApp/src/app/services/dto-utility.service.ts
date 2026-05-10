import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DtoUtilityService {
  /**
   * Removes null, undefined, and empty string values from an object
   * This prevents Angular's HttpClient from sending empty strings to the backend
   */
  cleanObject<T>(obj: T): Partial<T> {
    const cleaned: any = {};

    for (const key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        const value = obj[key];

        // Only include properties that have meaningful values
        if (value !== null && value !== undefined && value !== '') {
          cleaned[key] = value;
        }
      }
    }

    return cleaned;
  }

  /**
   * Converts a DTO object to query parameters, excluding null/undefined values
   */
  toQueryParams<T>(obj: T): { [key: string]: string | number | boolean } {
    const params: any = {};

    for (const key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        const value = obj[key];

        if (value !== null && value !== undefined && value !== '') {
          params[key] = value;
        }
      }
    }

    return params;
  }
}
