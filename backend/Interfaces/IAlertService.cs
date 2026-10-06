using RateAlerts.Api.Models;

namespace RateAlerts.Api.Interfaces
{
    public interface IAlertService
    {
        Task<RateAlert> CreateAsync(
    string userId,
    CreateRateAlertRequest request);

        Task<IReadOnlyCollection<RateAlert>> GetAllAsync(
            string userId);

        Task<bool> DeleteAsync(
            Guid id,
            string userId);

        Task EvaluateAlertsAsync(
            CancellationToken cancellationToken);

    }
}
