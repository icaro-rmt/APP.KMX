using System.ComponentModel.DataAnnotations;

namespace APP.KMX.App.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Você deve concordar com os termos de uso")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Você deve concordar com os termos de uso")]
        public bool AgreeToTerms { get; set; }

        public bool RememberMe { get; set; }
    }
}
