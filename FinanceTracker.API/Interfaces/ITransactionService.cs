using FinanceTracker.API.DTOs.Summary;
using FinanceTracker.API.DTOs.Transactions;

namespace FinanceTracker.API.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionResponseDto>> GetTransactionAsync(int userId, int? month, int? year);
    Task<TransactionResponseDto?> GetTransactionByIdAsync(int id, int userId);
    Task<TransactionResponseDto> CreateTransactionAsync(int userId, CreateTransactionDto dto);
    Task<TransactionResponseDto?> UpdateTransactionAsync(int id, int userId, UpdateTransactionDto dto);
    Task<bool> DeleteTransactionAsync(int id, int userId);
    Task<MonthlySummaryDto> GetMonthlySummaryAsync(int userId, int month, int year);
}
