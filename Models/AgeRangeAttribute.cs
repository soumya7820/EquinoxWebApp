using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Equinox.Models
{
    public class AgeRangeAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                var today = DateTime.Today;
                int age = today.Year - date.Year;
                if (date.Date > today.AddYears(-age)) age--;

                if (age >= _minAge && age <= _maxAge)
                    return ValidationResult.Success!;
            }

            return new ValidationResult(
                ErrorMessage ?? $"Age must be between {_minAge} and {_maxAge} years.");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
                context.Attributes.Add("data-val", "true");

            context.Attributes.Add("data-val-agerange", ErrorMessage ?? $"Age must be between {_minAge} and {_maxAge} years.");
            context.Attributes.Add("data-val-agerange-min", _minAge.ToString());
            context.Attributes.Add("data-val-agerange-max", _maxAge.ToString());
        }
    }
}
