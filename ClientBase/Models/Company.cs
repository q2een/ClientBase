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

        public long TaxpayerId { get; set; }

        public string Name { get; set; }

        public bool? IsIndividual { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<CompanyFounder> Founders { get; set; }
    }
}
