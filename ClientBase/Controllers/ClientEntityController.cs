using ClientBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Controllers
{
    public abstract class ClientEntityController<T> : Controller 
        where T: class, IClientEntity, new()
    {
        private readonly IClientRepository repository;

        public ClientEntityController(IClientRepository repository)
        {
            this.repository = repository;
        }

        protected abstract Task<T> GetEntityAsync(int? id);
    }
}
