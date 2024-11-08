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

        //Code that gets tracking data for all shipments
        [HttpGet("get-tracking-data")]
        public async Task<IActionResult> GetTrackingData()
        {
            var result = await _trackingMoreService.GetTrackingDataAsync();
            return Ok(result);
        }

        //Code that gets a list of all available couriers
        [HttpGet("get-couriers")]
        public IActionResult GetCouriers()
        {
            var result = _trackingMoreService.GetCouriers();
            return Ok(result);
        }

        //Code that tracks a shipment
        [HttpPost("track")]
        public async Task<IActionResult> TrackShipment([FromBody] TrackingRequest request)
        {
            if (string.IsNullOrEmpty(request.TrackingNumber) || string.IsNullOrEmpty(request.CarrierCode))
            {
                return BadRequest("Tracking number and carrier code are required.");
            }

            string trackingData = await _trackingMoreService.GetTrackingDataAsync(request.TrackingNumber, request.CarrierCode);
            return Ok(trackingData);
        }
    }
}
