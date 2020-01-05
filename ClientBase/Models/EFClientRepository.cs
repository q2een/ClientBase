using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class EFClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext context;

        public EFClientRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Company> Companies => context.Companies.Include(c => c.CompanyFounders)
                                                                 .ThenInclude(cf => cf.Founder);                                                                 
        public IQueryable<Founder> Founders => context.Founders.Include(f => f.FounderCompanies)
                                                               .ThenInclude(cf => cf.Company);
        public IQueryable<Company> GetSingleFounderCompanies(int founderId)
        {
            return Companies.Where(c => c.CompanyFounders.Count == 1 &&
                                        c.CompanyFounders.Single().FounderId == founderId);
        }

        public async Task DeleteFounderAsync(int id)
        {
            var founder = await Founders.SingleOrDefaultAsync(f => f.FounderId == id);

            context.Companies.RemoveRange(GetSingleFounderCompanies(id));
            context.Founders.Remove(founder);

            await context.SaveChangesAsync();
        }

        public async Task UpdateFounderAsync(Founder founder)
        {
            if (founder.FounderId == 0)
            {
                context.Founders.Add(founder);
                await context.SaveChangesAsync();
            }

            var entry = await context.Founders
                                     .SingleOrDefaultAsync(f => f.FounderId == founder.FounderId);

            if (entry == null)
                throw new DbUpdateException("Entity doesn't exist in database");

            entry.TaxpayerId = founder.TaxpayerId;
            entry.FullName = founder.FullName;
            entry.UpdateDate = founder.UpdateDate;

            await context.SaveChangesAsync();
        }
    }
}
