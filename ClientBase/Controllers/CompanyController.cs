using ClientBase.Models;
using System;
using System.Linq;

namespace ClientBase.Controllers
{
    public class CompanyController : ClientEntityController<Company>
    {
        #region Messages.

        protected override string CreationSuccessMessage => 
            "Компания успешно добавлена в базу клиентов";

        protected override string UpdateSuccessMessage =>
            "Информация о компании успешно обновлена";

        protected override string UpdateFailedMessage =>
            "Компания с таким ИНН уже зарегистрирована";

        protected override string DeleteSuccessMessage => 
            "Информация о компании удалена";

        protected override string DeleteFailedMessage =>
            "Ошибка при удалении компании из базы данны." +
            "Попробуйте снова, и если проблема повторится, обратитесь к системному администатору";

        #endregion

        protected override Func<Company, object> SelectFromFound => c => new { c.Id, c.Name, c.TaxpayerId };

        protected override Action<Company> EditHook => company =>
        {
            foreach (var companyFounder in company?.CompanyFounders ?? new CompanyFounder[0])
            {
                companyFounder.Company = company;
                companyFounder.CompanyId = company.Id;
            }
        };

        public CompanyController(IEntityRepository<Company> repository)
                    : base(repository)
        {
        }

        protected override IQueryable<Company> GetOrdered()
        {
            return Repository.Entities
                             .OrderByDescending(c => c.UpdateDate ?? c.CreationDate)
                             .ThenBy(c => c.Name);
        }

        protected override IQueryable<Company> GetFiltered(IQueryable<Company> companies, string search)
        {
            return companies.Filter(search);
        }
    }
}
