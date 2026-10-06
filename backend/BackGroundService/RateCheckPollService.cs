using RateAlerts.Api.Interfaces;

namespace RateAlerts.Api.BackGroundService
{
    public class RateCheckPollService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RateCheckPollService> _logger;

        private readonly TimeSpan _interval =
            TimeSpan.FromSeconds(30);
        public RateCheckPollService(
    IServiceScopeFactory scopeFactory,
    ILogger<RateCheckPollService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Alert evaluation service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var alertService =
                        scope.ServiceProvider
                            .GetRequiredService<IAlertService>();

                    await alertService.EvaluateAlertsAsync(
                        stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error while evaluating alerts.");
                }

                await Task.Delay(
                    _interval,
                    stoppingToken);
            }

            _logger.LogInformation(
                "Alert evaluation service stopped.");
        }
    }
}

