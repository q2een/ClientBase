using System.ComponentModel.DataAnnotations;

namespace ClientBase.Models.Validation
{
    public class CompanyFoundersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext?.ObjectInstance;
            var propertyName = new string[] { nameof(Company.CompanyFounders) };

            switch (instance)
            {
                case Company c when (c.CompanyFounders?.Count ?? 0) == 0:
                    return new ValidationResult("У компании должен быть, как минимум, один основатель");

                case Company c when c.IsIndividual == true && c.CompanyFounders.Count == 1:
                    return ValidationResult.Success;
                
                case Company c when c.IsIndividual == true && c.CompanyFounders.Count != 1:
                    return new ValidationResult("У индивидуального предпринимателя должен быть один учредитель", propertyName);

                case Company c when c.IsIndividual == null:
                    return new ValidationResult("Необходимо указать тип компании", new[] { nameof(Company.IsIndividual) });

                default:
                    return ValidationResult.Success;
            }
        }
    }
}
