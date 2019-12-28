using ClientBase.Models;
using Microsoft.AspNetCore.Mvc;
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

        public ViewResult List()
        {
            return View(repository.Founders);
        }
    }
}
