using ClientBase.Models.Validation;
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

        [Display(Name = "ИНН")]
        [Required(ErrorMessage = "Укажите ИНН")]
        [TaxpayerId]
        public long TaxpayerId { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Укажите ФИО")]
        [RegularExpression("(^[А-Яа-я ,.'-]{2,}$)", ErrorMessage = "Укажите фамилию, имя и отчество (если есть)")]
        public string FullName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<CompanyFounder> Companies { get; set; }
    }
}
