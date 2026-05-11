import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { SearchResponse, SearchInitializationData } from '../models/search-init.model';
import { FlightResult, FlightSearchResponse } from '../models/flight-result.model';

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  private http = inject(HttpClient);
  private readonly API_BASE = 'https://localhost:7259/api';

  initData = signal<SearchInitializationData | null>(null);
  flightResults = signal<FlightResult[]>([]);
  lastSearchCriteria = signal<any>(null);

  getSearchMetadata(): Observable<SearchInitializationData> {
    return this.http.get<SearchResponse>(`${this.API_BASE}/SearchMetadata`).pipe(
      map(response => response.data),
      tap(data => this.initData.set(data))
    );
  }

  searchFlights(criteria: any): Observable<FlightResult[]> {
    this.lastSearchCriteria.set(criteria);
    let params = new HttpParams();
    Object.keys(criteria).forEach(key => {
      const value = criteria[key];
      if (value !== null && value !== undefined && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    if (!params.has('Provider') || criteria.Provider === '') {
      params = params.set('Provider', 'All');
    }

    return this.http.get<FlightSearchResponse>(`${this.API_BASE}/Flight`, { params }).pipe(
      map(response => response.data),
      tap(results => this.flightResults.set(results))
    );
  }

  bookFlight(bookingData: any): Observable<any> {
    return this.http.post(`${this.API_BASE}/Booking`, bookingData);
  }

  refreshResults() {
    const last = this.lastSearchCriteria();
    if (last) {
      this.searchFlights(last).subscribe();
    }
  }
}
