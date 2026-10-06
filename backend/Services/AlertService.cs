using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;

namespace RateAlerts.Api.Services
{
    public class AlertService : IAlertService
    {
        public Task<RateAlert> CreateAsync(string userId, CreateRateAlertRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task EvaluateAlertsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<RateAlert>> GetAllAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
