using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScriptHook.Extensions;
using GTA;
using GTA.Native;

namespace ExtensionExamples
{
    /// <summary>
    /// A super advanced demo.
    /// This plugin allows the user to drive with gyroscope!
    /// </summary>
    class DrivingPlugin : Extension, ITickable
    {
        bool enable = false;
        float steerBias = 0;
        bool gas, brake;

        public DrivingPlugin() { }

        public void Tick()
        {
            // Only steer when player is in vehicle
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            if (enable && veh != null)
            {
                // Call VEHICLE::SET_VEHICLE_STEER_BIAS to steer the vehicle (-1 full right, 1 full left)
                Function.Call(Hash.SET_VEHICLE_STEER_BIAS, veh, steerBias);

                // Apply acceleration

            }
            gas = false;
            brake = false;
        }

        public override object HandleCall(object[] args)
        {
            // Must have 1 argument and it must be a string
            if (args.Length > 0 && args[0] as string != null)
            {
                string arg = (string)args[0];
                if (arg == "on")
                {
                    enable = true;
                    UI.Notify("Mobile driving is on");
                    return "Extension enabled";
                }
                else if (arg == "off")
                {
                    enable = false;
                    UI.Notify("Mobile driving is off");
                    return "Extension disabled";
                }
                else if (arg == "gas")
                {
                    gas = true;
                }
                else if (arg == "brake")
                {
                    brake = true;
                }
                else
                {
                    // Set bias
                    if (float.TryParse(arg, out steerBias))
                    {
                        return null; // return null
                    }
                    else
                    {
                        return "Invalid bias input!";
                    }
                }
            }

            // Always return null
            return "Invalid arguments!";
        }
    }
}
