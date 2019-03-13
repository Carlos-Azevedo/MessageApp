using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppModels
{
    public class User
    {
        public string Name { get; set; }
        public List<string> Following { get; set; }
        public List<Message> Messages { get; set; }

        public User(string user)
        {
            Name = user;
            Following = new List<string>();
            Messages = new List<Message>();
        }
    }
}
