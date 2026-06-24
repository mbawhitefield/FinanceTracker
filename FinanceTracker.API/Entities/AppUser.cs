using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.API.Entities;

public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties - one user has many transactions
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
