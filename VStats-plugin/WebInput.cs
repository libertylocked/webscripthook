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
    delegate void WebFunction(string arg);

    class WebInput
    {
        public string Cmd { get; set; }
        public string Arg { get; set; }

        public void Execute()
        {
            if (string.IsNullOrEmpty(Cmd)) return;

            WebFunction func = GetFunction(Cmd);
            if (func != null)
            {
                func(Arg);
            }
        }

        private WebFunction GetFunction(string funcStr)
        {
            switch (funcStr)
            {
                case "Radio":
                    return Commands.Radio;
                default:
                    return null;
            }
        }
    }

    class Commands
    {
        public static void Radio(string tuneTo)
        {
            int increment;
            if (tuneTo == "1")
            {
                increment = 1;
            }
            else
            {
                increment = -1;
            }
            if (Game.RadioStation == RadioStation.LosSantosRockRadio && increment == -1) Game.RadioStation = RadioStation.SelfRadio;
            else if (Game.RadioStation == RadioStation.SelfRadio && increment == 1) Game.RadioStation = RadioStation.LosSantosRockRadio;
            else
            {
                var radioStations = Enum.GetValues(typeof(RadioStation));
                var newIndex = (Array.IndexOf(radioStations, Game.RadioStation) + increment) % radioStations.Length;
                var newStation = (RadioStation)(radioStations.GetValue(newIndex));
                Game.RadioStation = newStation;
            }
        }
    }
}
