using System.ComponentModel.DataAnnotations;

namespace ChatApi.DTO.UserDTO
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(
            @"^(?=.{8,}$)(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Пароль должен быть не менее 8 символов, содержать латинские буквы, цифры и спецсимволы"
        )]
        public string Password { get; set; } = string.Empty;
    }
}
