using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScriptHook.Extensions;

namespace ExtensionExamples
{
    /// <summary>
    /// Throws an exception when called with argument "throw".
    /// Used to test WebScriptHook exception handling.
    /// </summary>
    class ExceptionPlugin : Extension, ITickable
    {
        bool enable = false;

        public ExceptionPlugin() { }

        public void Tick()
        {
            if (enable)
            {
                throw new Exception("Test exception. Please don't crash WebScriptHook.");
            }
        }

        public override object HandleCall(object[] args)
        {
            if ((string)args[0] == "throw")
            {
                enable = true;
            }
            return args[0];
        }
    }
}
