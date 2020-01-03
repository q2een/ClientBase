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

        public async Task<bool> UpdateFounderAsync(Founder founder)
        {
            var entry = await context.Founders
                                     .SingleOrDefaultAsync(f => f.FounderId == founder.FounderId);

            if (entry == null)
                throw new DbUpdateException("Entity doesn't exist in database");

            entry.TaxpayerId = founder.TaxpayerId;
            entry.FullName = founder.FullName;
            entry.UpdateDate = DateTime.Now;

            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
