using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public interface IClientRepository
    {
        IQueryable<Company> Companies { get; }
        
        IQueryable<Founder> Founders { get; }

        Task UpdateAsync<T>(T entity) where T : class, IClientEntity, new();

        Task DeleteAsync<T>(int id) where T : class, IClientEntity, new();

        Task UpdateFounderAsync(Founder founder);
        Task DeleteFounderAsync(int id);
        Task UpdateCompanyAsync(Company company);
    }
}
