namespace Senior2.Api.Models
{
    // Domain/Common/Validation/DateRangeAttribute.cs
    using System.ComponentModel.DataAnnotations;


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startProperty;
        private readonly string _endProperty;

        public DateRangeAttribute(string startProperty, string endProperty)
        {
            _startProperty = startProperty;
            _endProperty = endProperty;
            ErrorMessage = $"{endProperty} must be greater than {startProperty}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            var type = validationContext.ObjectType;
            var startProp = type.GetProperty(_startProperty);
            var endProp = type.GetProperty(_endProperty);

            if (startProp is null || endProp is null)
                return new ValidationResult("Invalid date range configuration.");

            var startValue = startProp.GetValue(value);
            var endValue = endProp.GetValue(value);

            if (startValue is DateTimeOffset start && endValue is DateTimeOffset end)
            {
                return end > start
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}
