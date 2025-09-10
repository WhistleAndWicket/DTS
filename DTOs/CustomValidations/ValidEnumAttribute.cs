using System.ComponentModel.DataAnnotations;

namespace DTOs.CustomValidations
{
    /// <summary>
    /// A custom validation attribute that checks if an enum value is defined in the <paramref name="enumType"/>.
    /// </summary>
    /// <remarks>
    /// This attribute can be used to validate that the enum value provided is a valid value in the <paramref name="enumType"/>.
    /// It ensures that the value is defined in the enum.
    /// </remarks>
    /// <param name="enumType">The enum type to validate the value against.</param>
    public class ValidEnumAttribute(Type enumType) : ValidationAttribute
    {
        /// <summary>
        /// The enum type to validate the value against.
        /// </summary>
        private readonly Type _enumType = enumType;

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // If the value is null and it's required, validation should fail.
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.DisplayName} is required.");
            }

            // Check if it's an enum and ensure it's a valid value
            if (_enumType.IsEnum && value is Enum enumValue)
            {
                // Check if the enum value is defined in the enum type.
                if (!Enum.IsDefined(_enumType, enumValue))
                {
                    return new ValidationResult($"The value {enumValue} is not a valid value for {_enumType.Name}.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
