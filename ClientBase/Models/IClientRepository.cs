using System.Linq;

namespace ClientBase.Models
{
    public interface IClientRepository
    {
        IQueryable<Company> Companies { get; }
        
        IQueryable<Founder> Founders { get; }
    }
}
