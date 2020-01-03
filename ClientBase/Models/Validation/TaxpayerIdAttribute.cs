using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models.Validation
{
    public class TaxpayerIdAttribute : ValidationAttribute
    {
        private const int PersonTaxpayerIdLength = 12;
        private const int CompanyTaxpayerIdLength = 10; 

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!long.TryParse(value.ToString(), out var _))
                return new ValidationResult("ИНН должен содержать только цифры");

            var instance = validationContext?.ObjectInstance;
            var valueLength = value.ToString().Length;

            if (instance is Company company)
            {
                var propertyName = new string[] { nameof(company.TaxpayerId) };

                if (company.IsIndividual == null)
                    return new ValidationResult($"ИНН должен содержать {PersonTaxpayerIdLength} цифр " +
                                                $"для индивидуального предпринимателя и {CompanyTaxpayerIdLength} цифр" +
                                                "для юридического лица", propertyName);

                if(company.IsIndividual == true)
                {
                    if (valueLength == PersonTaxpayerIdLength)
                        return ValidationResult.Success;

                    return new ValidationResult($"ИНН индивидуального предпринимателя должен сожержать {PersonTaxpayerIdLength} цифр", propertyName);
                }

                if (valueLength == CompanyTaxpayerIdLength)
                    return ValidationResult.Success;

                return new ValidationResult($"ИНН юридического лица должен содержать {PersonTaxpayerIdLength} цифр", propertyName);
            }
                           
            if(instance is Founder founder)
            {
                if (valueLength == PersonTaxpayerIdLength)
                    return ValidationResult.Success;

                return new ValidationResult($"ИНН должен сожержать {PersonTaxpayerIdLength} цифр", new string[] { nameof(founder.TaxpayerId) });
            }

            return null;
        }
    }
}
