using ClientBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models.ViewModels
{
    public class ListViewModel<TEntity> where TEntity: class, IClientEntity
    {
        public IEnumerable<TEntity> Entities { get; set; }

        public string Search { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
