namespace Database.Models
{
    public class EndpointResponse
    {
        public string? errorMessage { get; set; }
        public object? data { get; set; }

        public EndpointResponse(string errorMessage, object data)
        {
            this.errorMessage = errorMessage;
            this.data = data;
        }
    }
}