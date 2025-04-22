using System.ComponentModel.DataAnnotations;

namespace ChatApi.DTO.UserDTO
{
    public record class UserRegistration
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [MinLength(1, ErrorMessage = "Имя должно содержать хотя бы один символ")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(
            @"^(?=.{8,}$)(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Пароль должен быть не менее 8 символов, содержать латинские буквы, цифры и спецсимволы"
        )]
        public string Password { get; init; } = string.Empty;

        public string AvatarImgBase64 { get; init; } = string.Empty;

    }
}
