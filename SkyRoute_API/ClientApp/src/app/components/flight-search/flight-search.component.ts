import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './flight-search.component.html',
  styleUrl: './flight-search.component.css'
})
export class FlightSearchComponent implements OnInit {
  private fb = inject(FormBuilder);
  private flightService = inject(FlightService);

  initData = this.flightService.initData;
  flightResults = this.flightService.flightResults;

  selectedFlight = signal<any | null>(null);
  bookingLoading = signal(false);
  errorPopupMessage = signal<string | null>(null);
  showErrorContent = signal(false);
  bookingSuccessData = signal<any | null>(null);
  showSuccessContent = signal(false);

  searchForm = this.fb.group({
    CountryOriginId: [null as number | null],
    CityOriginId: [null as number | null],
    AirportOriginId: [null as number | null, Validators.required],
    CountryDestinationId: [null as number | null],
    CityDestinationId: [null as number | null],
    AirportDestinationId: [null as number | null, Validators.required],
    CabinTypeId: [null as number | null, Validators.required],
    Provider: [''],
    TimeDeparture: ['', Validators.required],
    minimumFreeSeats: [1, [Validators.required, Validators.min(1)]]
  });

  bookingForm = this.fb.group({
    flightId: [null as number | null, Validators.required],
    passengerList: this.fb.array([])
  });

  ngOnInit() {
    this.flightService.getSearchMetadata().subscribe();
  }

  get passengers() {
    return this.bookingForm.get('passengerList') as FormArray;
  }

  // Cascading computed signals
  countries = computed(() => this.initData()?.countries || []);
  filteredOriginCities = computed(() => this.initData()?.cities.filter(c => c.countryId === this.searchForm.get('CountryOriginId')?.value) || []);
  filteredOriginAirports = computed(() => this.initData()?.airports.filter(a => a.cityId === this.searchForm.get('CityOriginId')?.value) || []);
  filteredDestCities = computed(() => this.initData()?.cities.filter(c => c.countryId === this.searchForm.get('CountryDestinationId')?.value) || []);
  filteredDestAirports = computed(() => this.initData()?.airports.filter(a => a.cityId === this.searchForm.get('CityDestinationId')?.value) || []);

  bookingTotalPrice = computed(() => {
    const flight = this.selectedFlight();
    return flight ? flight.priceTotal * this.passengers.length : 0;
  });

  onSearch() {
    if (this.searchForm.valid) {
      this.flightService.searchFlights(this.searchForm.value).subscribe();
    }
  }

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
  }

  removePassenger(index: number) { this.passengers.removeAt(index); }

  confirmBooking() {
    if (this.bookingForm.invalid || this.bookingLoading()) return;

    this.bookingLoading.set(true);
    this.flightService.bookFlight(this.bookingForm.value).subscribe({
      next: (res) => {
        // 1. Refresh seat counts from DB
        this.flightService.refreshResults();

        // 2. Set the custom data (check if res has .data property)
        const finalData = res.data ? res.data : res;
        this.bookingSuccessData.set(finalData);

        // 3. Close the input form and open the fancy popup
        this.closeBooking();
        setTimeout(() => this.showSuccessContent.set(true), 50);
      },
      error: (err) => {
        this.errorPopupMessage.set(err.error?.message || 'Booking failed');
        this.showErrorContent.set(true);
      },
      complete: () => this.bookingLoading.set(false)
    });
  }

  closeBooking() { this.selectedFlight.set(null); this.bookingForm.reset(); }
  closeErrorPopup() { this.showErrorContent.set(false); this.errorPopupMessage.set(null); }
  closeSuccessPopup() { this.showSuccessContent.set(false); this.bookingSuccessData.set(null); }

  formatDuration(minutes: number): string {
    return `${Math.floor(minutes / 60)}h ${minutes % 60}m`;
  }
}
