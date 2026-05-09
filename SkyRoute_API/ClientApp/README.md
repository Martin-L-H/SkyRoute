# SkyRoute - Flight Travel Aggregator Frontend

A modern, responsive Angular application for searching, comparing, and booking flights. This frontend provides an intuitive user interface for the SkyRoute flight travel aggregator platform.

## Features

- **Flight Search**: Advanced search functionality with filters for airlines, price, duration, and more
- **Booking Management**: View, modify, and cancel flight bookings
- **User Profile**: Manage personal information and travel preferences
- **Responsive Design**: Optimized for desktop, tablet, and mobile devices
- **Modern UI**: Clean, professional interface with smooth animations and transitions

## Technology Stack

- **Angular 18**: Modern Angular framework with standalone components
- **TypeScript**: Type-safe JavaScript for better development experience
- **SCSS**: Advanced CSS with variables and mixins for consistent styling
- **RxJS**: Reactive programming for handling asynchronous operations
- **Angular Router**: Client-side routing for single-page application navigation

## Project Structure

```
src/
├── app/
│   ├── components/          # Reusable components
│   ├── pages/              # Page components (home, search, flights, booking, profile)
│   ├── services/           # API services for data management
│   ├── models/             # TypeScript interfaces and types
│   ├── shared/             # Shared components and utilities
│   ├── app.component.ts    # Root component
│   ├── app.config.ts       # Application configuration
│   └── app.routes.ts       # Route definitions
├── assets/                 # Static assets (images, icons)
├── styles.scss             # Global styles and CSS variables
└── index.html              # Main HTML template
```

## Getting Started

### Prerequisites

- Node.js (version 18 or higher)
- npm (version 9 or higher)

### Installation

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm run start
```

3. Open your browser and navigate to `http://localhost:4200`

### Build for Production

```bash
npm run build
```

The built files will be output to the `dist/` directory.

## Available Scripts

- `npm run start` - Start the development server
- `npm run build` - Build the application for production
- `npm run watch` - Build and watch for changes during development
- `npm run test` - Run unit tests
- `npm run serve:ssr` - Serve the application with server-side rendering

## Configuration

### Environment Variables

Create environment files in the `src/environments/` directory:

- `environment.ts` - Development environment
- `environment.production.ts` - Production environment

### API Configuration

Update the API endpoints in the service files located in `src/app/services/`:

- `flight.service.ts` - Flight search and management
- `booking.service.ts` - Booking operations
- Additional services as needed

## Styling

The application uses a comprehensive SCSS-based styling system with:

- **CSS Variables**: Consistent theming and easy customization
- **Responsive Design**: Mobile-first approach with breakpoints
- **Component Styles**: Scoped styles for each component
- **Utility Classes**: Reusable styling utilities

### Customization

Update the CSS variables in `src/styles.scss` to customize the theme:

```scss
:root {
  --primary-color: #3b82f6;
  --secondary-color: #64748b;
  --success-color: #10b981;
  // ... more variables
}
```

## Development Guidelines

### Code Style

- Use TypeScript for all new code
- Follow Angular best practices and style guide
- Implement responsive design principles
- Write clean, maintainable, and well-documented code

### Component Structure

- Use standalone components (Angular 18+)
- Implement proper input/output properties
- Handle loading states and error scenarios
- Provide meaningful accessibility attributes

### API Integration

- Use Angular HttpClient for API calls
- Implement proper error handling
- Use RxJS operators for data transformation
- Mock data for development and testing

## Testing

Run the test suite:

```bash
npm run test
```

For end-to-end testing, use the appropriate testing framework configuration.

## Deployment

### Production Build

```bash
npm run build
```

### Server-Side Rendering

The application supports server-side rendering (SSR):

```bash
npm run build:ssr
npm run serve:ssr
```

## Contributing

1. Follow the existing code style and conventions
2. Write tests for new features
3. Update documentation as needed
4. Ensure all tests pass before submitting

## Support

For questions or issues, please refer to the project documentation or contact the development team.

## License

This project is proprietary and confidential to SkyRoute.
