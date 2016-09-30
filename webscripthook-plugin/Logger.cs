using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebScriptHook
{
    class Logger
    {
        public static string Location
        {
            get;
            set;
        }

        public static bool Enable
        {
            get;
            set;
        }

        public static void Log(object message)
        {
            if (Enable)
            {
                File.AppendAllText(Location, DateTime.Now + " : " + message + Environment.NewLine);
            }
        }
    }
}
