using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;
using System.Collections.Concurrent;

namespace RateAlerts.Api.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ConcurrentDictionary<Guid, RateAlert> _alerts = new();

        public Task<RateAlert> AddAsync(RateAlert alert)
        {
            _alerts[alert.Id] = alert;

            return Task.FromResult(alert);
        }

        public Task<IReadOnlyCollection<RateAlert>> GetAllAsync(string userId)
        {
            var alerts = _alerts.Values
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return Task.FromResult<IReadOnlyCollection<RateAlert>>(alerts);
        }

        public Task<RateAlert?> GetByIdAsync(Guid id, string userId)
        {
            if (_alerts.TryGetValue(id, out var alert) &&
                alert.UserId == userId)
            {
                return Task.FromResult<RateAlert?>(alert);
            }

            return Task.FromResult<RateAlert?>(null);
        }

        public Task<bool> DeleteAsync(Guid id, string userId)
        {
            if (_alerts.TryGetValue(id, out var alert) &&
                alert.UserId == userId)
            {
                return Task.FromResult(_alerts.TryRemove(id, out _));
            }

            return Task.FromResult(false);
        }

        public Task<IReadOnlyCollection<RateAlert>> GetActiveAlertsAsync()
        {
            var alerts = _alerts.Values
                .Where(x => !x.Triggered)
                .ToList();

            return Task.FromResult<IReadOnlyCollection<RateAlert>>(alerts);
        }

        public Task UpdateAsync(RateAlert alert)
        {
            _alerts[alert.Id] = alert;

            return Task.CompletedTask;
        }

    }
}
