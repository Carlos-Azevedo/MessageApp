using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppInterfaces.Providers
{
    public interface IMessageAppCommandParser
    {
        Command TranslateUserInputToCommand(string userInput);
    }
}
