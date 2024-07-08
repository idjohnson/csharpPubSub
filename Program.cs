using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

app.MapPost("/BBB", [Topic("pubsub", "BBB")] async (ILogger<Program> logger, MessageEvent item) =>
{
    Console.WriteLine($"{item.MessageType}: {item.Message}");
    if (item.Message.StartsWith("AWX:"))
    {
        string AWXJOB = item.Message.Substring(4); // Extract the text after "AWX:"
        Console.WriteLine($"JOB FOUND: {AWXJOB}");

        // Construct the URL
        string baseUrl = Environment.GetEnvironmentVariable("AWX_BASEURI");;
        string launchUrl = $"{baseUrl}{AWXJOB}/launch/";

        // Create an HttpClient with basic authentication
        using var client = new HttpClient();

        // Retrieve username and password from environment variables
        string username = Environment.GetEnvironmentVariable("AWX_USERNAME");
        string password = Environment.GetEnvironmentVariable("AWX_PASSWORD");

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

        // Make the POST request
        HttpResponseMessage response = await client.PostAsync(launchUrl, null);

        // Print the response code
        Console.WriteLine($"Response Code: {response.StatusCode}");
    }
    else
    {
        Console.WriteLine("No AWX job found.");
    }

    return Results.Ok();
});


await app.RunAsync();

internal record MessageEvent(string MessageType, string Message);
