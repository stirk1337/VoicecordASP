using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voicecord.Models;

namespace Voicecord.Tests.Helpers
{
    internal class TestDataHelper
    {
        public static List<ApplicationUser> GetFakeUsersList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser(){ Email= "email1@email.ru",Id= 1,Password="pass1",UserName="user1"},
                new ApplicationUser(){ Email= "email2@email.ru",Id= 2,Password="pass2",UserName="user2"},
                new ApplicationUser(){ Email= "email3@email.ru",Id= 3,Password="pass3",UserName="user3"},
                new ApplicationUser(){ Email= "email4@email.ru",Id= 4,Password="pass4",UserName="user4"},
                new ApplicationUser(){ Email= "email5@email.ru",Id= 5,Password="pass5",UserName="user5"},
            };
        }
        public static List<UserGroup> GetFakeGroupList()
        {
            return new List<UserGroup>()
            {
                new UserGroup() { Id = 1, Chats = new List<Chat>(), LinkImageGroup="Link1",Name="Name1",Users=GetFakeUsersList(), Voices= new List<VoiceChat>()},
                new UserGroup() { Id = 2, Chats = new List<Chat>(), LinkImageGroup="Link2",Name="Name2",Users=GetFakeUsersList().Where(x=>x.Id<=3).ToList(), Voices= new List<VoiceChat>()}
            };
        }
    }
}
