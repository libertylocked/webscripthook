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

        public static string GetVehicleType(Vehicle veh)
        {
            var type = "";
            if (veh.Model.IsBicycle) type = "Bicycle";
            else if (veh.Model.IsBike) type = "Bike";
            else if (veh.Model.IsBoat) type = "Boat";
            else if (veh.Model.IsCar) type = "Car";
            else if (veh.Model.IsHelicopter) type = "Helicopter";
            else if (veh.Model.IsPed) type = "Ped";
            else if (veh.Model.IsPlane) type = "Plane";
            else if (veh.Model.IsQuadbike) type = "Quadbike";
            else if (veh.Model.IsTrain) type = "Train";

            return type;
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
