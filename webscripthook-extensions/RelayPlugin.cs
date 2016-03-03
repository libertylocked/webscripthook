using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScriptHook.Extensions;

namespace ExtensionExamples
{
    /// <summary>
    /// This is used to test extension-to-extension calling
    /// It relays messages to and from subtitle plugin
    /// </summary>
    class RelayPlugin : Extension
    {
        public RelayPlugin() { }

        public override object HandleCall(object[] args)
        {
            string subtitlePluginID = this.GetType().Assembly.GetName().Name + "." + typeof(SubtitlePlugin).FullName;
            return CallExtension(subtitlePluginID, args);
        }
    }
}
