using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace VStats_plugin
{
    class WebInput
    {
        public string Cmd { get; set; }
        public string Arg { get; set; }

        public void Execute()
        {
            if (string.IsNullOrEmpty(Cmd)) return;

            WebFunction func = FunctionConvert.GetFunction(Cmd);
            if (func != null)
            {
                func(Arg);
            }
        }
    }
}
