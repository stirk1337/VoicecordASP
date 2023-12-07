using System.ComponentModel.DataAnnotations;

namespace Voicecord.Domain.ViewModels.Group
{
    public class CreateTextChatViewModel
    { 
            [Required(ErrorMessage = "Укажите имя")]
            [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
            [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
            public string NameGroup { get; set; }

            [Required]
            public string CreatorId { get; set; }
            [Required]
            public int GroupLink { get; set; }

    }
}
