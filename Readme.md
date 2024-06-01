# Hotel Reservation System

This document provides information on how to set up and run the Hotel Reservation System, which consists of three main applications: AuthenticationService, ReservationManagementService, and BookingClientApp.

## Table of Contents

1. Introduction
2. Prerequisites
3. Setup Instructions
   - AuthenticationService
   - ReservationManagementService
   - BookingClientApp
4. Running the Applications
5. API Endpoints
6. User Interface
7. Configuration Files
8. Additional Notes

## 1. Introduction

The Hotel Reservation System is composed of the following applications:
1. **AuthenticationService**: Handles user authentication (registration and login).
2. **ReservationManagementService**: Manages reservations and room availability.
3. **BookingClientApp**: Provides the user interface for hotel room bookings and management.

## 2. Prerequisites

Before setting up and running the applications, ensure you have the following installed:
- .NET Core 3.1 SDK
- Microsoft SQL Server
- Visual Studio 2019 Community Edition or later
- A web browser (e.g., Google Chrome, Mozilla Firefox)

## 3. Setup Instructions

### AuthenticationService

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/Dominator4/OnlineReservationSystem.git
    cd OnlineReservationSystem\AuthenticationService
    ```

2. **Configure the Database**:
    - Update the `appsettings.json` file with your SQL Server connection string.
    - Example:
      ```json
      {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
          }
        },
        "AllowedHosts": "*",
        "Jwt": {
          "Key": "SuperSecretKey123456789",
          "Issuer": "YourIssuer",
          "Audience": "YourAudience"
        },
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthDb;Trusted_Connection=True;"
        }
      }
      ```

3. **Run Database Migrations**:
    Open the Package Manager Console in Visual Studio and run:
    ```sh
    Update-Database
    ```

4. **Build and Run the Project**:
    - Open the solution file `AuthenticationService.sln` in Visual Studio.
    - Build the project (Ctrl+Shift+B).
    - Run the project (F5).

### ReservationManagementService

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/Dominator4/OnlineReservationSystem.git
    cd OnlineReservationSystem\ReservationManagementService
    ```

2. **Configure the Database**:
    - Update the `appsettings.json` file with your SQL Server connection string.
    - Example:
      ```json
      {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
          }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ReservationDb;Trusted_Connection=True;"
        }
      }
      ```

3. **Run Database Migrations**:
    Open the Package Manager Console in Visual Studio and run:
    ```sh
    Update-Database
    ```

4. **Build and Run the Project**:
    - Open the solution file `ReservationManagementService.sln` in Visual Studio.
    - Build the project (Ctrl+Shift+B).
    - Run the project (F5).

### BookingClientApp

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/Dominator4/OnlineReservationSystem.git
    cd OnlineReservationSystem\BookingClientApp
    ```

2. **Configure the Database**:
    - Update the `appsettings.json` file with your SQL Server connection string.
    - Example:
      ```json
      {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
          }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookingClientDb;Trusted_Connection=True;"
        }
      }
      ```

3. **Run Database Migrations**:
    Open the Package Manager Console in Visual Studio and run:
    ```sh
    Update-Database
    ```

4. **Build and Run the Project**:
    - Open the solution file `BookingClientApp.sln` in Visual Studio.
    - Build the project (Ctrl+Shift+B).
    - Run the project (F5).

## 4. Running the Applications

After building and running all three projects, you can access the applications through the following URLs:

- **AuthenticationService**: https://localhost:5001
- **ReservationManagementService**: https://localhost:5002
- **BookingClientApp**: https://localhost:5003

Ensure all applications are running concurrently.

## 5. API Endpoints

### AuthenticationService

- **Register**: `POST /auth/register`
  - Body: `{ "email": "user@example.com", "password": "password123" }`

- **Login**: `POST /auth/login`
  - Body: `{ "email": "user@example.com", "password": "password123" }`

### ReservationManagementService

- **Check Availability**: `POST /reservation/check-availability`
  - Body: `{ "CheckInDate": "2024-06-01", "CheckOutDate": "2024-06-10" }`

- **Make Reservation**: `POST /reservation/make-reservation`
  - Body: `{ "SelectedRoomIds": [1, 2], "CustomerId": 1, "CheckInDate": "2024-06-01", "CheckOutDate": "2024-06-10", "Guests": 2 }`

## 6. User Interface

### Authentication Pages
- **Login Page**: `https://localhost:5003/Auth/Login`
- **Register Page**: `https://localhost:5003/Auth/Register`

### Booking Pages
- **Check Availability**: `https://localhost:5003/Reservation/CheckAvailability`
- **User Profile**: `https://localhost:5003/Reservation/UserProfile`
- **User Reservations**: `https://localhost:5003/Reservation/UserReservations`

## 7. Configuration Files

### appsettings.json (AuthenticationService)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
	"Key": "SuperSecretKey123456789",
	"Issuer": "YourIssuer",
	"Audience": "YourAudience"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthDb;Trusted_Connection=True;"
  }
}
