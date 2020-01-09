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
    public abstract class ClientEntityController<TEntity> : Controller 
        where TEntity : class, IClientEntity, new()
    {
        protected IEntityRepository<TEntity> Repository { get; }

        protected virtual int PageSize { get; set; } = 10;

        protected abstract Func<TEntity, object> SelectFromFound { get; }

        protected virtual Action<TEntity> EditHook { get; }

        #region Messages.

        protected abstract string CreationSuccessMessage { get; }
        protected abstract string UpdateSuccessMessage { get; }
        protected abstract string UpdateFailedMessage { get; }
        protected abstract string DeleteSuccessMessage { get; }
        protected abstract string DeleteFailedMessage { get; }

        #endregion

        protected ClientEntityController(IEntityRepository<TEntity> repository)
        {
            this.Repository = repository;
        }

        public virtual async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            return await List(search, pageNumber);
        }

        public virtual async Task<IActionResult> List(string search, int pageNumber = 1)
        {
            var entities = GetOrdered();

            if (!string.IsNullOrEmpty(search))
            {
                entities = GetFiltered(entities, search);
            }

            var pages = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = entities.Count()
            };

            entities = entities.Skip((pageNumber - 1) * PageSize)
                               .Take(PageSize);

            return View(new ListViewModel<TEntity>
            {
                Entities = await entities.ToListAsync(),
                PagingInfo = pages,
                Search = search
            });
        }

        public virtual async Task<IActionResult> Details(int? id) => await RepresentEntity("Details", id);
        
        public virtual async Task<IActionResult> Edit(int? id) => await RepresentEntity("Edit", id);

        [HttpPost]
        public virtual async Task<IActionResult> Edit(TEntity entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            try
            {
                var isNew = entity.Id == 0;

                if (isNew)
                    entity.CreationDate = DateTime.Now;
                else
                    entity.UpdateDate = DateTime.Now;

                EditHook?.Invoke(entity);

                await Repository.UpdateAsync(entity);
                TempData["message"] = isNew ? CreationSuccessMessage : UpdateSuccessMessage;
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", UpdateFailedMessage);
                return View(entity);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() => View("Edit", new TEntity());

        public async Task<IActionResult> Delete(int? id) => await RepresentEntity("Delete", id);

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await Repository.DeleteAsync(id);
                TempData["message"] = DeleteSuccessMessage;
            }
            catch (DbUpdateException e)
            {
                ViewData["ErrorMessage"] = DeleteFailedMessage;
                return View(nameof(Delete), id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> Find([FromBody] SearchQuery searchQuery)
        {
            var (nameOrTaxpayerId, except, count) = searchQuery;
            except ??= new int[0];

            var entities = (await GetFiltered(Repository.Entities, searchQuery.Text)
                                  .Where(e => !except.Contains(e.Id))
                                  .Take(searchQuery.Count)
                                  .ToArrayAsync())
                                  .Select(SelectFromFound);

            return Json(entities);
        }

        protected abstract IQueryable<TEntity> GetOrdered();

        protected abstract IQueryable<TEntity> GetFiltered(IQueryable<TEntity> entities, string search);

        protected virtual async Task<TEntity> GetEntityAsync(int? id)
        {
            if (id == null)
                return null;

            return await Repository.Entities.SingleOrDefaultAsync(e => e.Id == id);
        }

        private async Task<IActionResult> RepresentEntity(string viewName, int? entityId)
        {
            var entity = await GetEntityAsync(entityId);
            return entity != null ? View(viewName, entity) : NotFound() as IActionResult;
        }
    }
}
