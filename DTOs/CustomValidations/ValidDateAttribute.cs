using System.ComponentModel.DataAnnotations;

namespace DTOs.CustomValidations
{
    /// <summary>
    /// Validation attribute for <see cref="DateTime"/> properties.
    /// Ensures that the value is not <see cref="DateTime.MinValue"/> or <see cref="DateTime.MaxValue"/>.
    /// </summary>
    public class ValidDateAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // If the value is null and it's required, validation should fail.
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.DisplayName} is required.");
            }

            if (value is DateTime date)
            {
                // Reject default min or max values
                if (date == DateTime.MinValue || date == DateTime.MaxValue)
                {
                    return new ValidationResult(
                        $"The field {validationContext.DisplayName} must be a valid date and cannot be MinValue or MaxValue.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
