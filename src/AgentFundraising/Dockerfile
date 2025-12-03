# Dockerfile for Fundraising Agent (.NET 10)
# Multi-stage build for optimized container size

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["AgentFundraising.csproj", "./"]
RUN dotnet restore "AgentFundraising.csproj"

# Copy source code and build
COPY . .
RUN dotnet build "AgentFundraising.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AgentFundraising.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port (App Service expects 8080 by default)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "AgentFundraising.dll"]
