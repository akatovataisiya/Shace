namespace Shace.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "• Не указан электронный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "• Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
