using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public interface IClientRepository
    {
        IQueryable<Company> Companies { get; }
        
        IQueryable<Founder> Founders { get; }

        IQueryable<Company> GetSingleFounderCompanies(int founderId);

        Task UpdateFounderAsync(Founder founder);

        Task DeleteFounderAsync(int id);
    }
}
