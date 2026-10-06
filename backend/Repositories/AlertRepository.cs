using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;

namespace RateAlerts.Api.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        public Task<RateAlert> AddAsync(RateAlert alert)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<RateAlert>> GetActiveAlertsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<RateAlert>> GetAllAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<RateAlert?> GetByIdAsync(Guid id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RateAlert alert)
        {
            throw new NotImplementedException();
        }
    }
}
