using FCamara.Commission.Application.Sales.Commission;
using FCamara.Commission.Application.Shared;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FCamara.Commission.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CommissionCalculationRequest>, CommissionCalculationRequestValidator>();
        services.AddScoped<IMediator, Mediator>();
        services
            .AddScoped<IRequestHandler<CommissionCalculationRequest, Result<CommissionCalculationResponse>>,
                CommissionCalculationHandler>();
        services.AddScoped<ICommissionCalculator, FCamaraCalculator>();
        services.AddScoped<ICommissionCalculator, CompetitorCalculator>();
        return services;
    }
}