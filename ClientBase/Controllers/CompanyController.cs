using ClientBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Controllers
{
    public class CompanyController : ClientEntityController<Company>
    {
        protected override Func<Company, object> SelectFromFound => throw new NotImplementedException();

        protected override string CreationSuccessMessage => throw new NotImplementedException();

        protected override string UpdateSuccessMessage => throw new NotImplementedException();

        protected override string UpdateFailedMessage => throw new NotImplementedException();

        protected override string DeleteSuccessMessage => throw new NotImplementedException();

        protected override string DeleteFailedMessage => throw new NotImplementedException();

        protected override Action<Company> EditHook => EditHook1;

        public CompanyController(IEntityRepository<Company> repository) 
            :base(repository)
        {
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(Company company)
        //{
        //    if (!ModelState.IsValid)
        //        return View(company);

        //    try
        //    {
        //        var isNew = company.Id == 0;

        //        if (isNew)
        //            company.CreationDate = DateTime.Now;
        //        else
        //            company.UpdateDate = DateTime.Now;

        //        EditHook(company);

        //        await repository.UpdateCompanyAsync(company);
        //        TempData["message"] = $"Данные об учредителе успешно {(isNew ? "добавлены" : "обновлены")}";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        ModelState.AddModelError("", $"Невозможно обновить информацию об учредителе. Возможно, учредитель был удален из базы данных");
        //        return View(company);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        protected void EditHook1(Company company)
        {
            foreach (var companyFounder in company.CompanyFounders)
            {
                companyFounder.Company = company;
                companyFounder.CompanyId = company.Id;
            }
        }

        protected override IQueryable<Company> GetOrdered()
        {
            return Repository.Entities;
        }

        protected override IQueryable<Company> GetFiltered(IQueryable<Company> companies, string search)
        {
            return companies.Where(c => c.Name.Contains(search) || c.TaxpayerId.ToString().Contains(search));
        }
    }
}
