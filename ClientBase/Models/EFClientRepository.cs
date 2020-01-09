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

        public async Task UpdateFounderAsync(Founder founder)
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

        public async Task UpdateCompanyAsync(Company company)
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

        public async Task UpdateAsync<T>(T entity) where T: class, IClientEntity, new ()
        {
            switch(entity)
            {
                case Company company:
                    await UpdateCompanyAsync(company);
                    return;
                case Founder founder:
                    await UpdateFounderAsync(founder);
                    return;
                default:
                    throw new ArgumentException(
                        message: "Not recognized entity",
                        paramName: nameof(entity));
            }
        }

        public async Task DeleteAsync<T>(int id) where T : class, IClientEntity, new()
        {
            switch (new T())
            {
                case Company c:
                    await DeleteFounderAsync(id);
                    return;
                case Founder f:
                    await DeleteFounderAsync(id);
                    return;
                default:
                    throw new ArgumentException(
                        message: "Not recognized type",
                        paramName: nameof(T));
            }
        }
    }
}
