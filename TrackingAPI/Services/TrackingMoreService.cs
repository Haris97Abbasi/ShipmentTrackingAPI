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

        //Code that gets tracking data for all shipments
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
            catch (HttpRequestException ex)
            {
                return $"HTTP Request Exception: {ex.Message}";
            }
            catch (TimeoutException ex)
            {
                return $"Timeout Exception: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Catch other exceptions: {ex.Message}";
            }
        }


        //Code that gets a list of all available couriers
        public string GetCouriers()
        {
            try
            {
                TrackingMore trackingMore = new TrackingMore(apiKey);

                var apiResponse = trackingMore.Courier.GetAllCouriers();
                var filteredData = new List<object>();

                foreach (var item in apiResponse.data)
                {
                    var courierJson = JObject.FromObject(item);
                    var filteredItem = new
                    {
                        courier_name = courierJson["courier_name"]?.ToString(),
                        courier_code = courierJson["courier_code"]?.ToString()
                    };
                    filteredData.Add(filteredItem);
                }

                var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(filteredData);
                return jsonResponse;
            }
            catch (TrackingMoreException ex)
            {
                return $"Catch custom exceptions: {ex.Message}";
            }
            catch (TimeoutException ex)
            {
                return $"Timeout Exception: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Catch other exceptions: {ex.Message}";
            }
        }

        //Code that gets tracking data for a specific tracking number and carrier code
        public async Task<string> GetTrackingDataAsync(string trackingNumber, string carrierCode)
        {
            try
            {
                string url = "https://api.trackingmore.com/v2/trackings/realtime";
                var requestData = new
                {
                    tracking_number = trackingNumber,
                    carrier_code = carrierCode
                };
                string jsonData = JsonConvert.SerializeObject(requestData);

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Add("Trackingmore-Api-Key", apiKey);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseBody);
                        return jsonResponse.ToString();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return $"HTTP Request Exception: {ex.Message}";
            }
            catch (TimeoutException ex)
            {
                return $"Timeout Exception: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
