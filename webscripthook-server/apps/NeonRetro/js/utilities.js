var DATA = null;
var SPEED_MODE = 0;
var CLOCK_RATE = 100;
var COLOR_ON = "#00c6ff";
var COLOR_OFF = "#202020";
var PANE = "dashboard";
var WEATHER_CACHE = [
    "ExtraSunny", "Clear","Clouds","Smog","Foggy","Overcast","Raining",
    "ThunderStorm","Clearing","Neutral","Snowing","Blizzard","Snowlight",
    "Christmas"
];
var RADIO_CACHE = {
    "Los Santos Rock Radio": [0, "Los Santos Rock Radio"],
    "Non-Stop-Pop FM": [1, "Non-Stop-Pop FM"],
    "Radio Los Santos": [2, "Radio Los Santos"],
    "Channel X": [3, "Channel X"],
    "West Coast Talk Radio": [4, "WCTR"],
    "Rebel Radio": [5, "Rebel Radio"],
    "Soulwax FM": [6, "Soulwax FM"],
    "East Los FM": [7, "East Los FM"],
    "West Coast Classics": [8, "West Coast Classics"],
    "Blue Ark": [9, "Blue Ark"],
    "Worldwide FM": [10, "WorldWide FM"],
    "FlyLo FM": [11, "FlyLo FM"],
    "The Lowdown 91.1": [12, "The Lowdown 91.1"],
    "The Lab": [13, "The Lab"],
    "Radio Mirror Park": [14, "Radio Mirror Park"],
    "Space 103.2": [15, "Space 103.2"],
    "Vinewood Boulevard Radio": [16, "Vinewood Boulevard Radio"],
    "Self Radio": [17, "Self Radio"],
    "Blaine County Radio" : [18, "Blaine County Radio"],
    "Radio Off": [19, "Radio Off"],
};
var WEAPON_CACHE = {
    "Knife": [0, "Knife", "melee"],
    "Nightstick": [1, "Nightstick", "melee"],
    "Hammer": [2, "Hammer", "melee"],
    "Bat": [3, "Bat", "melee"],
    "GolfClub": [4, "Golf Club", "melee"],
    "Crowbar": [5, "Crowbar", "melee"],
    "Bottle": [6, "Bottle", "melee"],
    "SwitchBlade": [7, "Switch Blade", "melee"],
    "Pistol": [8, "Pistol", "ranged"],
    "CombatPistol": [9, "Combat Pistol", "ranged"],
    "APPistol": [10, "AP Pistol", "ranged"],
    "Pistol150": [11, "Pistol 150", "ranged"],
    "FlareGun": [12, "Flare Gun", "ranged"],
    "MarksmanPistol": [13, "Marksman Pistol", "ranged"],
    "Revolver": [14, "Revolver", "ranged"],
    "MicroSMG": [15, "Micro SMG", "ranged"],
    "SMG": [16, "SMG", "ranged"],
    "AssaultSMG": [17, "Assault SMG", "ranged"],
    "CombatPDW": [18, "Combat PDW", "ranged"],
    "AssaultRifle": [19, "Assault Rifle", "ranged"],
    "CarbineRifle": [20, "Carbine Rifle", "ranged"],
    "AdvancedRifle": [21, "Advanced Rifle", "ranged"],
    "MG": [22, "MG", "ranged"],
    "CombatMG": [23, "Combat MG", "ranged"],
    "PumpShotgun": [24, "Pump Shotgun", "ranged"],
    "SawnOffShotgun": [25, "Sawn Off Shotgun", "ranged"],
    "AssaultShotgun": [26, "Assault Shotgun", "ranged"],
    "BullpupShotgun": [27, "Bullpup Shotgun", "ranged"],
    "StunGun": [28, "Stun Gun", "ranged"],
    "SniperRifle": [29, "Sniper Rifle", "ranged"],
    "HeavySniper": [30, "Heavy Sniper", "ranged"],


    "Unarmed": [-1, "Unarmed", "melee"]
}

var STATION_COUNT = 20;

/*============================================================================*/

$(document).ready(function() {
    console.log("Document Ready...");
    convertToKPH();
    poll();
});

function poll() {
    console.log("Polling started...")
    var prevStation;
    setInterval(function() {
        $.ajax({
            url: "/pull",
            dataType: "json",
            success: function(response) {
                //if(response != null && response != "NO_DATA") {
                DATA = response;
                // } else {
                //     $.ajax({
                //         url: "/dummy",
                //         dataType: "json",
                //         success: function(response) {
                //             prevStation = DATA["RadioStation"];
                //             DATA = response;
                //             $("#page-header").html("NO GTA V CLIENT FOUND<br />USING DUMMY SIMULATION<br /><hr />");
                //             DATA["GameTime"] = null;
                //             DATA["RadioStation"] = prevStation;
                //             DATA["VehicleName"] = null;
                //         },
                //         error: function (xhr, ajaxOptions, thrownError) {
                //             $("#page-header").html("NO SERVER RESPONSE<br />USING DUMMY SIMULATION<br /><hr />");
                //         }
                //     });
                // }
            }
        });
        if (DATA == null || DATA == "NO_DATA") {
            // DATA = {"GameTime":"12:55:18","FPS":65.6418839,"RadioStation":"RadioLosSantos","Weather":"Clear","WantedLevel":0,"PlayerHealth":100,"PlayerArmor":0,"PlayerMoney":500328,"PlayerPos":{"X":-37.74217,"Y":-1460.89392,"Z":31.0653267},"PlayerName":"Franklin","ZoneName":"Strawberry","StreetName":"Forum Dr","WeaponName":"Unarmed","WeaponAmmo":1,"WeaponAmmoInClip":1,"WeaponMaxInClip":1,"VehicleName":"Buffalo S","VehicleSpeed":0.00330539187,"VehicleRPM":0.2,"VehicleLicense":" FC1988 ","VehicleType":"Sports","VehicleEngineHealth":1000.0,"VehiclePetrolHealth":1000.0}
            // DATA["VehicleName"] = null;
            DATA = {"GameTime":"15:27:23","FPS":52.83915,"RadioStation":"NonStopPopFM","Weather":"Smog","WantedLevel":0,"PlayerHealth":75,"PlayerArmor":0,"PlayerMoney":500328,"PlayerPos":{"X":1000.90381,"Y":-1767.83521,"Z":31.85},"PlayerName":"Franklin","ZoneName":"San Andreas","StreetName":"Innocence Blvd","WeaponName":"Unarmed","WeaponAmmo":1,"WeaponAmmoInClip":1,"WeaponMaxInClip":1,"VehicleName":"Buffalo S","VehicleSpeed":10.4436655,"VehicleRPM":0.425404578,"VehicleLicense":" FC1988 ","VehicleType":"Sports","VehicleEngineHealth":970.103333,"VehiclePetrolHealth":982.828552}
        }
        setClockWidget();
        if (PANE == "dashboard") {
            initDashboardWidgets();
        } else if (PANE == "player") {
            initPlayerWidgets();
        } else if (PANE == "vehicle") {
            initVehicleWidgets();
        }
    }, CLOCK_RATE);
}

function getRadioStationIndex(delta) {
    var currIndex = -1;
    var newIndex;
    var currStation = getRadioStation();
    $.each(RADIO_CACHE, function(key, value) {
        if (key == currStation) {
            currIndex = value[0];
        }
    });
    // Assume valid station, since only valid stations get set.
    if (delta == null || delta == 0) {
        return currIndex;
    } else {
        newIndex = currIndex + delta;

        if(newIndex >= STATION_COUNT) {
            newIndex = newIndex - STATION_COUNT;
        } else if(newIndex < 0) {
            newIndex = STATION_COUNT - newIndex - 2;
        }
        return newIndex;


    }
}

function initDashboardWidgets() {
    setActivePane("dashboard");
    setRadioWidget();
    setClockWidget();
    setSpedometerWidget();
    setWeatherWidget();
    setLocationWidget();
    if(getVehicleName() != null) {
        $(".widget-radio").show();
        $(".widget-spedometer").show();
        $(".widget-weather").show();
        $("#tab-vehicle").prop("disabled",false);
        setBackgroundColor("#tab-vehicle", COLOR_OFF);
    } else {
        $(".widget-radio").hide();
        $(".widget-spedometer").hide();
        $(".widget-weather").hide();
        $("#tab-vehicle").prop("disabled",true);
        setBackgroundColor("#tab-vehicle", "#bbbbbb");
    }

}

function initPlayerWidgets() {
    setActivePane("player");
    setPlayerStatsWidget();
    setPlayerWeaponWidget();
}

function setPlayerWeaponWidget() {
    setWeaponName();
    setWeaponAmmo();
}

function setWeaponName() {
    var weapKey = getWeaponName();
    // var weapName;
    // $.each(WEAPON_CACHE, function(key, value) {
    //     if (key == weapKey) {
    //         weapName = value[1];
    //     }
    // });
    // $(".weapon-name").html(weapName);
    $(".weapon-name").html(weapKey);
}

function setWeaponAmmo() {
    $(".weapon-ammo").html(getWeaponAmmo() + " / " + getWeaponMaxInClip());
}

function setPlayerHealth() {
    $(".player-health").html("&#10084; " + getPlayerHealth());
}

function setPlayerArmor() {
    $(".player-armor").html("&#9820; " + getPlayerArmor());
}

function initVehicleWidgets() {
    setActivePane("vehicle");
    setSpedometerWidget();
    setRadioWidget();
    setLocationWidget();
    setVehicleWidgets();
}

function setVehicleWidgets() {
    setVehicleName();
    setVehicleType();
    setVehiclePlate();
    setVehicleEngineHealth();
    setVehiclePetrolHealth();
}

function setVehicleName() {
    $(".vehicle-name").html(getVehicleName());
}

function setVehicleType() {
    $(".vehicle-type").html(getVehicleType());
}

function setVehiclePlate() {
    $(".vehicle-plate").html(getVehiclePlate());
}

function setVehicleEngineHealth() {
    $(".vehicle-health").html("Engine: " + (getVehicleEngineHealth() / 10).toFixed(0) + "%");
}

function setVehiclePetrolHealth() {
    $(".vehicle-petrol").html("Petrol: " + (getVehiclePetrolHealth() / 10).toFixed(0) + "%");
}

function setActivePane(pane) {
    if (pane == "dashboard") {
        PANE = "dashboard";
        $("#pane-dashboard").show();
        $("#pane-player").hide();
        $("#pane-vehicle").hide();
        setBorderColor("#tab-dashboard", COLOR_ON);
        setBorderColor("#tab-player", COLOR_OFF);
        setBorderColor("#tab-vehicle", COLOR_OFF);
    } else if (pane == "vehicle") {
        PANE = "vehicle";
        $("#pane-vehicle").show();
        $("#pane-dashboard").hide();
        $("#pane-player").hide();
        setBorderColor("#tab-dashboard", COLOR_OFF);
        setBorderColor("#tab-player", COLOR_OFF);
        setBorderColor("#tab-vehicle", COLOR_ON);
    } else if (pane == "player") {
        PANE = "player";
        $("#pane-player").show();
        $("#pane-dashboard").hide();
        $("#pane-vehicle").hide();
        setBorderColor("#tab-dashboard", COLOR_OFF);
        setBorderColor("#tab-player", COLOR_ON);
        setBorderColor("#tab-vehicle", COLOR_OFF);
    } else {

    }
}

function setRadioStation(delta) {
    $.post("/input", JSON.stringify({"Cmd": "radio", "Arg": toString(delta)}));
}

function setData(response) {
    DATA = JSON.parse(response);
}

function getPlayerMoney() {
    return DATA["PlayerMoney"];
}

function getWantedLevel() {
    return DATA["WantedLevel"];
}

function getPlayerName() {
    return DATA["PlayerName"];
}

function getPlayerPos() {
    return DATA["PlayerPos"];
}

function getWeaponName() {
    return DATA["WeaponName"];
}

function getWeaponAmmo() {
    return DATA["WeaponAmmo"];
}

function getWeaponAmmoInClip() {
    return DATA["WeaponName"];
}

function getWeaponMaxInClip() {
    return DATA["WeaponMaxInClip"];
}

function getVehicleSpeed() {
    return DATA["VehicleSpeed"];
}

function getVehicleName() {
    return DATA["VehicleName"];
}

function getVehicleType() {
    return DATA["VehicleType"];
}

function getVehiclePlate() {
    return DATA["VehicleLicense"];
}

function getVehicleRPM() {
    return DATA["VehicleRPM"];
}

function getVehicleEngineHealth() {
    return DATA["VehicleEngineHealth"];
}

function getVehiclePetrolHealth() {
    return DATA["VehiclePetrolHealth"];
}

function getRadioStation() {
    return DATA["RadioStation"];
}

function getPlayerHealth() {
    return DATA["PlayerHealth"];
}

function getPlayerArmor() {
    return DATA["PlayerArmor"];
}

function getWeather() {
    return DATA["Weather"];
}

function getGameTime() {
    if (DATA["GameTime"] == null) {
        return(new Date().toTimeString().split(' ')[0]);
    }
    return DATA["GameTime"];
}

function getZone() {
    return DATA["ZoneName"];
}

function getStreet() {
    return DATA["StreetName"];
}

function setRadioWidget() {
    var stationKey = getRadioStation();
    var stationValue = RADIO_CACHE[stationKey];
    var station;

    // Would normally just use image like in-game

    // Following code for demonstrative purposes
    $.each(RADIO_CACHE, function(key, value) {
        if (key == stationKey) {
            station = value[1];
        }
    });
    $(".station").html("<br />" + station + "<br /><br />");
}

function setClockWidget() {
    var timeString;
    timeString = getGameTime();
    var timeArray = [];
    timeArray = timeString.split(":");
    var minutes;
    minutes = timeArray[1];
    var hours;
    hours = timeArray[0];
    var seconds;
    seconds = timeArray[2];

    $("#secs").html(seconds);
    $("#mins").html(minutes);
    $("#hours").html(hours);
}

function setSpedometerWidget() {
    var v_original = 0;
    var v_converted = 0;
    var mode;
    v_original = getVehicleSpeed();
    if(SPEED_MODE == 0) {
        mode = "KPH";
        v_converted = v_original * 3.6;
    } else {
        mode = "MPH";
        v_converted = v_original * 3.6 / 1.609344;
    }
    $(".velocity").html(v_converted.toFixed(3));
}

function setPlayerStatsWidget() {
    setPlayerName();
    setPlayerMoney();
    setWantedLevel();
    setPlayerHealth();
    setPlayerArmor();
}

function setPlayerName() {
    $(".player-name").html("NAME: " + getPlayerName());
}

function setPlayerMoney() {
    $(".player-money").html("CASH: $" + getPlayerMoney());
}

function setWantedLevel() {
    $(".wanted-level").html("WNTD: " + buildWantedLevel());
}

function buildWantedLevel() {
    var level = getWantedLevel();
    switch(level) {
        case 0: return "&#9734;&#9734;&#9734;&#9734;&#9734;";
        case 1: return "&#9733;&#9734;&#9734;&#9734;&#9734;";
        case 2: return "&#9733;&#9733;&#9734;&#9734;&#9734;";
        case 3: return "&#9733;&#9733;&#9733;&#9734;&#9734;";
        case 4: return "&#9733;&#9733;&#9733;&#9733;&#9734;";
        case 5: return "&#9733;&#9733;&#9733;&#9733;&#9733;";
    }
}

function setLocationWidget() {
    $(".widget-location").text(getZone() + " / " + getStreet());
}

function setWeatherWidget() {
    var currWeather = getWeather();
    var index = WEATHER_CACHE.indexOf(currWeather);
    if (index > -1) {
        $(".widget-weather").html("<br /><br /><br />" + currWeather);
    } else {

    }
}

function convertToKPH() {
    SPEED_MODE = 0;
    setBorderColor(".kph", COLOR_ON);
    setBorderColor(".mph", COLOR_OFF);
}

function convertToMPH() {
    SPEED_MODE = 1;
    setBorderColor(".mph", COLOR_ON);
    setBorderColor(".kph", COLOR_OFF);
}
function setBorderColor(elem, color) {
    // var property=document.getElementById(elem);
    // property.style.borderColor = color;
    $(elem).css("border-color", color);
}

function setBackgroundColor(elem, color) {
    // var property=document.getElementById(elem);
    // property.style.backgroundColor = color;
    $(elem).css("background-color", color);
}
