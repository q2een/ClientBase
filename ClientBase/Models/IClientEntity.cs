using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public interface IClientEntity
    {
        int Id { get; set; }
        DateTime CreationDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
