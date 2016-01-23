# VStats plugin + server
Created by libertylocked

Real-time GTAV in-game data for your second screen experience. Includes server and plugin.
Inspired by [ETS2 Telemetry Server](https://github.com/Funbit/ets2-telemetry-server)!

# Installation
#### Plugin
- You need [Script Hook V .NET](https://github.com/crosire/scripthookvdotnet/tree/master) and all its required runtimes
- Extract the plugin files to "GTAV/scripts"
- Run the game

#### Server
- You can really just extract the server files anywhere
- Run "VStats-server-go.exe"
- You need to allow the program to communicate through firewall
- The default port is **25555**. Can be changed in config (see **Other notes**).
- Now open your browser and go to **http://localhost:25555** You should see the home page.
- To access the page from devices on your LAN, replace "localhost" above with the address of your PC. Google ["How to find my LAN IP"](https://www.google.com/search?q=How%20to%20find%20my%20LAN%20IP) if you don't know how to do that.

# Custom skins
#### Apply a skin
- Drop the folder of your custom skins under "/skins" folder
- If done correctly, the index page of the skin should be at **/skins/\<skin name\>/index.html**
- Refresh 

#### Make your own skin
- You need to make some AJAX calls to...
- Get game data from "/pull"
- Get last updated time from "/getTime"
- You can also get a dummy data from "/dummy" for testing purposes
- You can send certain inputs to the game. See [the wiki page](https://github.com/LibertyLocked/VStats/wiki) for details

# Other notes
- Whether you start the game first or the server first does not matter
- If you want to change port, make sure you modify "VStats-plugin.ini" for **both the plugin and the server**
- If things broke, check "GTAV/ScriptHookVDotNet-2016-01-19.log" (replace the date with your current date) for errors

# Screenshots
<img src="/Images/example0.png" width="360">
<img src="/Images/example1.png" width="360">
<img src="/Images/example2.png" width="360">
<img src="/Images/example3.png" width="360">
<img src="/Images/example4.png" width="360">

# Video
[![Video1](http://img.youtube.com/vi/-LCbX3b3LsU/0.jpg)](http://www.youtube.com/watch?v=-LCbX3b3LsU)
> Have fun making your own skins! <img src="https://github.com/favicon.ico" width="32"> Pull requests are welcome
