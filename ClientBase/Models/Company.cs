using ClientBase.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientBase.Models
{
    public class Company : IClientEntity
    {
        public int Id { get; set; }

        [Display(Name = "Идентификационный номер налогоплательщика (ИНН)")]
        [Required(ErrorMessage = "Укажите ИНН")]
        [Range(1000000000, 999999999999, ErrorMessage = "ИНН должен содержать 10 цифр для юридического лица (или 12 для ИП)")]
        [TaxpayerId]
        public long TaxpayerId { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Укажите наименование компании")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Имя компании должно содержать не менее 5 и не более 80 символов")]
        public string Name { get; set; }

        [Display(Name = "Тип компании (ИП или юридическое лицо)")]
        [Required(ErrorMessage = "Необходимо указать тип компании")]
        public bool? IsIndividual { get; set; }

        [Required]
        [Display(Name = "Дата добавления")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Дата обновления")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Учредители")]
        public ICollection<CompanyFounder> CompanyFounders { get; set; }
    }
}
