namespace FinanceTracker.API.DTOs.Summary;

public class MonthlySummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetBalance { get; set; }
    public List<CategoryBreakdownDto> Breakdown { get; set; } = new();


}

public class CategoryBreakdownDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
