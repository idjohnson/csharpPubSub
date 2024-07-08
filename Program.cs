using System.Text.Json.Serialization;
using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();

// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

if (app.Environment.IsDevelopment()) {app.UseDeveloperExceptionPage();}

app.MapGet("/", () => "Hello World!");

// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/BBB", [Topic("pubsub", "BBB")] (ILogger<Program> logger, MessageEvent item) => {
    Console.WriteLine($"{item.MessageType}: {item.Message}");
    return Results.Ok();
});

await app.RunAsync();

internal record MessageEvent(string MessageType, string Message);
