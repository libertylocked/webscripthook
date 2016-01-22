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
            switch (funcStr.ToLower())
            {
                case "radio":
                    return Commands.Radio;
                case "radioto":
                    return Commands.RadioTo;
                case "repair":
                    return Commands.FixPlayerVehicle;
                case "wantedlevel":
                    return Commands.SetWantedLevel;
                case "health":
                    return Commands.SetPlayerHealth;
                case "armor":
                    return Commands.SetPlayerArmor;
                case "blackout":
                    return Commands.SetBlackout;
                case "time":
                    return Commands.ChangeTime;
                case "weather":
                    return Commands.ChangeWeather;
                case "maxammo":
                    return Commands.MaxAmmo;
                case "spawnvehicle":
                    return Commands.SpawnVehicle;
                default:
                    return null;
            }
        }
    }
}
