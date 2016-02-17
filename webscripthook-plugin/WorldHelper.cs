using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace WebScriptHook
{
    class WorldHelper
    {
        static Dictionary<string, string> radioDict = new Dictionary<string, string>
        {
            {"RADIO_01_CLASS_ROCK", "Los Santos Rock Radio"},
            {"RADIO_02_POP", "Non-Stop-Pop FM"},
            {"RADIO_03_HIPHOP_NEW", "Radio Los Santos"},
            {"RADIO_04_PUNK", "Channel X"},
            {"RADIO_05_TALK_01", "West Coast Talk Radio"},
            {"RADIO_06_COUNTRY", "Rebel Radio"},
            {"RADIO_07_DANCE_01", "Soulwax FM"},
            {"RADIO_08_MEXICAN", "East Los FM"},
            {"RADIO_09_HIPHOP_OLD", "West Coast Classics"},
            {"RADIO_11_TALK_02", "Blaine County Radio"},
            {"RADIO_12_REGGAE", "Blue Ark"},
            {"RADIO_13_JAZZ", "Worldwide FM"},
            {"RADIO_14_DANCE_02", "FlyLo FM"},
            {"RADIO_15_MOTOWN", "The Lowdown 91.1"},
            {"RADIO_16_SILVERLAKE", "Radio Mirror Park"},
            {"RADIO_17_FUNK", "Space 103.2"},
            {"RADIO_18_90S_ROCK", "Vinewood Boulevard Radio"},
            {"RADIO_19_USER", "Self Radio"},
            {"RADIO_20_THELAB", "The Lab"},
            {"RADIO_OFF", "Radio Off"},
            {"", "Radio Off"},
        };

        public static string GetRadioFriendlyName(string displayName)
        {
            string friendlyName;
            if (radioDict.TryGetValue(displayName, out friendlyName)) return friendlyName;
            else return "";
        }
    }
}
