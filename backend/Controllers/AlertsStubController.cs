using Microsoft.AspNetCore.Mvc;
using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;
namespace RateAlerts.Api.Controllers;

// =====================================================================================
// STUB CONTROLLER - provided for the FRONTEND track.
//
// This gives frontend candidates a working /api/alerts API so they can build the alert
// management UI without writing backend code. It keeps alerts in a static in-memory
// list and evaluates the "triggered" flag against the canned rates below, so creating
// an alert with a threshold on the wrong side of the canned rate will show as triggered.
//
// BACKEND-TRACK CANDIDATES: this is not a partial solution and you are not expected to
// keep it. Replace it or delete it; the alert feature is yours to design.
// =====================================================================================

[ApiController]
[Route("api/alerts")]
public class AlertsStubController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsStubController(IAlertService alertService)
    {
        _alertService = alertService;
    }
    private static readonly Dictionary<string, decimal> CannedRates = new()
    {
        ["USD/CAD"] = 1.3650m,
        ["GBP/USD"] = 1.2710m,
        ["EUR/USD"] = 1.0830m,
    };

   

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
       
            var userId = GetUserId();

            var alerts =  await _alertService.GetAllAsync(
                userId);

            return Ok(alerts);
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRateAlertRequest request)
    {
        if (!CannedRates.ContainsKey(request.Pair))
        {
            return BadRequest(new { error = $"Unknown pair '{request.Pair}'. The stub supports: {string.Join(", ", CannedRates.Keys)}." });
        }

        if (request.Direction is not (AlertDirection.Above or AlertDirection.Below))
        {
            return BadRequest(new { error = "Direction must be 'Above' or 'Below'." });
        }
        var userId = GetUserId();

        var alert = await _alertService.CreateAsync(
            userId,
            request);

       
        return CreatedAtAction(nameof(ListAsync),alert);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
       
            var userId = GetUserId();

            var deleted = await _alertService.DeleteAsync(
                id,
                userId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        
    }

   

    //public record Alert(Guid Id, string Pair, decimal Threshold, string Direction);

    /// <summary>
    /// /public record CreateAlertRequest(string Pair, decimal Threshold, string Direction);
    /// </summary>
    /// <returns></returns>
    private string GetUserId()
    {
        // Replace this with the authenticated user's ID
        // when authentication is configured.
        return User.Identity?.Name ?? "demouser";
    }
}
