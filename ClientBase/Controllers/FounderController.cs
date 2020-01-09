﻿using ClientBase.Infrastructure;
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
    public class FounderController : Controller
    {
        private readonly IClientRepository repository;

        public int PageSize { get; set; } = 3;

        public FounderController(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index() => await List("");

        public async Task<IActionResult> List(string search, int pageNumber = 1)
        {
            var founders = repository.Founders;

            if (!string.IsNullOrEmpty(search))
            {
                founders = founders.Where(f => f.LastName.Contains(search) || 
                                               f.TaxpayerId.ToString().Contains(search));
            }

            var pages = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = founders.Count()
            };

            founders = founders.Skip((pageNumber - 1) * PageSize)
                               .Take(PageSize);

            return View(new ListViewModel<Founder>
            {
                Entities = await founders.ToListAsync(),
                PagingInfo = pages,
                Search = search
            });
        }

        public async Task<IActionResult> Details(long? id)
        {
            var founder = await GetFounderAsync(id);

            if (founder == null)
                return NotFound();

            return View(founder);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var founder = await GetFounderAsync(id);

            if (founder == null)
                return NotFound();

            return View(founder);
        }

        [HttpPost]
        public async Task<JsonResult> Find([FromBody] SearchQuery query)
        {
            var (nameOrTaxpayerId, except, count) = query;
            except ??= new int[0];

            var founders = (await repository.Founders.Where(f => (!except.Contains(f.Id)) && 
                                                                   (f.FullName.Contains(nameOrTaxpayerId) ||
                                                                   f.TaxpayerId.ToString().Contains(nameOrTaxpayerId)))
                .Take(count)
                .ToArrayAsync())
                .Select(f => new { Id = f.Id, Name = f.FullName, f.TaxpayerId });

            return Json(founders);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Founder founder)
        {
            if (!ModelState.IsValid)
                return View(founder);

            try
            {
                var isNew = founder.Id == 0;

                if (isNew)
                    founder.CreationDate = DateTime.Now;
                else
                    founder.UpdateDate = DateTime.Now;

                await repository.UpdateFounderAsync(founder);
                TempData["message"] = $"Данные об учредителе успешно {(isNew ? "добавлены" :"обновлены")}";
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("", $"Невозможно обновить информацию об учредителе. Возможно, учредитель был удален из базы данных");
                return View(founder);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() => View("Edit", new Founder());

        public async Task<IActionResult> Delete(int? id)
        {
            var founder = await GetFounderAsync(id);

            if (founder == null)
                return NotFound();

            return View(founder);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await repository.DeleteFounderAsync(id);
                TempData["message"] = $"Данные об учредителе успешно удалены";
            }
            catch (DbUpdateException e)
            {
                ViewData["ErrorMessage"] = "Ошибка при удалении учредителя из базы данны." + 
                               "Попробуйте снова, и если проблема повторится, обратитесь к системному администатору";

                return View(nameof(Delete), id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Founder> GetFounderAsync(long? id)
        {
            if (id == null)
                return null;

            return await repository.Founders
                                   .SingleOrDefaultAsync(f => f.Id == id);
        }
    }
}
