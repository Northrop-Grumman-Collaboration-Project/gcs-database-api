using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using Database.Models;
using System.Reflection;

public class VehicleStatusController : ControllerBase
{
    private ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
    private readonly IDatabase gcs;

    public VehicleStatusController()
    {
        gcs = redis.GetDatabase();
    }

/*
--------------Posts and Gets----------------
Body format for all posts and gets in this file:
{
    "Key": "[VehicleName]"
}
*/

    [HttpPost("SetStatusInUse")]
    public async Task<IActionResult> SetStatusInUse([FromBody] VehicleKey requestBody)
    {
        List<string> missingFields = new List<string>();

        Type type = typeof(VehicleKey); // Replace ExampleModel with the respective model
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

        await gcs.StringSetAsync($"{requestBody.Key}-status", "1"); // Replace "example" with the respective database key
        return Ok("Status set to In Use");
    }

    [HttpPost("SetStatusStandby")]
    public async Task<IActionResult> SetStatusStandby([FromBody] VehicleKey requestBody)
    {
        List<string> missingFields = new List<string>();

        Type type = typeof(VehicleKey); // Replace ExampleModel with the respective model
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

        await gcs.StringSetAsync($"{requestBody.Key}-status", "2"); // Replace "example" with the respective database key
        return Ok("Status set to Standby");
    }

    [HttpPost("EmergencyStop")]
    public async Task<IActionResult> EmergencyStop([FromBody] VehicleKey requestBody)
    {
        List<string> missingFields = new List<string>();

        Type type = typeof(VehicleKey); // Replace ExampleModel with the respective model
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

        await gcs.StringSetAsync($"{requestBody.Key}-status", "3"); // Replace "example" with the respective database key
        return Ok("Vehicle Emergency Stopped");
    }

    [HttpGet("GetVehicleStatus")]
    public async Task<string> GetVehicleStatus(){
        int num = 0; //Default of 0, database value will be read into this variable
        using (var sr = new StreamReader(Request.Body)){
            string vehicleData = await sr.ReadToEndAsync();
            VehicleKey? vehicleKey = JsonSerializer.Deserialize<VehicleKey>(vehicleData);
            string key = $"{vehicleKey?.Key}-status";
            num = (int) await gcs.StringGetAsync(key);
        }
        string output; //output string, will be returned. 
        //Switch block to conert int value in database to string value for enum. 
        switch (num){
            case 1: 
                output = "In Use";
                break;
            case 2:
                output = "Standby";
                break;
            case 3:
                output = "Emergency Stopped";
                break;
            default:
                output = "Invalid";
                break;
        }
        return output;
    }
}