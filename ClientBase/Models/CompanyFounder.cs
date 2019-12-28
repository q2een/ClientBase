using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class CompanyFounder
    {
        public int CompanyId { get; set; }

        public int FounderId { get; set; }

        public Company Company { get; set; }

        public Founder Founder { get; set; }
    }
}
