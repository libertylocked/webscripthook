using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;

namespace VStats_plugin
{
    class Commands
    {
        public static void Radio(string tuneTo, object[] args)
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
            if (Game.RadioStation == RadioStation.LosSantosRockRadio && increment == -1)
            {
                // TODO: Fix tuning to self radio
                Game.RadioStation = RadioStation.VinewoodBoulevardRadio;
            }
            else if (Game.RadioStation == RadioStation.SelfRadio && increment == 1)
            {
                Game.RadioStation = RadioStation.LosSantosRockRadio;
            }
            else
            {
                var radioStations = Enum.GetValues(typeof(RadioStation));
                var newIndex = (Array.IndexOf(radioStations, Game.RadioStation) + increment) % radioStations.Length;
                var newStation = (RadioStation)(radioStations.GetValue(newIndex));
                Game.RadioStation = newStation;
            }
        }

        public static void RadioTo(string arg, object[] args)
        {
            Function.Call(Hash.SET_RADIO_TO_STATION_NAME, arg);
        }

        public static void FixPlayerVehicle(string arg, object[] args)
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.Repair();
            }
        }

        public static void SetWantedLevel(string arg, object[] args)
        {
            int stars;
            if (int.TryParse(arg, out stars) && stars >= 0 && stars <= 5)
            {
                Game.Player.WantedLevel = stars;
            }
        }

        public static void SetPlayerHealth(string arg, object[] args)
        {
            int hp;
            if (int.TryParse(arg, out hp) && hp >= -1 && hp <= Game.Player.Character.MaxHealth)
            {
                Game.Player.Character.Health = hp;
            }
        }

        public static void SetPlayerArmor(string arg, object[] args)
        {
            int ar;
            if (int.TryParse(arg, out ar) && ar >= 0 && ar <= 100)
            {
                Game.Player.Character.Armor = ar;
            }
        }

        public static void SetBlackout(string arg, object[] args)
        {
            bool on;
            if (bool.TryParse(arg, out on))
            {
                World.SetBlackout(on);
            }
        }

        public static void ChangeTime(string arg, object[] args)
        {
            TimeSpan ts;
            if (TimeSpan.TryParse(arg, out ts))
            {
                World.CurrentDayTime = ts;
            }
        }

        public static void ChangeWeather(string arg, object[] args)
        {
            Weather weather;
            if (Enum.TryParse<Weather>(arg, out weather))
            {
                World.Weather = weather;
            }
        }

        public static void MaxAmmo(string arg, object[] args)
        {
            Weapon currWep = Game.Player.Character.Weapons.Current;
            currWep.Ammo = currWep.MaxAmmo;
        }

        public static void SpawnVehicle(string arg, object[] args)
        {
            VehicleHash vehHash;
            if (Enum.TryParse<VehicleHash>(arg, out vehHash))
            {
                World.CreateVehicle(new Model(vehHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
            }
        }

        public static void SpawnPed(string arg, object[] args)
        {
            PedHash pedHash;
            if (Enum.TryParse<PedHash>(arg, out pedHash))
            {
                World.CreatePed(new Model(pedHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
            }
        }

        public static void GiveWeapon(string arg, object[] args)
        {
            WeaponHash wepHash;
            if (Enum.TryParse<WeaponHash>(arg, out wepHash))
            {
                Game.Player.Character.Weapons.Give(wepHash, 9999, true, true);
            }
        }

        public static void ShowSaveMenu(string arg, object[] args)
        {
            Game.ShowSaveMenu();
        }

        public static void CallNative(string arg, object[] args)
        {
            // Arg is the native hash
            Hash nativeHash;
            if (!Enum.TryParse<Hash>(arg, out nativeHash)) return;

            // Build arguments
            List<InputArgument> nativeArgs = new List<InputArgument>();
            for (int i = 0; i < args.Length; i++)
            {
                dynamic parameter = args[i];
                if (parameter.GetType() == typeof(System.Int64))
                {
                    parameter = (int)parameter;
                }
                else if (parameter.GetType() == typeof(System.String))
                {
                    if (parameter.StartsWith("{float}"))
                    {
                        parameter = float.Parse(((string)parameter).Substring(7));
                    }
                }
                nativeArgs.Add(parameter);
                Logger.Log("Args[" + i + "]: " + parameter.ToString() + " " + parameter.GetType());
            }

            Logger.Log("Native called: " + nativeHash.ToString());
            Function.Call(nativeHash, nativeArgs.ToArray());
        }
    }
}
