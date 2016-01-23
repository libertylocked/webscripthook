using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStats_plugin
{
    class FunctionConvert
    {
        private static Dictionary<string, WebFunction> funcDict;

        public static void Initialize()
        {
            funcDict = new Dictionary<string, WebFunction>();
            funcDict.Add("radio", Commands.Radio);
            funcDict.Add("radioto", Commands.RadioTo);
            funcDict.Add("repair", Commands.FixPlayerVehicle);
            funcDict.Add("wantedlevel", Commands.SetWantedLevel);
            funcDict.Add("health", Commands.SetPlayerHealth);
            funcDict.Add("armor", Commands.SetPlayerArmor);
            funcDict.Add("blackout", Commands.SetBlackout);
            funcDict.Add("time", Commands.ChangeTime);
            funcDict.Add("weather", Commands.ChangeWeather);
            funcDict.Add("maxammo", Commands.MaxAmmo);
            funcDict.Add("spawnvehicle", Commands.SpawnVehicle);
        }

        /// <summary>
        /// Converts a string cmd to a WebFunction
        /// </summary>
        /// <param name="cmd">Command input</param>
        /// <returns>A WebFunction delegate, or null</returns>
        public static WebFunction GetFunction(string cmd)
        {
            if (funcDict == null || funcDict.Count == 0)
            {
                throw new InvalidOperationException("Not initialized");
            }

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
