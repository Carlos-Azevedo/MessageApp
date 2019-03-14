using MessageAppInterfaces.Providers;
using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppProviders
{
    public class MessageTranslator : IMessageTranslator
    {
        public string ToReadString(Message message, DateTime currentDateTime)
        {
            var timeDifferenceString = GetTimeDifferenceString(message, currentDateTime);
            return $"{message.Contents} {timeDifferenceString}";
        }

        public string ToWallString(WallMessage message, DateTime currentDateTime)
        {
            var timeDifferenceString = GetTimeDifferenceString(message, currentDateTime);
            return $"{message.User} - {message.Contents} {timeDifferenceString}";
        }

        private string GetTimeDifferenceString(Message message, DateTime currentDateTime)
        {
            var differenceInTime = currentDateTime.Subtract(message.PostTime);
            if(differenceInTime.TotalSeconds < 60)
            {
                return BuildTimeString((int)differenceInTime.TotalSeconds, "second");
            }
            if (differenceInTime.TotalMinutes < 60)
            {
                return BuildTimeString((int)differenceInTime.TotalMinutes, "minute");
            }

            return BuildTimeString((int)differenceInTime.TotalHours, "hour");
        }

        private string BuildTimeString(int timeValue, string temporalNoun)
        {
            return $"({timeValue} {temporalNoun}{(timeValue > 1 ? "s" : "")} ago)";
        }
    }
}
