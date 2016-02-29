using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScriptHook;
using WebScriptHook.Extensions;
using GTA;

namespace ExtensionExamples
{
    public class SubtitleExtension : Extension
    {
        int count = 0;

        public SubtitleExtension() { }

        public override object HandleCall(object[] args)
        {
            // Show the subtitle
            string print = "";
            foreach (var obj in args)
            {
                print += obj.ToString();
            }
            UI.ShowSubtitle(print);
            return "Hey! You called me " + ++count + " times. I showed the subtitle: " + print;
        }

    }
}
