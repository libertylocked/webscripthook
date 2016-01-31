using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VStats_plugin
{
    public static class Logger
    {
        public static void Log(object message)
        {
#if DEBUG
            File.AppendAllText(@"VStats.log", DateTime.Now + " : " + message + Environment.NewLine);
#endif
        }
    }
}
