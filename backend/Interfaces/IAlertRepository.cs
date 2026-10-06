using RateAlerts.Api.Models;

namespace RateAlerts.Api.Interfaces
{
    public interface IAlertRepository
    {
        Task<RateAlert> AddAsync(RateAlert alert);

        Task<IReadOnlyCollection<RateAlert>> GetAllAsync(string userId);

        Task<RateAlert?> GetByIdAsync(Guid id, string userId);

        Task<bool> DeleteAsync(Guid id, string userId);

        Task<IReadOnlyCollection<RateAlert>> GetActiveAlertsAsync();

        Task UpdateAsync(RateAlert alert);
    }
}
