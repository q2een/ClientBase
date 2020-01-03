using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Display(Name = "ИНН")]
        public long TaxpayerId { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Тип")]
        public bool? IsIndividual { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<CompanyFounder> Founders { get; set; }
    }
}
