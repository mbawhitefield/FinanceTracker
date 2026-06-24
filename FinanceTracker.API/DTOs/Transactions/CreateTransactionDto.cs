namespace FinanceTracker.API.DTOs.Transactions;

public class CreateTransactionDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime Date { get; set; }
}
