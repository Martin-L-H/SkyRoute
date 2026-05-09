import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

export interface UserProfile {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  nationality: string;
  passportNumber: string;
  frequentFlyer: {
    airline: string;
    number: string;
  }[];
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  profile: UserProfile = {
    firstName: 'John',
    lastName: 'Doe',
    email: 'john.doe@example.com',
    phone: '+1 (555) 123-4567',
    dateOfBirth: '1990-01-15',
    nationality: 'United States',
    passportNumber: 'US123456789',
    frequentFlyer: [
      { airline: 'American Airlines', number: 'AA123456' },
      { airline: 'United Airlines', number: 'UA789012' }
    ]
  };

  isEditing = false;
  activeSection = 'personal';

  sections = [
    { id: 'personal', name: 'Personal Information', icon: '👤' },
    { id: 'travel', name: 'Travel Preferences', icon: '✈️' },
    { id: 'security', name: 'Security', icon: '🔒' },
    { id: 'notifications', name: 'Notifications', icon: '🔔' }
  ];

  setActiveSection(sectionId: string): void {
    this.activeSection = sectionId;
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
  }

  saveProfile(): void {
    console.log('Saving profile:', this.profile);
    this.isEditing = false;
    // TODO: Implement profile save functionality
  }

  cancelEdit(): void {
    this.isEditing = false;
    // TODO: Reset to original values
  }

  addFrequentFlyer(): void {
    this.profile.frequentFlyer.push({ airline: '', number: '' });
  }

  removeFrequentFlyer(index: number): void {
    this.profile.frequentFlyer.splice(index, 1);
  }
}
