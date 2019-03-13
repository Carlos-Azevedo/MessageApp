using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppInterfaces.Repositories
{
    public interface IMessageAppRepository
    {
        User GetUser(string name);

        void AddUser(User user);

        void UpdateUser(User user);
    }
}
