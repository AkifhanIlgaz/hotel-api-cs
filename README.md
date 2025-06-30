This repository is an implementation of the project described at https://github.com/VB10/real_projects_for_developers/blob/main/projects/hotel-booking-app/backend.md.

# Hotel Booking App - Backend

## Application Type
A backend REST API.

## Technology Stack
This project is built with **ASP.NET Core** and uses **Entity Framework Core** for data access. It does not use Node.js or in-memory data as described in the original project description.

## Core Features
*   **User Authentication:** Implements JWT-based registration and login using ASP.NET Core Identity.
*   **Hotel API:** Endpoints to list, filter, and manage hotels.
*   **Reservations API:** Endpoint to view reservations for a specific hotel.

## Project Structure
```
/
├── Context/              # DbContext for Entity Framework
├── Controllers/          # API controllers
├── DTOs/                 # Data Transfer Objects
├── Exceptions/           # Custom exception classes
├── Extensions/           # Extension methods
├── Middlewares/          # Custom middleware
├── Migrations/           # Database migrations
├── Models/               # Data models (Entities)
├── Repositories/         # Repository interfaces
├── Responses/            # Standardized API response models
├── Services/             # Business logic services
├── Program.cs            # Main application entry point
└── HotelApi.csproj       # Project file
```

## API Endpoints

### Authentication
The application uses ASP.NET Core Identity, which provides the following endpoints under the `/register`, `/login` etc. base paths.

*   `POST /register`: Register a new user.
    *   **Body:** `{ "email": "user@example.com", "password": "Password123!" }`
*   `POST /login`: Log in a user and receive a JWT.
    *   **Body:** `{ "email": "user@example.com", "password": "Password123!" }`
*   `POST /refresh`: Refresh an expired JWT.
*   And other standard Identity endpoints...

### Hotels
*   `GET /api/hotels`: Get a paginated list of all hotels.
    *   **Query Params:** `PageNumber`, `PageSize`, `SearchTerm`, `MinPrice`, `MaxPrice`, `Location`, `SortBy`, `SortOrder`
*   `GET /api/hotels/{id}`: Get details for a specific hotel.
*   `POST /api/hotels`: Add a new hotel. (**Admin only**)
*   `PUT /api/hotels/{id}`: Update an existing hotel. (**Admin only**)
*   `DELETE /api/hotels/{id}`: Delete a hotel. (**Admin only**)
*   `GET /api/hotels/{hotelId}/reservations`: Get all reservations for a specific hotel. (**Admin only**)


## Example Usage

### Register a new user
```bash
curl -X POST "http://localhost:5000/register" \
-H "Content-Type: application/json" \
-d '{
  "email": "testuser@example.com",
  "password": "Password123!"
}'
```

### Login
```bash
curl -X POST "http://localhost:5000/login" \
-H "Content-Type: application/json" \
-d '{
  "email": "testuser@example.com",
  "password": "Password123!"
}'
```
**Response will contain the `accessToken`.**

### Get all hotels
```bash
curl -X GET "http://localhost:5000/api/hotels?PageNumber=1&PageSize=10"
```

### Get a specific hotel
```bash
curl -X GET "http://localhost:5000/api/hotels/{hotel_id}"
```

### Add a new hotel (Admin)
```bash
curl -X POST "http://localhost:5000/api/hotels" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer {your_access_token}" \
-d '{
  "name": "The Grand Hotel",
  "description": "A very grand hotel.",
  "location": "New York",
  "imageUrl": "http://example.com/image.jpg",
  "pricePerNight": 250.00,
  "features": ["Pool", "Spa"]
}'
```
