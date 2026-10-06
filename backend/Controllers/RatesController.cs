using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;

namespace RateAlerts.Api.Controllers;

[ApiController]
[Route("api/rates")]
public class RatesController : ControllerBase
{
    private readonly IXeRateService _rateXEService;

    public RatesController(IXeRateService rateXEService)
    {
        _rateXEService = rateXEService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRatesAsync(string baseCurrency, string targetcurrency)
    {
        var results = new List<XERateResponse>();
        results.Add(await _rateXEService.GetRateAsync(baseCurrency, targetcurrency, CancellationToken.None));
        

        return Ok(results);
    }
}
