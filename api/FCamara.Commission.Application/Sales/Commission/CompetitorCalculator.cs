namespace FCamara.Commission.Application.Sales.Commission;

public class CompetitorCalculator : ICommissionCalculator
{
    private const decimal LocalRate = 0.02m;
    private const decimal ForeignRate = 0.0755m;

    public decimal CalculateTotalCommission(CommissionCalculationRequest request)
    {
        return CalculateLocalCommission(request.LocalSalesCount, request.AverageSaleAmount) +
               CalculateForeignCommission(request.ForeignSalesCount, request.AverageSaleAmount);
    }

    private decimal CalculateLocalCommission(int localSales, decimal averageSaleAmount)
        => LocalRate * localSales * averageSaleAmount;

    private decimal CalculateForeignCommission(int foreignSales, decimal averageSaleAmount)
        => ForeignRate * foreignSales * averageSaleAmount;
    
}
