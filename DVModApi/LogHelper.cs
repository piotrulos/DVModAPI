using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace DVModApi
{
    internal class LogHelper
    {
        internal static void Log(string message)
        {
            UnityModManager.Logger.Log(message, "<color=#00ffffff>[DVModAPI] </color>");
        }
        internal static void LogError(string message)
        {
            UnityModManager.Logger.Log(message, "<color=red>[DVModAPI: Error] </color>");
        }
        internal static void LogWarning(string message)
        {
            UnityModManager.Logger.Log(message, "<color=yellow>[DVModAPI: Warning] </color>");
        }
    }
}
