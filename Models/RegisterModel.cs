namespace Shace.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан электронный адрес")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Указан некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан никнейм")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Никнейм должен быть от {2} до {1} символов")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан подтверждающий пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public virtual string ConfirmPassword { get; set; }

    }
}
