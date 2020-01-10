using ClientBase.Infrastructure;
using ClientBase.Models;
using ClientBase.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Controllers
{
    public class FounderController : ClientEntityController<Founder>
    {
        #region Messages.

        protected override string CreationSuccessMessage => "Данные об учредителе успешно добавлены";

        protected override string UpdateSuccessMessage => "Данные об учредителе успешно обновлены";

        protected override string UpdateFailedMessage => "Невозможно обновить информацию об учредителе. Возможно, учредитель был удален из базы данных";

        protected override string DeleteSuccessMessage => "Данные об учредителе успешно удалены";

        protected override string DeleteFailedMessage => "Ошибка при удалении учредителя из базы данны." +
                               "Попробуйте снова, и если проблема повторится, обратитесь к системному администатору";

        #endregion

        protected override Func<Founder, object> SelectFromFound =>
            f => new { Id = f.Id, Name = f.FullName, f.TaxpayerId };

        public FounderController(IEntityRepository<Founder> repository) : base(repository)
        {
        }

        protected override IQueryable<Founder> GetOrdered()
        {
            return Repository.Entities
                             .OrderByDescending(f => f.UpdateDate)
                             .ThenByDescending(f => f.CreationDate)
                             .ThenBy(f => f.LastName);
        }

        protected override IQueryable<Founder> GetFiltered(IQueryable<Founder> founders, string search)
        {
            return founders.Where(f => f.LastName.Contains(search) ||
                                       f.FirstName.Contains(search) ||
                                       f.TaxpayerId.ToString().Contains(search));
        }
    }
}
