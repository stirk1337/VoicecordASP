using System.ComponentModel.DataAnnotations;

namespace Voicecord.ViewModels.Group
{
    public class AddToGroupViewModel
    {
        [MaxLength(20, ErrorMessage = "Название группы должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Название группы  должно иметь длину больше 3 символов")]

        [Required(ErrorMessage = "Ссылка на группу")]
        public string GroupLink { get; set; }
    }
}
