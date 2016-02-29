using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using WebScriptHook.Extensions;

namespace WebScriptHook
{
    class Commands
    {
        public static object Radio(string tuneTo, object[] args)
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
            return null;
        }

        public static object RadioTo(string arg, object[] args)
        {
            Function.Call(Hash.SET_RADIO_TO_STATION_NAME, arg);
            return null;
        }

        public static object FixPlayerVehicle(string arg, object[] args)
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.Repair();
            }
            return null;
        }

        public static object SetWantedLevel(string arg, object[] args)
        {
            int stars;
            if (int.TryParse(arg, out stars) && stars >= 0 && stars <= 5)
            {
                Game.Player.WantedLevel = stars;
            }
            return null;
        }

        public static object SetPlayerHealth(string arg, object[] args)
        {
            int hp;
            if (int.TryParse(arg, out hp) && hp >= -1 && hp <= Game.Player.Character.MaxHealth)
            {
                Game.Player.Character.Health = hp;
            }
            return null;
        }

        public static object SetPlayerArmor(string arg, object[] args)
        {
            int ar;
            if (int.TryParse(arg, out ar) && ar >= 0 && ar <= 100)
            {
                Game.Player.Character.Armor = ar;
            }
            return null;
        }

        public static object SetBlackout(string arg, object[] args)
        {
            bool on;
            if (bool.TryParse(arg, out on))
            {
                World.SetBlackout(on);
            }
            return null;
        }

        public static object ChangeTime(string arg, object[] args)
        {
            TimeSpan ts;
            if (TimeSpan.TryParse(arg, out ts))
            {
                World.CurrentDayTime = ts;
            }
            return null;
        }

        public static object ChangeWeather(string arg, object[] args)
        {
            Weather weather;
            if (Enum.TryParse<Weather>(arg, out weather))
            {
                World.Weather = weather;
            }
            return null;
        }

        public static object MaxAmmo(string arg, object[] args)
        {
            Weapon currWep = Game.Player.Character.Weapons.Current;
            currWep.Ammo = currWep.MaxAmmo;
            return null;
        }

        public static object SpawnVehicle(string arg, object[] args)
        {
            VehicleHash vehHash;
            if (Enum.TryParse<VehicleHash>(arg, out vehHash))
            {
                return World.CreateVehicle(new Model(vehHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5).Handle;
            }
            else
            {
                return null;
            }
        }

        public static object SpawnPed(string arg, object[] args)
        {
            PedHash pedHash;
            if (Enum.TryParse<PedHash>(arg, out pedHash))
            {
                return World.CreatePed(new Model(pedHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5).Handle;
            }
            else
            {
                return null;
            }
        }

        public static object GiveWeapon(string arg, object[] args)
        {
            WeaponHash wepHash;
            if (Enum.TryParse<WeaponHash>(arg, out wepHash))
            {
                Game.Player.Character.Weapons.Give(wepHash, 9999, true, true);
            }
            return null;
        }

        public static object ShowSaveMenu(string arg, object[] args)
        {
            Game.ShowSaveMenu();
            return null;
        }

        public static object Echo(string arg, object[] args)
        {
            return arg;
        }

        public static object CallNative(string arg, object[] args)
        {
            // Arg is the native hash
            Hash nativeHash;
            if (!Enum.TryParse<Hash>(arg, out nativeHash)) return null;

            // Verify return type
            if (args.Length < 1 || !(args[0] is string)) return null;
            string retType = (string)args[0];

            // Build arguments
            List<InputArgument> nativeArgs = new List<InputArgument>();
            for (int i = 1; i < args.Length; i++)
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
            if (retType == "void")
            {
                Function.Call(nativeHash, nativeArgs.ToArray());
                return null;
            }
            else if (retType == "bool") // Boolean
            {
                return Function.Call<bool>(nativeHash, nativeArgs.ToArray());
            }
            else if (retType == "float") // Single
            {
                return Function.Call<float>(nativeHash, nativeArgs.ToArray());
            }
            else if (retType == "double") // Double
            {
                return Function.Call<double>(nativeHash, nativeArgs.ToArray());
            }
            else if (retType == "Int64") // Int64
            {
                return Function.Call<Int64>(nativeHash, nativeArgs.ToArray());
            }
            else if (retType == "string") // String
            {
                return Function.Call<string>(nativeHash, nativeArgs.ToArray());
            }
            else // Int32
            {
                // because int is mostly used as handles, this is the default type
                return Function.Call<int>(nativeHash, nativeArgs.ToArray());
            }
        }

        public static object InvokeExtension(string arg, object[] args)
        {
            return ExtensionManager.Instance.CallExtension(arg, args);
        }
    }
}
