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
    public struct GameData
    {
        // Game and world
        public TimeSpan GameTime { get; set; }
        public float FPS { get; set; }
        public string RadioStation { get; set; }
        public string Weather { get; set; }

        // Player
        public int PlayerHandle { get; set; }
        public int WantedLevel { get; set; }
        public int PlayerHealth { get; set; }
        public int PlayerArmor { get; set; }
        public int PlayerMoney { get; set; }
        public Vector3 PlayerPos { get; set; }
        public float PlayerHeading { get; set; }
        public string PlayerName { get; set; }
        public string ZoneName { get; set; }
        public string StreetName { get; set; }

        // Weapons
        public string WeaponName { get; set; }
        public int WeaponAmmo { get; set; }
        public int WeaponAmmoInClip { get; set; }
        public int WeaponMaxInClip { get; set; }

        // Vechicle
        public int VehicleHandle { get; set; }
        public string VehicleName { get; set; }
        public float VehicleSpeed { get; set; }
        public float VehicleRPM { get; set; }
        public string VehicleLicense { get; set; }
        public string VehicleType { get; set; }
        public float VehicleEngineHealth { get; set; }
        public float VehiclePetrolHealth { get; set; }

        public static GameData GetData()
        {
            GameData dat = new GameData();

            // Game and world
            dat.GameTime = World.CurrentDayTime;
            dat.FPS = Game.FPS;
            dat.RadioStation = WorldHelper.GetRadioFriendlyName(Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME));
            dat.Weather = World.Weather.ToString();

            // Player
            dat.PlayerHandle = Game.Player.Character.Handle;
            dat.WantedLevel = Game.Player.WantedLevel;
            dat.PlayerHealth = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Game.Player.Character.Handle) - 100;
            dat.PlayerArmor = Game.Player.Character.Armor;
            dat.PlayerMoney = Game.Player.Money;
            dat.PlayerPos = Game.Player.Character.Position;
            dat.PlayerHeading = Game.Player.Character.Heading;
            dat.PlayerName = ((PedHash)Game.Player.Character.Model.Hash).ToString();
            dat.ZoneName = World.GetZoneName(Game.Player.Character.Position);
            dat.StreetName = World.GetStreetName(Game.Player.Character.Position);

            // Weapons
            dat.WeaponName = Enum.GetName(typeof(WeaponHash), Game.Player.Character.Weapons.Current.Hash);
            dat.WeaponAmmo = Game.Player.Character.Weapons.Current.Ammo;
            dat.WeaponAmmoInClip = Game.Player.Character.Weapons.Current.AmmoInClip;
            dat.WeaponMaxInClip = Game.Player.Character.Weapons.Current.MaxAmmoInClip;

            // Vehicle
            if (Game.Player.Character.IsInVehicle())
            {
                var veh = Game.Player.Character.CurrentVehicle;
                dat.VehicleHandle = veh.Handle;
                dat.VehicleName = veh.FriendlyName;
                dat.VehicleSpeed = veh.Speed;
                dat.VehicleRPM = veh.CurrentRPM;
                dat.VehicleLicense = veh.NumberPlate;
                dat.VehicleType = veh.ClassType.ToString();
                dat.VehicleEngineHealth = veh.EngineHealth;
                dat.VehiclePetrolHealth = veh.PetrolTankHealth;
            }
            
            
            return dat;
        }
    }
}
