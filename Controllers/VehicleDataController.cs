using System.Net.Http.Headers;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;


public class VehicleDataController : ControllerBase
{
    private ConnectionMultiplexer conn;
    private readonly IDatabase _redis;

    public VehicleDataController()
    {
        conn = DBConn.Instance().getConn();
        _redis = conn.GetDatabase();
    }
    [HttpGet("vehicledata/get")]
    public String getVehicleData()
    {
        // return vehicle data
        return _redis.StringGet("vehicleData");
    }

    [HttpPost("vehicledata/post")]
    public async Task postVehicleData()
    {
        using (var sr = new StreamReader(Request.Body))
        {
            var content = await sr.ReadToEndAsync();
            await _redis.StringSetAsync("vehicleData", content);
        }
    }
    [HttpPost("example")]
    public async Task<IActionResult> PostExample([FromBody] ExampleModel requestBody)
    {
        List<string> missingFields = new List<string>();

        Type type = typeof(VehicleData); // Replace ExampleModel with the respective model
        PropertyInfo[] properties = type.GetProperties();
        foreach (System.Reflection.PropertyInfo property in requestBody.GetType().GetProperties())
        {
            var value = property.GetValue(requestBody, null);
            object defaultValue = null;
            if (property.PropertyType == typeof(string))
            {
                defaultValue = null;
            }
            else if (property.PropertyType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(property.PropertyType);
            }

            if (value?.Equals(defaultValue) == true || value == null)
            {
                missingFields.Add(property.Name);
            }

        }
        // Iterates through every property in the model and checks if it is null or default value

        if (missingFields.Count > 0)
        {
            return BadRequest("Missing fields: " + string.Join(", ", missingFields));
        }
        // If any field is missing, return a bad request

        await _redis.StringSetAsync("vehicleData", requestBody.ExampleString); // Replace "example" with the respective database key
        return Ok("hi");
    }

}
