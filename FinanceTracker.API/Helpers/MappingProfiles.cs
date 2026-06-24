using AutoMapper;
using FinanceTracker.API.DTOs.Transactions;
using FinanceTracker.API.Entities;

namespace FinanceTracker.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Map from Entity to Response dto (for get requests)
        CreateMap<Transaction, TransactionResponseDto>();

        // Map from create dto to entity (for post requests)
        CreateMap<CreateTransactionDto, Transaction>();

        // Map from update dto to entity (for put requests)
        // Only update fields that are not null in the incoming DTO
        // This allows partial updates — fields not sent remain unchanged
        CreateMap<UpdateTransactionDto, Transaction>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
