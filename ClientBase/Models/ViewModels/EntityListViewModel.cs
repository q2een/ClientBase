using ClientBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models.ViewModels
{
    public class ListViewModel<TEntity> : ListViewModel where TEntity: class, IClientEntity
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }
}
