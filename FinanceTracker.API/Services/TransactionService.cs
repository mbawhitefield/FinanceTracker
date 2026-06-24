using AutoMapper;
using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs.Summary;
using FinanceTracker.API.DTOs.Transactions;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TransactionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TransactionResponseDto> CreateTransactionAsync(int userId, CreateTransactionDto dto)
    {
        var transaction = _mapper.Map<Transaction>(dto);
        transaction.AppUserId = userId;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return _mapper.Map<TransactionResponseDto>(transaction);
    }

    public async Task<bool> DeleteTransactionAsync(int id, int userId)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);

        if (transaction == null) return false;

        _context.Transactions.Remove(transaction);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int userId, int month, int year)
    {
        var transactions = await _context.Transactions
            .Where(t => t.AppUserId == userId
            && t.Date.Month == month && t.Date.Year == year).ToListAsync();

        var income = transactions
            .Where(t => t.Type == "income")
            .Sum(t => t.Amount);

        var expenses = transactions
            .Where(t => t.Type == "expense")
            .Sum(t => t.Amount);

        var breakdown = transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategoryBreakdownDto
            {
                Category = g.Key,
                Total = g.Sum(t => t.Amount)
            }).ToList();

        return new MonthlySummaryDto
        {
            Year = year,
            Month = month,
            TotalIncome = income,
            NetBalance = income - expenses,
            Breakdown = breakdown
        };
    }

    public async Task<List<TransactionResponseDto>> GetTransactionAsync(int userId, int? month, int? year)
    {
        var query = _context.Transactions
            .Where(t => t.AppUserId == userId)
            .AsQueryable();

        if (month.HasValue)
            query = query.Where(t => t.Date.Month == month.Value);

        if (year.HasValue)
            query = query.Where(t => t.Date.Year == year.Value);

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();

        return _mapper.Map<List<TransactionResponseDto>>(transactions);
    }

    public async Task<TransactionResponseDto?> GetTransactionByIdAsync(int id, int userId)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
        return transaction == null ? null : _mapper.Map<TransactionResponseDto?>(transaction);
    }

    public async Task<TransactionResponseDto?> UpdateTransactionAsync(int id, int userId, UpdateTransactionDto dto)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);

        if (transaction == null) return null;

        _mapper.Map(dto, transaction);
        await _context.SaveChangesAsync();

        return _mapper.Map<TransactionResponseDto>(transaction);
    }
}
