export interface Airport {
  id: number;
  name: string;
  codeIATA: string;
  cityName: string;
  cityId: number;
  countryName: string;
  countryID: number;
}
export interface CabinTypeLookup {
  id: number;
  name: string;
}
export interface SearchInitializationData {
  airports: Airport[];
  cabinTypes: CabinTypeLookup[];
  providers: string[];
}
export interface ServiceResponse<T> {
  data: T;
  success: boolean;
  message: string;
}
export type SearchInitResponse = ServiceResponse<SearchInitializationData>;
