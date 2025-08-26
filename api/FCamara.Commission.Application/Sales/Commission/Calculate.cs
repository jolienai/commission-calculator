using FCamara.Commission.Application.Shared;
using FluentValidation;

namespace FCamara.Commission.Application.Sales.Commission;

public record CommissionCalculationRequest(int LocalSalesCount, int ForeignSalesCount, decimal AverageSaleAmount)
    : IRequest<Result<CommissionCalculationResponse>>;
public record CommissionCalculationResponse(decimal FCamaraCommissionAmount, decimal CompetitorCommissionAmount);

public class CommissionCalculationRequestValidator : AbstractValidator<CommissionCalculationRequest>
{
    public CommissionCalculationRequestValidator()
    {
        // I am assuming that the values cannot be zero
        RuleFor(x => x.LocalSalesCount)
            .GreaterThan(0)
            .WithMessage("Local sales count must be 0 or greater.");

        RuleFor(x => x.ForeignSalesCount)
            .GreaterThan(0)
            .WithMessage("Foreign sales count must greater than 0.");

        RuleFor(x => x.AverageSaleAmount)
            .GreaterThan(0)
            .WithMessage("Average sale amount must be greater than 0.");
    }
}

public class CommissionCalculationHandler (
    IValidator<CommissionCalculationRequest> validator, 
    IEnumerable<ICommissionCalculator> calculators) 
    : IRequestHandler<CommissionCalculationRequest, Result<CommissionCalculationResponse>>
{
    public async Task<Result<CommissionCalculationResponse>> HandleAsync(CommissionCalculationRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var fc = calculators.First(c => c is FCamaraCalculator);
            var comp = calculators.First(c => c is CompetitorCalculator);

            var result = new CommissionCalculationResponse(
                FCamaraCommissionAmount: fc.CalculateTotalCommission(request),
                CompetitorCommissionAmount: comp.CalculateTotalCommission(request)
            );

            return Result<CommissionCalculationResponse>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CommissionCalculationResponse>.Failure(ex);
        }
    }
}