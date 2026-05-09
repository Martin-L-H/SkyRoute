import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  features = [
    {
      icon: '🔍',
      title: 'Smart Search',
      description: 'Advanced search algorithms to find the best flights at the best prices.'
    },
    {
      icon: '💰',
      title: 'Best Prices',
      description: 'Compare prices from multiple airlines to get the most competitive rates.'
    },
    {
      icon: '📱',
      title: 'Easy Booking',
      description: 'Simple and intuitive booking process with secure payment options.'
    },
    {
      icon: '🌍',
      title: 'Worldwide Coverage',
      description: 'Access to flights from thousands of airlines worldwide.'
    },
    {
      icon: '🎯',
      title: 'Personalized Results',
      description: 'Get recommendations based on your preferences and travel history.'
    },
    {
      icon: '📧',
      title: 'Real-time Updates',
      description: 'Instant notifications about flight status and booking changes.'
    }
  ];

  popularDestinations = [
    { city: 'New York', country: 'USA', price: '$299', image: '🗽' },
    { city: 'London', country: 'UK', price: '$349', image: '🎡' },
    { city: 'Paris', country: 'France', price: '$379', image: '🗼' },
    { city: 'Tokyo', country: 'Japan', price: '$599', image: '🗾' }
  ];
}
