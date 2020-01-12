using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public static class Extensions
    {
        public static IQueryable<Founder> Filter(this IQueryable<Founder> founders, string search)
        {
            search = search.RebuildSearch();
            return founders.Where(f => f.LastName.Contains(search) ||
                                       f.FirstName.Contains(search) ||
                                       (f.LastName + " " + f.FirstName + " " + f.Patronymic).Trim().Contains(search) ||
                                       (f.FirstName + " " + f.LastName).Trim().Contains(search) ||
                                       f.TaxpayerId.ToString().Contains(search));
        }

        public static IQueryable<Company> Filter(this IQueryable<Company> companies, string search)
        {
            search = search.RebuildSearch();
            return companies.Where(c => c.Name.Contains(search) || c.TaxpayerId.ToString().Contains(search));
        }

        private static string RebuildSearch(this string search)
        {
            return string.Join(' ', search.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
