using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppModels
{
    public class Command
    {
        public string User { get; set; }
        public string Argument { get; set; }
        public CommandEnums MethodCall { get; set; }
    }
}
