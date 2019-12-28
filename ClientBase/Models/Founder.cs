using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class Founder
    {
        public int FounderId { get; set; }

        public long TaxpayerId { get; set; }

        public string FullName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<CompanyFounder> Companies { get; set; }
    }
}
