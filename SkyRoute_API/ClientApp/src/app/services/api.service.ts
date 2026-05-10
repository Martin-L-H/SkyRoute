import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DtoUtilityService } from './dto-utility.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = 'https://localhost:7259/api';
  private readonly httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private dtoUtility: DtoUtilityService
  ) {}

  // Generic GET method
  get<T>(endpoint: string, params?: any): Observable<T> {
    // Clean params to remove null/undefined/empty string values
    const cleanParams = params ? this.dtoUtility.toQueryParams(params) : undefined;

    return this.http.get<T>(`${this.baseUrl}${endpoint}`, { 
      params: cleanParams,
      ...this.httpOptions 
    });
  }

  // Generic POST method
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${endpoint}`, data, this.httpOptions);
  }

  // Generic PUT method
  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${endpoint}`, data, this.httpOptions);
  }

  // Generic DELETE method
  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${endpoint}`, this.httpOptions);
  }
}
