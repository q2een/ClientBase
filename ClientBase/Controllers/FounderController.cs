using ClientBase.Models;
using System;
using System.Linq;

namespace ClientBase.Controllers
{
    public class FounderController : ClientEntityController<Founder>
    {
        #region Messages.

        protected override string CreationSuccessMessage => "Данные об учредителе успешно добавлены";

        protected override string UpdateSuccessMessage => "Данные об учредителе успешно обновлены";

        protected override string UpdateFailedMessage => "Учредитель с таким ИНН уже зарегистрирован";

        protected override string DeleteSuccessMessage => "Данные об учредителе успешно удалены";

        protected override string DeleteFailedMessage => "Ошибка при удалении учредителя из базы данны." +
                               "Попробуйте снова, и если проблема повторится, обратитесь к системному администатору";

        #endregion

        protected override Func<Founder, object> SelectFromFound =>
            f => new { f.Id, Name = f.FullName, f.TaxpayerId };

        public FounderController(IEntityRepository<Founder> repository) : base(repository)
        {
        }

        protected override IQueryable<Founder> GetOrdered()
        {
            return Repository.Entities
                             .OrderByDescending(f => f.UpdateDate ?? f.CreationDate)
                             .ThenBy(f => f.LastName);
        }

        protected override IQueryable<Founder> GetFiltered(IQueryable<Founder> founders, string search)
        {
            return founders.Filter(search);
        }
    }
}
