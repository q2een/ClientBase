using ClientBase.Infrastructure;
using ClientBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEntityRepository<Founder> founderRepository;
        private readonly IEntityRepository<Company> companyRepository;

        public HomeController(IEntityRepository<Founder> founderRepository,
                              IEntityRepository<Company> companyRepository)
        {
            this.founderRepository = founderRepository;
            this.companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Find([FromBody] SearchQuery search)
        {
            var (filter, _, count) = search;

            var companies = await companyRepository.Entities
                                .OrderByDescending(f => f.UpdateDate ?? f.CreationDate)
                                .Filter(filter)
                                .Take(count)
                                .Select(c => new {Type = nameof(Company), c.Id, c.Name, c.TaxpayerId})
                                .ToListAsync();

            var result = (await founderRepository.Entities
                            .OrderByDescending(f => f.UpdateDate ?? f.CreationDate)
                            .Filter(filter)
                            .Take(count)
                            .Select(f => new { Type = nameof(Founder), f.Id, Name = f.FullName, f.TaxpayerId })
                            .ToListAsync())
                            .Union(companies);

            return Json(result);
        }
    }
}
