using System.ComponentModel.DataAnnotations;

namespace Voicecord.Domain.ViewModels.Group
{
    public class AddToGroupViewModel
    {
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]

        [Required(ErrorMessage = "Ссылка на сервер")]
        public string GroupLink { get; set; }
    }
}
