using System.ComponentModel.DataAnnotations;

namespace ChatApi.DTO.UserDTO
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [MinLength(1, ErrorMessage = "Имя должно содержать хотя бы один символ")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(
            @"^(?=.{8,}$)(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Пароль должен быть не менее 8 символов, содержать латинские буквы, цифры и спецсимволы"
        )]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }

    public record class UserRegistrationSender
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string AvatarImgBase64 { get; set; } = string.Empty;
    }

}
