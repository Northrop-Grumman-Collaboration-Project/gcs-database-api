using Database.Handlers;
using StackExchange.Redis;
using NATS.Handlers;

//json.Set("UGV_[timestamp]", "$", UGV_tel);
//Console.WriteLine(json.Get("UGV_[timestamp]")); // prints json

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    return ConnectionMultiplexer.Connect("localhost");
});

builder.Services.AddScoped<RabbitMqConsumer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseWebSockets();

app.UseAuthorization();

app.MapControllers();

app.Run();