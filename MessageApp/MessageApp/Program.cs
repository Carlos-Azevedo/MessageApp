using MessageAppInterfaces.Providers;
using MessageAppInterfaces.Repositories;
using MessageAppInterfaces.Services;
using MessageAppModels;
using MessageAppProviders;
using MessageAppRepositories;
using MessageAppServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageDictionary = new Dictionary<string, User>();
            StructureMap.Container contanier = new StructureMap.Container(_ =>
            {
                _.For<IDateTimeProvider>().Use<DateTimeProvider>();
                _.For<IMessageTranslator>().Use<MessageTranslator>();
                _.For<IMessageAppCommandParser>().Use<MessageAppCommandParser>();
                _.For<IMessageAppService>().Use<MessageAppService>();
                _.For<IMessageAppRepository>().Use<MessageAppRepository>().Ctor<Dictionary<string, User>>().Is(storageDictionary);
                _.For<MessageAppController>().Use<MessageAppController>();
            });
            var messageAppController = contanier.GetInstance<MessageAppController>();

            while (true)
            {
                var userInput = Console.ReadLine();
                if (userInput == "exit")
                { break; }
                messageAppController.ExecuteCommand(userInput);
            }
        }
    }
}
