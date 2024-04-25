using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using StackExchange.Redis;
using Database.Handlers;
using System.Text.Json;

namespace Database.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MissionInfoController : ControllerBase
{
    private ConnectionMultiplexer conn;
    private readonly IDatabase gcs;



public MissionInfoController(){
        conn = DBConn.Instance().getConn();

        gcs = conn.GetDatabase();
        
    }

    
    [HttpGet("GetMissionInfo")]
    public IActionResult getExample()
    {
        return gcs.StringGet("missionName");
    }


    [HttpPost("MissionInfo")]
    public async Task<IActionResult> MissionInfo([FromBody] MissionInfo requestBody)
    {
        List<string> missingFields = new List<string>();

        Type type = typeof(MissionInfo);
        PropertyInfo[] properties = type.GetProperties();

        foreach (System.Reflection.PropertyInfo property in properties)
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
            if (missingFields.Count > 0)
            {
                return BadRequest("Missing fields: " + string.Join(", ", missingFields));
            }
            
        }


        await _redis.StringAppendAsync("missionName", requestBody.ToString());
        return Ok("Posted MissionInfo");
    } 
    


}
