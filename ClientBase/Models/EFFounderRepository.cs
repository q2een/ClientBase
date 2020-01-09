using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class EFFounderRepository : IEntityRepository<Founder>
    {
        private readonly ApplicationDbContext context;

        public IQueryable<Founder> Entities => context.Founders.Include(f => f.FounderCompanies)
                                                               .ThenInclude(cf => cf.Company);

        public EFFounderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var founder = await context.Founders.Include(f => f.FounderCompanies)
                                    .ThenInclude(f => f.Company)
                                    .ThenInclude(f => f.CompanyFounders)
                                    .SingleOrDefaultAsync(f => f.Id == id);

            var companiesToRemove = founder.FounderCompanies.Where(f => f.Company.CompanyFounders.Count == 1)
                                                            .Select(f => f.Company);

            context.Companies.RemoveRange(companiesToRemove);
            context.Founders.Remove(founder);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Founder founder)
        {
            if (founder.Id == 0)
            {
                context.Founders.Add(founder);
                await context.SaveChangesAsync();
            }

            var entry = await context.Founders
                                     .SingleOrDefaultAsync(f => f.Id == founder.Id);

            if (entry == null)
                throw new DbUpdateException("Entity doesn't exist in database");

            entry.TaxpayerId = founder.TaxpayerId;
            entry.FirstName = founder.FirstName;
            entry.LastName = founder.LastName;
            entry.Patronymic = founder.Patronymic;
            entry.UpdateDate = founder.UpdateDate;

            await context.SaveChangesAsync();
        }
    }
}
