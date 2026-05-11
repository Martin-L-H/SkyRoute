import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './flight-search.component.html',
})
export class FlightSearchComponent implements OnInit {
  private fb = inject(FormBuilder);
  private flightService = inject(FlightService);

  initData = this.flightService.initData;
  private rawFlights = signal<any[]>([]);
  sortState = signal<{ key: string, dir: 'asc' | 'desc' | null }>({ key: '', dir: null });
  isLoading = signal(false);

  searchForm = this.fb.group({
    CountryOriginId: [null as number | null],
    CityOriginId: [null as number | null],
    AirportOriginId: [null as number | null],
    CountryDestinationId: [null as number | null],
    CityDestinationId: [null as number | null],
    AirportDestinationId: [null as number | null],
    CabinTypeId: [null as number | null],
    Provider: [''],
    TimeDeparture: [''],
    minimumFreeSeats: [1]
  });

  formValues = toSignal(this.searchForm.valueChanges, { initialValue: this.searchForm.value });

  flightResults = computed(() => {
    let list = [...this.rawFlights()];
    const filters = this.formValues();
    const sort = this.sortState();

    let filtered = list.filter(f => {
      return (!filters?.CountryOriginId || f.countryOriginId === filters.CountryOriginId) &&
        (!filters?.CityOriginId || f.cityOriginId === filters.CityOriginId) &&
        (!filters?.AirportOriginId || f.airportOriginId === filters.AirportOriginId) &&
        (!filters?.CountryDestinationId || f.countryDestinationId === filters.CountryDestinationId) &&
        (!filters?.CityDestinationId || f.cityDestinationId === filters.CityDestinationId) &&
        (!filters?.AirportDestinationId || f.airportDestinationId === filters.AirportDestinationId) &&
        (!filters?.CabinTypeId || f.cabinTypeId === filters.CabinTypeId) &&
        (!filters?.minimumFreeSeats || f.seatsFree >= filters.minimumFreeSeats) &&
        (!filters?.TimeDeparture || f.timeDeparture.startsWith(filters.TimeDeparture));
    });

    if (sort.key && sort.dir) {
      filtered.sort((a, b) => {
        const valA = a[sort.key];
        const valB = b[sort.key];
        if (typeof valA === 'string' && typeof valB === 'string') {
          return sort.dir === 'asc' ? valA.localeCompare(valB) : valB.localeCompare(valA);
        }
        if (valA < valB) return sort.dir === 'asc' ? -1 : 1;
        if (valA > valB) return sort.dir === 'asc' ? 1 : -1;
        return 0;
      });
    }
    return filtered;
  });

  originCountryVal = computed(() => this.formValues()?.CountryOriginId);
  originCityVal = computed(() => this.formValues()?.CityOriginId);
  destCountryVal = computed(() => this.formValues()?.CountryDestinationId);
  destCityVal = computed(() => this.formValues()?.CityDestinationId);

  selectedFlight = signal<any | null>(null);
  bookingLoading = signal(false);
  bookingSuccessData = signal<any | null>(null);
  showSuccessContent = signal(false);

  bookingForm = this.fb.group({
    flightId: [null as number | null, Validators.required],
    passengerList: this.fb.array([])
  });

  get passengers() { return this.bookingForm.get('passengerList') as FormArray; }

  // FIX 1: Listen to valueChanges to ensure total price updates on Add/Remove
  bookingTotalPrice = computed(() => {
    const flight = this.selectedFlight();
    const count = this.formValues() ? this.passengers.length : 1;
    return (flight?.priceTotal || 0) * count;
  });

  ngOnInit() {
    this.flightService.getSearchMetadata().subscribe();
    this.onSearch();
  }

  onSearch() {
    this.isLoading.set(true);
    this.flightService.searchFlights(this.searchForm.value).subscribe({
      next: (results) => {
        this.rawFlights.set(results);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  setSort(key: string) {
    const current = this.sortState();
    if (current.key === key) {
      if (current.dir === 'asc') this.sortState.set({ key, dir: 'desc' });
      else if (current.dir === 'desc') this.sortState.set({ key: '', dir: null });
    } else {
      this.sortState.set({ key, dir: 'asc' });
    }
  }

  countries = computed(() => {
    const airports = this.initData()?.airports || [];
    return airports.reduce((acc: any[], curr) => {
      if (!acc.find(item => item.id === curr.countryId)) acc.push({ id: curr.countryId, name: curr.countryName });
      return acc;
    }, []);
  });

  filteredOriginCities = computed(() => {
    const countryId = this.originCountryVal();
    const airports = this.initData()?.airports || [];
    if (!countryId) return [];
    return airports.filter(a => a.countryId === countryId)
      .reduce((acc: any[], curr) => {
        if (!acc.find(item => item.id === curr.cityId)) acc.push({ id: curr.cityId, name: curr.cityName });
        return acc;
      }, []);
  });

  filteredOriginAirports = computed(() => {
    const cityId = this.originCityVal();
    const airports = this.initData()?.airports || [];
    return cityId ? airports.filter(a => a.cityId === cityId) : [];
  });

  filteredDestCities = computed(() => {
    const countryId = this.destCountryVal();
    const airports = this.initData()?.airports || [];
    if (!countryId) return [];
    return airports.filter(a => a.countryId === countryId)
      .reduce((acc: any[], curr) => {
        if (!acc.find(item => item.id === curr.cityId)) acc.push({ id: curr.cityId, name: curr.cityName });
        return acc;
      }, []);
  });

  filteredDestAirports = computed(() => {
    const cityId = this.destCityVal();
    const airports = this.initData()?.airports || [];
    return cityId ? airports.filter(a => a.cityId === cityId) : [];
  });

  goToBooking(flight: any) {
    this.selectedFlight.set(flight);
    this.bookingForm.patchValue({ flightId: flight.id });
    this.passengers.clear();
    this.addPassenger();
  }

  addPassenger() {
    this.passengers.push(this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      documentNumber: ['', Validators.required],
      ispassport: [false]
    }));
    // Force a UI refresh for the computed total
    this.searchForm.patchValue({});
  }

  // FIX 3: Use removeAt(index) to remove the specific passenger clicked
  removePassenger(index: number) {
    if (this.passengers.length > 1) {
      this.passengers.removeAt(index);
      this.searchForm.patchValue({}); // Refresh total
    }
  }

  closeOnBackdrop(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeBooking();
    }
  }

  confirmBooking() {
    if (this.bookingForm.invalid || this.bookingLoading()) return;
    this.bookingLoading.set(true);
    this.flightService.bookFlight(this.bookingForm.value).subscribe({
      next: (res) => {
        this.onSearch();
        this.bookingSuccessData.set(res.data || res);
        this.closeBooking();
        setTimeout(() => this.showSuccessContent.set(true), 50);
      },
      complete: () => this.bookingLoading.set(false)
    });
  }

  closeBooking() { this.selectedFlight.set(null); this.bookingForm.reset(); }
  closeSuccessPopup() { this.showSuccessContent.set(false); this.bookingSuccessData.set(null); }
  formatDuration(min: number) { return `${Math.floor(min / 60)}h ${min % 60}m`; }
}
