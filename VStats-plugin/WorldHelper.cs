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
        public static string GetRadioFriendlyName(string displayName)
        {
            switch (displayName)
            {
                case "RADIO_01_CLASS_ROCK":
                    return "Los Santos Rock Radio";
                case "RADIO_02_POP":
                    return "Non-Stop-Pop FM";
                case "RADIO_03_HIPHOP_NEW":
                    return "Radio Los Santos";
                case "RADIO_04_PUNK":
                    return "Channel X";
                case "RADIO_05_TALK_01":
                    return "West Coast Talk Radio";
                case "RADIO_06_COUNTRY":
                    return "Rebel Radio";
                case "RADIO_07_DANCE_01":
                    return "Soulwax FM";
                case "RADIO_08_MEXICAN":
                    return "East Los FM";
                case "RADIO_09_HIPHOP_OLD":
                    return "West Coast Classics";
                case "RADIO_11_TALK_02":
                    return "Blaine County Radio";
                case "RADIO_12_REGGAE":
                    return "Blue Ark";
                case "RADIO_13_JAZZ":
                    return "Worldwide FM";
                case "RADIO_14_DANCE_02":
                    return "FlyLo FM";
                case "RADIO_15_MOTOWN":
                    return "The Lowdown 91.1";
                case "RADIO_16_SILVERLAKE":
                    return "Radio Mirror Park";
                case "RADIO_17_FUNK":
                    return "Space 103.2";
                case "RADIO_18_90S_ROCK":
                    return "Vinewood Boulevard Radio";
                case "RADIO_19_USER":
                    return "Self Radio";
                case "RADIO_20_THELAB":
                    return "The Lab";
                default:
                    return "Radio Off";
            }
        }
    }
}
