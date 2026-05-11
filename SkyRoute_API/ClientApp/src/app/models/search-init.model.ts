export interface AirportSearchResponseDTO {
  id: number;
  name: string;
  codeIATA: string;
  cityName: string;
  cityId: number;
  countryName: string;
  countryId: number;
}

export interface EnumDisplayDTO {
  id: number;
  name: string;
}

export interface SearchInitializationData {
  airports: AirportSearchResponseDTO[];
  cabinTypes: EnumDisplayDTO[];
  providers: string[];
}

// The generic base that allows 'data' to be flexible
export interface BaseResponse<T> {
  data: T;
  success?: boolean;
  message?: string;
}

// SearchResponse now specifically uses the initialization data type
export type SearchResponse = BaseResponse<SearchInitializationData>;
