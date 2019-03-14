using MessageAppInterfaces.Repositories;
using MessageAppModels;
using System;
using System.Collections.Generic;

namespace MessageAppRepositories
{
    public class MessageAppRepository : IMessageAppRepository
    {
        private readonly Dictionary<string, User> Storage;

        public MessageAppRepository(Dictionary<string, User> storageDictionary)
        {
            Storage = storageDictionary;
        }

        public void AddUser(User user)
        {
            if(!Storage.ContainsKey(user.Name.ToLower()))
            {
                Storage.Add(user.Name.ToLower(), user);
            }
        }

        public User GetUser(string name)
        {
            if(Storage.ContainsKey(name.ToLower()))
            {
                return Storage[name.ToLower()];
            }
            else
            {
                return null;
            }
        }

        public void UpdateUser(User user)
        {
            if (Storage.ContainsKey(user.Name.ToLower()))
            {
                Storage[user.Name.ToLower()] = user;
            }
        }
    }
}
