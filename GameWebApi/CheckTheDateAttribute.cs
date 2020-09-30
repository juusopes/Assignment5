using System;
using System.ComponentModel.DataAnnotations;

public class CheckTheDateAttribute : ValidationAttribute
{
    
    public string GetErrorMessage() =>
        "Select another date.";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime datetime = (DateTime)validationContext.ObjectInstance;

        if (datetime < DateTime.UtcNow)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}