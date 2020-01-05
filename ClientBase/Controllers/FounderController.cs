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
        public async Task<IActionResult> Edit(Founder founder)
        {
            if (!ModelState.IsValid)
                return View(founder);

            try
            {
                var isNew = founder.FounderId == 0;

                if (isNew)
                    founder.CreationDate = DateTime.Now;
                else
                    founder.UpdateDate = DateTime.Now;

                await repository.UpdateFounderAsync(founder);
                TempData["message"] = $"Данные об учредителе успешно {(isNew ? "добавлены" :"обновлены")}";
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("", $"Невозможно обновить информацию об учредителе.Возможно, учредитель был удален из базы данных");
                return View(founder);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() => View("Edit", new Founder());

        private async Task<Founder> GetFounderAsync(long? id)
        {
            if (id == null)
                return null;

            return await repository.Founders
                                   .SingleOrDefaultAsync(f => f.FounderId == id);
        }
    }
}
