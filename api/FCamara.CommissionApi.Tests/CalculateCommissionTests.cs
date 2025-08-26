using System.Net;
using System.Net.Http.Json;
using FCamara.Commission.Application.Sales.Commission;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionApi.Tests;

public class CalculateCommissionTests
{
    private readonly HttpClient _client;

    public CalculateCommissionTests()
    {
        var factory = new CustomWebApplicationFactory();
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task CalculateCommission_ReturnsCorrectAmounts_ForValidInput()
    {
        // Arrange
        var request =
            new CommissionCalculationRequest(LocalSalesCount: 10, ForeignSalesCount: 10, AverageSaleAmount: 100);
        
        // Act
        var response = await _client.PostAsJsonAsync("api/v1/commission", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CommissionCalculationResponse>();
        result.Should().NotBeNull();
        result!.FCamaraCommissionAmount.Should().Be(550);       
        result.CompetitorCommissionAmount.Should().Be(95.5m); 
    }
    
    [Fact]
    public async Task CalculateCommission_ReturnsBadRequest_ForInValidInput()
    {
        // Arrange
        var request =
            new CommissionCalculationRequest(LocalSalesCount: 0, ForeignSalesCount: 0, AverageSaleAmount: 0);
        
        // Act
        var response = await _client.PostAsJsonAsync("api/v1/commission", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        result.Should().NotBeNull();
        
        result!.Errors.Should().ContainKey(nameof(request.LocalSalesCount));
        result.Errors.Should().ContainKey(nameof(request.ForeignSalesCount));
        result.Errors.Should().ContainKey(nameof(request.AverageSaleAmount));

        // Optionally assert specific messages
        result.Errors[nameof(request.LocalSalesCount)][0]
            .Should().Be("Local sales count must be 0 or greater.");

        result.Errors[nameof(request.ForeignSalesCount)][0]
            .Should().Be("Foreign sales count must greater than 0.");

        result.Errors[nameof(request.AverageSaleAmount)][0]
            .Should().Be("Average sale amount must be greater than 0.");
         
    }
}