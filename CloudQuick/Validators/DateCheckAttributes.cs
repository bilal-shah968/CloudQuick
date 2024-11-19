using System.ComponentModel.DataAnnotations;

namespace CloudQuick.Validators
{
    public class DateCheckAttributes : ValidationAttribute

    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if (date < DateTime.Now)
            {
                return new ValidationResult("The date must b greater than or equal to today date ");
            }
            return ValidationResult.Success;
        }


    }
}
