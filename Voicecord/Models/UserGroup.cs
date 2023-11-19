using Microsoft.EntityFrameworkCore;

namespace Voicecord.Models
{
    public class UserGroup: DbContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LinkImageGroup {  get; set; }
        public List<User>


    }
}
