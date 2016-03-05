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
        // Time to hold gas/brake once input is received.
        // We do this because there's always going to be some lag/delay and 
        // inputs won't be continuous between every frame
        const float HOLD_TIME = 0.1f;

        bool enable = false;
        float steerBias = 0; // -1 for full left, 1 for full right
        float brakeTime = 0, accelerateTime = 0;

        public DrivingPlugin() { }

        public void Tick()
        {
            // Only steer when player is in vehicle
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            if (enable && veh != null)
            {
                // Steer by simulating left/right controls
                Function.Call(Hash._0xE8A25867FBA3B05E, 2, (int)Control.VehicleMoveLeftRight, steerBias);

                // Apply acceleration by simulating input
                if (accelerateTime > 0)
                {
                    Function.Call(Hash._0xE8A25867FBA3B05E, 2, (int)Control.VehicleAccelerate, 1f); // 71 is INPUT_VEH_ACCELERATE
                }
                if (brakeTime > 0)
                {
                    Function.Call(Hash._0xE8A25867FBA3B05E, 2, (int)Control.VehicleBrake, 1f); // 72 is INPUT_VEH_BRAKE
                }
            }
            // Decrement hold times
            if (accelerateTime > 0)
            {
                accelerateTime -= Game.LastFrameTime;
            }
            if (brakeTime > 0)
            {
                brakeTime -= Game.LastFrameTime;
            }
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
                }
                else if (arg == "off")
                {
                    enable = false;
                    UI.Notify("Mobile driving is off");
                }
                else if (arg == "gas")
                {
                    StartAccelerating();
                }
                else if (arg == "brake")
                {
                    StartBraking();
                }
                else
                {
                    // Set bias
                    if (float.TryParse(arg, out steerBias)) { }
                }
            }

            // Always return null
            return null;
        }

        void StartBraking()
        {
            brakeTime = HOLD_TIME;
        }

        void StartAccelerating()
        {
            accelerateTime = HOLD_TIME;
        }
    }
}
