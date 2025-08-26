namespace FCamara.Commission.Application.Sales.Commission;

public interface ICommissionCalculator
{
    decimal CalculateTotalCommission(CommissionCalculationRequest request);
}