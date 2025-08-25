using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(Guid id);
        Task<List<Sale>> GetAllAsync(int page, int pageSize);
        Task<List<Sale>> GetByCustomerAsync(Guid customerId, int page, int pageSize);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Sale sale);
    }
}
