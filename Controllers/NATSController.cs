using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using NATS.Handlers;
namespace Database.Controllers;


// Template
// Name file endpoint name + Controller
public class NATSController : ControllerBase
{
    private readonly ILogger<NATSController> _logger;
    private readonly IDatabase _redis;
    private readonly NATSConsumer _nats;

    public NATSController(
        ILogger<NATSController> logger,
        IConnectionMultiplexer redis,
        NATSConsumer nats)
    {
        _logger = logger;
        _redis = redis.GetDatabase();
        _nats = nats;
    }

    [HttpGet("/ws/{vehicleName}")]
    public String getExample()
    {
        . . .
        return _redis.StringGet("example"); // Replace "example" with the respective database key
    }

    [HttpPost("example")]
    public async Task postExample()
    {
        using (var sr = new StreamReader(Request.Body))
        {
            var content = await sr.ReadToEndAsync();
            await _redis.StringSetAsync("example", content); // Replace "example" with the respective database key
        }
    }
}