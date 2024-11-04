using System.ComponentModel.DataAnnotations;

namespace APP.KMX.App.Models.ViewModel
{
    public class PasswordViewModel
    {
        [Required]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        [ContainsLowercase(ErrorMessage = "A senha deve conter pelo menos uma letra minúscula.")]
        [ContainsUppercase(ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula.")]
        [ContainsNumber(ErrorMessage = "A senha deve conter pelo menos um número.")]
        public string Password { get; set; }
    }

    public class ContainsLowercaseAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            string password = value as string;
            return password.Any(char.IsLower);
        }
    }
    public class ContainsUppercaseAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            string password = value as string;
            return password.Any(char.IsUpper);
        }
    }
    public class ContainsNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            string password = value as string;
            return password.Any(char.IsDigit);
        }
    }
}
