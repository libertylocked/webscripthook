using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStats_plugin
{
    class FunctionConvert
    {
        private static Dictionary<string, WebFunction> funcDict = new Dictionary<string, WebFunction>
        {
            {"radio", Commands.Radio},
            {"radioto", Commands.RadioTo},
            {"repair", Commands.FixPlayerVehicle},
            {"wantedlevel", Commands.SetWantedLevel},
            {"health", Commands.SetPlayerHealth},
            {"armor", Commands.SetPlayerArmor},
            {"blackout", Commands.SetBlackout},
            {"time", Commands.ChangeTime},
            {"weather", Commands.ChangeWeather},
            {"maxammo", Commands.MaxAmmo},
            {"spawnvehicle", Commands.SpawnVehicle},
            {"spawnped", Commands.SpawnPed},
            {"giveweapon", Commands.GiveWeapon},
            {"showsavemenu", Commands.ShowSaveMenu},
            {"echo", Commands.Echo},
            {"native", Commands.CallNative},
        };

        /// <summary>
        /// Converts a string cmd to a WebFunction
        /// </summary>
        /// <param name="cmd">Command input</param>
        /// <returns>A WebFunction delegate, or null</returns>
        public static WebFunction GetFunction(string cmd)
        {
            WebFunction func;
            if (funcDict.TryGetValue(cmd.ToLower(), out func))
            {
                return func;
            }
            else
            {
                return null;
            }
        }
    }
}
