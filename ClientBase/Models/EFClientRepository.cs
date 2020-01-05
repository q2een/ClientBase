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

        public IQueryable<Company> Companies => context.Companies.Include(c => c.Founders)
                                                                 .ThenInclude(cf=> cf.Founder);
        public IQueryable<Founder> Founders => context.Founders.Include(f => f.Companies)
                                                               .ThenInclude(cf => cf.Company);

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

            context.SaveChangesAsync();
        }
    }
}
