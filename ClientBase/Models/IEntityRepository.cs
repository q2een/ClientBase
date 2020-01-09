using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public interface IEntityRepository<TEntity> where TEntity : IClientEntity
    {
        IQueryable<TEntity> Entities { get; }

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);
    }
}
