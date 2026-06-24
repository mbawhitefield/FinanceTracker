namespace FinanceTracker.API.DTOs.Transactions;

public class UpdateTransactionDto
{
    public string? Title { get; set; }
    public decimal Amount { get; set; }
    public string? Type { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
    public DateTime? Date { get; set; }
}
