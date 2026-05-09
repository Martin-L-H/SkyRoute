export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  dateOfBirth: string;
  nationality: string;
  passportNumber?: string;
  preferences: {
    language: string;
    currency: string;
    timezone: string;
    notifications: {
      email: boolean;
      sms: boolean;
      push: boolean;
    };
    travelPreferences: {
      seatPreference: 'window' | 'aisle' | 'middle';
      mealPreference: string;
      frequentFlyerPrograms: {
        airline: string;
        number: string;
      }[];
    };
  };
  createdAt: string;
  updatedAt: string;
  lastLoginAt?: string;
  isVerified: boolean;
}

export interface UserProfile {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  nationality: string;
  passportNumber?: string;
  frequentFlyer: {
    airline: string;
    number: string;
  }[];
}
