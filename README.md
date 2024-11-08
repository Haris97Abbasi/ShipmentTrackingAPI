# Shipment Tracking Application

## Overview
This application provides a powerful RESTful API for tracking shipments across over 1200 carriers. Built with an ASP.NET Core Web API backend and a responsive Angular frontend, it leverages the robust TrackingMore API to deliver real-time tracking of shipments, display courier details, and track shipment history. This project aims to reduce WISMO (Where Is My Order) inquiries, optimize logistics operations, and boost customer satisfaction.

## Features
- **Track Shipments**: Enter a tracking number and carrier code to retrieve up-to-date tracking data.
- **View Couriers**: Provides a list of over 1200 available couriers, including their names and codes.
- **Comprehensive Data**: Tracks and displays more than 40 parameters for each shipment, including:
  - **Meta Section**: 3 parameters (e.g., code, type, message).
  - **Data Section (Single Item)**: 19 parameters (e.g., tracking number, status, country information).
  - **Origin Information**: 10 parameters (e.g., customs clearance status, departure).
  - **Tracking History**: Includes details like status description, date, and location checkpoints.
- **Responsive Design**: Angular frontend with Bootstrap styling for a modern user interface.
- **Easy Toggle Views**: Seamlessly switch between viewing tracking details and courier lists.

## Technologies Used
- **Backend**: ASP.NET Core Web API (C#)
- **Frontend**: Angular
- **API Integration**: TrackingMore API for carrier tracking and data retrieval
- **Design & Styling**: Bootstrap
- **Data Serialization**: Newtonsoft.Json for JSON parsing and formatting

## Getting Started
These instructions will help you set up and run a copy of the project for development and testing.

### Prerequisites
- Microsoft Visual Studio 2022
- Node.js and Angular CLI
- TrackingMore API Key (stored securely in `appsettings.json`)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/shipment-tracking-app.git
2. Open the solution file in Visual Studio 2022.
3. Update the `appsettings.json` file with your TrackingMore API key. Ensure the key is securely stored and not exposed in the code.
4. Install the necessary dependencies using the NuGet Package Manager for the ASP.NET Core backend and the Node.js package manager for the Angular frontend:
   - For the backend:
     ```bash
     dotnet restore
     ```
   - For the frontend:
     ```bash
     npm install
     ```

5. Build and run the backend API:
   ```bash
   dotnet build
   dotnet run
   ```
6. Navigate to the Angular project directory and start the frontend:
```bash
ng serve
```
7. Open a browser and go to [http://localhost:4200](http://localhost:4200) to interact with the application.

## Usage
**Track Shipments**: Enter the tracking number and carrier code in the input fields and click "Track" to see the shipment's details, including the tracking status, origin information, and detailed history.

**View Couriers**: Click the "See All Couriers" button to view a table listing available couriers and their codes.

## Code Snippets

### `TrackingMoreService.cs` (Backend Service)

```csharp
using TrackingMoreAPI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace TrackingAPI.Services
{
    public class TrackingMoreService : ITrackingMoreService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string apiKey;

        public TrackingMoreService(IConfiguration configuration)
        {
            apiKey = configuration["TrackingMoreAPI:ApiKey"] ?? throw new InvalidOperationException("API Key not found");
        }

        // Code to get tracking data for all shipments
        public async Task<string> GetTrackingDataAsync()
        {
            try
            {
                string url = "https://api.trackingmore.com/v4/trackings/get";

                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Tracking-Api-Key", apiKey);

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        // Additional methods omitted for brevity...
    }
}
```

### `TrackingController.cs` (Backend Controller)

```csharp
using Microsoft.AspNetCore.Mvc;
using TrackingAPI.Model;
using TrackingAPI.Services;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly TrackingMoreService _trackingMoreService;

        public TrackingController(IConfiguration configuration)
        {
            _trackingMoreService = new TrackingMoreService(configuration);
        }

        // Code to get tracking data for all shipments
        [HttpGet("get-tracking-data")]
        public async Task<IActionResult> GetTrackingData()
        {
            var result = await _trackingMoreService.GetTrackingDataAsync();
            return Ok(result);
        }

        // Additional methods omitted for brevity...
    }
}
```
### `tracking.component.html` (Frontend)

```html
<div class="container mt-5 bg-container p-5 rounded">
  <h2 class="text-center">Track Your Shipment</h2>
  <div class="d-flex justify-content-between align-items-start">
    <form (ngSubmit)="onTrack()" #trackForm="ngForm" class="p-4 border rounded bg-light w-75">
      <div class="form-group">
        <label for="TrackingNumber">Tracking ID:</label>
        <input type="text" id="TrackingNumber" [(ngModel)]="trackingData.TrackingNumber" name="TrackingNumber" required class="form-control" placeholder="Enter Tracking ID">
      </div>
      <div class="form-group mt-3">
        <label for="CarrierCode">Carrier Code:</label>
        <input type="text" id="CarrierCode" [(ngModel)]="trackingData.CarrierCode" name="CarrierCode" required class="form-control" placeholder="Enter Carrier Code">
      </div>
      <button type="submit" [disabled]="!trackForm.valid" class="btn btn-primary mt-3">Track</button>
    </form>

    <button class="btn btn-secondary ml-3 mt-3" (click)="toggleView('couriers')">See All Couriers</button>
  </div>

  <!-- Additional HTML omitted for brevity... -->
</div>

```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. Please ensure any code changes adhere to the project's coding standards and include appropriate tests.

## License
Distributed under the MIT License. See `LICENSE` for more information.

## Contact
Haris Abbasi - [haris97duisburg@gmail.com](mailto:haris97duisburg@gmail.com)  
GitHub: [Haris97Abbasi](https://github.com/Haris97Abbasi)




