using AutoMapper;
using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs.Transactions;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Helpers;
using FinanceTracker.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FinanceTracker.Tests;

public class TransactionServiceTests
{
    // Creates a fresh in-memory database for each test
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
            .Options;
        return new AppDbContext(options);
    }

    private IMapper GetMapper()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<MappingProfiles>(),
            NullLoggerFactory.Instance
        );
        return config.CreateMapper();
    }

    [Fact]
    public async Task CreateTransaction_ShouldReturnTransactionWithCorrectUserId()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new TransactionService(context, GetMapper());
        var dto = new CreateTransactionDto
        {
            Title = "Salary",
            Amount = 5000,
            Type = "income",
            Category = "Work",
            Date = DateTime.UtcNow
        };

        // Act
        var result = await service.CreateTransactionAsync(userId: 1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Salary", result.Title);
        Assert.Equal(5000, result.Amount);
    }

    [Fact]
    public async Task GetTransactions_ShouldOnlyReturnCurrentUsersTransactions()
    {
        // Arrange
        var context = GetInMemoryContext();
        context.Transactions.AddRange(
            new Transaction
            {
                Title = "Mine",
                Amount = 100,
                AppUserId = 1,
                Type = "income",
                Category = "Work",
                Date = DateTime.UtcNow
            },
            new Transaction
            {
                Title = "NotMine",
                Amount = 200,
                AppUserId = 2,
                Type = "income",
                Category = "Work",
                Date = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();

        var service = new TransactionService(context, GetMapper());

        // Act
        var results = await service.GetTransactionAsync(userId: 1, null, null);

        // Assert
        Assert.Single(results);
        Assert.Equal("Mine", results[0].Title);
    }
}