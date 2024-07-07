# Use the official .NET 6.0 SDK image as the base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files from your local machine to the container
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Create a new stage for the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port your application listens on (adjust if needed)
EXPOSE 5009

# Set the entry point to run your application
ENTRYPOINT ["dotnet", "csharpPubSub.dll"]