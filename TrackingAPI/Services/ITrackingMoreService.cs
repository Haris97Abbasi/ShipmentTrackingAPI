namespace TrackingAPI.Services
{
    public interface ITrackingMoreService
    {
        Task<string> GetTrackingDataAsync();
        string GetCouriers();
        Task<string> GetTrackingDataAsync(string trackingNumber, string carrierCode);
    }
}
