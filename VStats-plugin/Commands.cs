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

        public static void RadioTo(string arg)
        {
            RadioStation station;
            if (Enum.TryParse<RadioStation>(arg, out station))
            {
                Game.RadioStation = station;
            }
        }

        public static void FixPlayerVehicle(string arg)
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.Repair();
            }
        }

        public static void SetWantedLevel(string arg)
        {
            int stars;
            if (int.TryParse(arg, out stars) && stars >= 0 && stars <= 5)
            {
                Game.Player.WantedLevel = stars;
            }
        }

        public static void SetPlayerHealth(string arg)
        {
            int hp;
            if (int.TryParse(arg, out hp) && hp >= -1 && hp <= Game.Player.Character.MaxHealth)
            {
                Game.Player.Character.Health = hp;
            }
        }

        public static void SetPlayerArmor(string arg)
        {
            int ar;
            if (int.TryParse(arg, out ar) && ar >= 0 && ar <= 100)
            {
                Game.Player.Character.Armor = ar;
            }
        }

        public static void SetBlackout(string arg)
        {
            bool on;
            if (bool.TryParse(arg, out on))
            {
                World.SetBlackout(on);
            }
        }

        public static void ChangeTime(string arg)
        {
            TimeSpan ts;
            if (TimeSpan.TryParse(arg, out ts))
            {
                World.CurrentDayTime = ts;
            }
        }

        public static void ChangeWeather(string arg)
        {
            Weather weather;
            if (Enum.TryParse<Weather>(arg, out weather))
            {
                World.Weather = weather;
            }
        }

        public static void MaxAmmo(string arg)
        {
            Weapon currWep = Game.Player.Character.Weapons.Current;
            currWep.Ammo = currWep.MaxAmmo;
        }

        public static void SpawnVehicle(string arg)
        {
            VehicleHash vehHash;
            if (Enum.TryParse<VehicleHash>(arg, out vehHash))
            {
                World.CreateVehicle(new Model(vehHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
            }
        }

        public static void SpawnPed(string arg)
        {
            PedHash pedHash;
            if (Enum.TryParse<PedHash>(arg, out pedHash))
            {
                World.CreatePed(new Model(pedHash), Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
            }
        }

        public static void GiveWeapon(string arg)
        {
            WeaponHash wepHash;
            if (Enum.TryParse<WeaponHash>(arg, out wepHash))
            {
                Game.Player.Character.Weapons.Give(wepHash, 9999, true, true);
            }
        }

        public static void ShowSaveMenu(string arg)
        {
            Game.ShowSaveMenu();
        }
    }
}
