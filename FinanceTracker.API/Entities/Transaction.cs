namespace FinanceTracker.API.Entities;

public class Transaction
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "Income" or "Expense"
    public string Category { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    // Foreign key - links this transaction to a user
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
