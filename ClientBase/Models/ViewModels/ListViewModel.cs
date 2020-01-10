using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models.ViewModels
{
    public abstract class ListViewModel
    {
        public string Search { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
