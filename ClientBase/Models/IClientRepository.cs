using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public interface IClientRepository
    {
        IQueryable<Company> Companies { get; }
        
        IQueryable<Founder> Founders { get; }

        Task UpdateFounderAsync(Founder founder);
    }
}
