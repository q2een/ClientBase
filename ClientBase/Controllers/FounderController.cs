using ClientBase.Models;
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

        public FounderController(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index() => await List();

        public async Task<IActionResult> List()
        {
            return View("List", await repository.Founders.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            var founder = await GetFounder(id);

            if (founder == null)
                return NotFound();

            return View(founder);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var founder = await GetFounder(id);

            if (founder == null)
                return NotFound();

            return View(founder);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Founder founder)
        {
            if (!ModelState.IsValid)
                return View(founder);

            try
            {
                var result = await repository.UpdateFounderAsync(founder);

                if (result)
                    TempData["message"] = "Данные о учредителе успешно обновлены";
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("", $"Невозможно обновить информацию об учредителе.{Environment.NewLine}Возможно, учредитель был удален из базы данных");
                return View(founder);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Founder> GetFounder(long? id)
        {
            if (id == null)
                return null;

            return await repository.Founders
                                   .SingleOrDefaultAsync(f => f.FounderId == id);
        }
    }
}
