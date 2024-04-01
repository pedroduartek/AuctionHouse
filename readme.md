
# Auction House
## Description
Auction House is a project aimed at managing vehicle auctions. It provides functionalities for starting and closing auctions, placing bids on vehicles, and adding and searching for vehicles based on various criteria.

## Installation
To install Auction House, follow these steps:
- Clone the repository to your local machine.
- Make sure you have .NET Core SDK installed.
- Restore the NuGet packages using dotnet restore.
- Run the application.

## Usage
Once installed, you can use Auction House to:
- Start and close auctions for vehicles.
- Place bids on vehicles.
- Search for vehicles based on type, manufacturer, model, and year.
- Add new vehicles to the auction.

# Documentation
## Authentication
Authentication is not required for accessing the endpoints.


## Auction Controller  

### Start Auction  
**Method:** POST  
**Endpoint:** `/api/Auction/start`  
**Description:** Starts an auction for a specified vehicle.  
**Parameters:**
- `vehicleId` (string, UUID): The unique identifier of the vehicle to start the auction for.  
**Response:** Returns a success status code (200) upon successful auction start.


### Close Auction  
**Method:** POST  
**Endpoint:** `/api/Auction/close`  
**Description:** Closes an active auction for a specified vehicle.  
**Parameters:**
- `vehicleId` (string, UUID): The unique identifier of the vehicle to close the auction for.  
**Response:** Returns a success status code (200) upon successful auction closure.



### Place Bid  
**Method:** POST  
**Endpoint:** `/api/Auction/place-bid`  
**Description:** Places a bid on an active auction for a specified vehicle.  
**Parameters:**
- `vehicleId` (string, UUID): The unique identifier of the vehicle on which the bid is placed.
- `bidAmount` (number, double): The amount of the bid.
- `bidder` (string): The name of the bidder.  
**Response:** Returns a success status code (200) upon successful bid placement.




## Vehicle Controller

### Add Vehicle  
**Method:** POST  
**Endpoint:** `/api/Vehicle/add`  
**Description:** Adds a new vehicle to the system.  
**Request Body:** `VehicleInputModel`  
**Example:**  
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "type": "string",
  "numberOfDoors": 0,
  "numberOfSeats": 0,
  "loadCapacity": 0,
  "manufacturer": "string",
  "model": "string",
  "year": 0,
  "startingBid": 0
}
```
**Response:** Returns a success status code (200) upon successful addition of the vehicle.  


### Search Vehicle
**Method:** GET  
**Endpoint:** `/api/Vehicle/search`  
**Description:** Searches for vehicles based on specified criteria.  
**Parameters:**
- `type` (string): Type of the vehicle (e.g., Sedan, SUV).
- `manufacturer` (string): Manufacturer of the vehicle.
- `model` (string): Model of the vehicle.
- `year` (integer): Year of manufacture of the vehicle.  
**Response:** Returns a success status code (200) along with the list of vehicles matching the search criteria.

This documentation provides a brief overview of the available endpoints and their usage. For detailed information on request and response formats, refer to the API specification provided in the OpenAPI format.
