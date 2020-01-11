using System.ComponentModel.DataAnnotations;

namespace ClientBase.Models.Validation
{
    public class TaxpayerIdAttribute : ValidationAttribute
    {
        private const int PersonTaxpayerIdLength = 12;
        private const int CompanyTaxpayerIdLength = 10; 

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext?.ObjectInstance;
            var valueLength = value.ToString().Length;
            var propertyName = new string[] { validationContext.MemberName };

            switch (instance)
            {
                case Company c when c.IsIndividual == null:
                    return new ValidationResult($"ИНН должен содержать {PersonTaxpayerIdLength} цифр " +
                                                $"для индивидуального предпринимателя и {CompanyTaxpayerIdLength} цифр" +
                                                "для юридического лица", propertyName);

                case Company c when c.IsIndividual == true && valueLength != PersonTaxpayerIdLength:
                    return new ValidationResult($"ИНН индивидуального предпринимателя должен сожержать {PersonTaxpayerIdLength} цифр", propertyName);

                case Company _ when valueLength != CompanyTaxpayerIdLength:
                    return new ValidationResult($"ИНН юридического лица должен содержать {PersonTaxpayerIdLength} цифр", propertyName);

                case Founder _ when valueLength != PersonTaxpayerIdLength:
                    return new ValidationResult($"ИНН должен сожержать {PersonTaxpayerIdLength} цифр", propertyName);

                default:
                    return ValidationResult.Success;
            }
        }
    }
}
