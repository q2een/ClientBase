using ClientBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IClientRepository repository;

        public CompanyController(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> List(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var companies = repository.Companies;

            return View(await companies.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            var company = await GetCompanyAsync(id);

            if (company == null)
                return NotFound();

            return View(company);
        }

        private async Task<Company> GetCompanyAsync(long? id)
        {
            if (id == null)
                return null;

            return await repository.Companies
                                   .SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var company = await GetCompanyAsync(id);

            if (company == null)
                return NotFound();

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            if (!ModelState.IsValid)
                return View(company);

            try
            {
                var isNew = company.Id == 0;

                if (isNew)
                    company.CreationDate = DateTime.Now;
                else
                    company.UpdateDate = DateTime.Now;

                EditHook(company);

                await repository.UpdateCompanyAsync(company);
                TempData["message"] = $"Данные об учредителе успешно {(isNew ? "добавлены" : "обновлены")}";
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", $"Невозможно обновить информацию об учредителе. Возможно, учредитель был удален из базы данных");
                return View(company);
            }

            return RedirectToAction(nameof(Index));
        }

        protected void EditHook(Company company)
        {
            foreach (var companyFounder in company.CompanyFounders)
            {
                companyFounder.Company = company;
                companyFounder.CompanyId = company.Id;
            }
        }


    }
}
