namespace Shace.Models
{
    public class SettingsModel
    {

        [StringLength(20, MinimumLength = 6, ErrorMessage = "• Никнейм должен быть от {2} до {1} символов")]
        public string? ShortName { get; set; }

        public bool Privacy { get; set; }

        public long? Phone { get; set; }

        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "• Пароль должен быть от {2} до {1} символов")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "• Пароль должен быть от {2} до {1} символов")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "• Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "• Указан некорректный адрес")]
        public string? Email { get; set; }

        public DateTime? BDay { get; set; }

        [StringLength(100, ErrorMessage = "• Описание должно быть не более {1} символов")]
        public string? Description { get; set; }

        public string? Location { get; set; }

        public IFormFile? image { get; set; }
    }
}
