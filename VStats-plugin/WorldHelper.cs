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
    class WorldHelper
    {
        static String[] WeatherNames = { "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS" };

        public static string GetVehicleClass(Vehicle veh)
        {
            int vehClass = Function.Call<int>(Hash.GET_VEHICLE_CLASS, veh.Handle);
            return ((VehicleClass)vehClass).ToString();
        }

        public static Weather GetWeather()
        {
            for (int i = 0; i < WeatherNames.Length; i++)
            {
                int weatherHash = Function.Call<int>((Hash)0x564B884A05EC45A3);
                if (weatherHash == Function.Call<int>(Hash.GET_HASH_KEY, WeatherNames[i]))
                {
                    return (Weather)i;
                }
            }
            return Weather.Christmas;
        }
    }
}
