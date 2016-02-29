using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using WebScriptHook.Extensions;

namespace ExtensionExamples
{
    class ClockPlugin : Extension, ITickable
    {
        bool on = false;
        UIText timeText;

        public ClockPlugin() 
        {
            timeText = new UIText("", new Point(10, 10), 0.5f, Color.Gray);
        }

        public void Tick()
        {
            if (on)
            {
                timeText.Caption = DateTime.Now.ToString();
                timeText.Draw();
            }
        }

        public override object HandleCall(object[] args)
        {
            if (args.Length > 0 && (string)args[0] == "on")
            {
                on = true;
            }
            else
            {
                on = false;
            }
            return "Clock is " + (on ? "on" : "off");
        }
    }
}
