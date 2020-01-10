using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class EFCompanyRepository : IEntityRepository<Company>
    {
        private readonly ApplicationDbContext context;

        public IQueryable<Company> Entities => context.Companies.Include(c => c.CompanyFounders)                                                                
                                                                .ThenInclude(cf => cf.Founder)
                                                                .AsNoTracking();

        public EFCompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var company = await Entities.SingleOrDefaultAsync(c => c.Id == id);

            context.Companies.Remove(company);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            if (company.Id == 0)
            {
                context.Companies.Add(company);
                await context.SaveChangesAsync();
            }

            var entry = await context.Companies
                                     .Include(c => c.CompanyFounders)
                                     .SingleOrDefaultAsync(f => f.Id == company.Id);

            if (entry == null)
                throw new DbUpdateException("Entity doesn't exist in database");

            entry.TaxpayerId = company.TaxpayerId;
            entry.Name = company.Name;
            entry.UpdateDate = company.UpdateDate;
            entry.CompanyFounders = company.CompanyFounders;

            await context.SaveChangesAsync();
        }
    }
}
