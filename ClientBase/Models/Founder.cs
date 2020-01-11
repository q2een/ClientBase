using ClientBase.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientBase.Models
{
    public class Founder: IClientEntity
    {
        public int Id { get; set; }

        [Display(Name = "Идентификационный номер налогоплательщика (ИНН)")]
        [Required(ErrorMessage = "Укажите ИНН учредителя")]
        [Range(100000000000, 999999999999, ErrorMessage = "ИНН должен содержать 12 цифр")]
        [TaxpayerId]
        public long TaxpayerId { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Укажите имя учредителя")]
        [RegularExpression(@"^[А-Яа-я][А-Яа-я\'\-]+([А-Яа-я][А-Яа-я\'\-]+)*", ErrorMessage = "Имя не должно содержать цифры или специальные знаки")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Имя должно содержать не менее двух букв")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Укажите фамилию учредителя")]
        [RegularExpression(@"^[А-Яа-я][А-Яа-я\'\-]+([А-Яа-я][А-Яа-я\'\-]+)*", ErrorMessage = "Фамилия не должна содержать цифры или специальные знаки")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать не менее двух букв")]
        public string LastName { get; set; }

        [Display(Name = "Отчество (если имеется)")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Отчество должно содержать не менее двух букв")]
        [RegularExpression(@"^[А-Яа-я][А-Яа-я\'\-]+([А-Яа-я][А-Яа-я\'\-]+)*", ErrorMessage = "Отчество не должно содержать цифры или специальные знаки")]
        public string Patronymic { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();

        [Required]
        [Display(Name = "Дата добавления")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Дата обновления")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Компании учредителя")]
        public ICollection<CompanyFounder> FounderCompanies { get; set; }
    }
}
