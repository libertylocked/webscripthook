function radioPrev() {
  sendToGame("Radio", "0");
}
function radioNext() {
  sendToGame("Radio", "1");
}
function radioMirrorPark() {
  sendToGame("RadioTo", "RadioMirrorPark");
}
function radioSelf() {
  sendToGame("RadioTo", "SelfRadio");
}
function radioLab() {
  sendToGame("RadioTo", "TheLab");
}
function radioRock() {
  sendToGame("RadioTo", "LosSantosRockRadio");
}
function fixCar() {
  sendToGame("Repair", "");
}
function fiveStars() {
  sendToGame("WantedLevel", "5");
}
function zeroStars() {
  sendToGame("WantedLevel", "0");
}
function fullHealth() {
  sendToGame("Health", "100");
}
function suicide() {
  sendToGame("Health", "-1");
}
function fullArmor() {
  sendToGame("Armor", "100");
}
function maxAmmo() {
  sendToGame("MaxAmmo", "");
}
function blackoutOn() {
  sendToGame("Blackout", "true");
}
function blackoutOff() {
  sendToGame("Blackout", "false");
}
function setMorning() {
  sendToGame("Time", "08:00:00");
}
function setNoon() {
  sendToGame("Time", "12:00:00");
}
function setNight() {
  sendToGame("Time", "20:00:00");
}
function setSunny() {
  sendToGame("Weather", "ExtraSunny");
}
function setRain() {
  sendToGame("Weather", "Raining");
}
function setThunderStorm() {
  sendToGame("Weather", "ThunderStorm");
}
function setBlizzard() {
  sendToGame("Weather", "Blizzard");
}
function spawnT20() {
  sendToGame("SpawnVehicle", "T20");
}
function spawnBuzzard() {
  sendToGame("SpawnVehicle", "Buzzard");
}

function sendToGame(cmd, arg) {
  $.ajax({
    url: '/input',
    type: 'post',
    dataType: 'json',
    success: null,
    data: JSON.stringify({ "Cmd": cmd, "Arg" : arg }),
  });
}
