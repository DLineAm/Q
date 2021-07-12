using System.Globalization;
using System.Windows.Controls;

namespace Q.Services
{
    public class LengthRule : ValidationRule
    {
        public int MinLength { get; set; } = 6;

        public int MaxLength { get; set; } = 16;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = (string) value;
            if(str?.Length < MinLength || str?.Length > MaxLength )
                return new ValidationResult(false, "Логин должен иметь длинну не менее 6 и не более 16 символов");
            return new ValidationResult(true, null);
        }
    }
}