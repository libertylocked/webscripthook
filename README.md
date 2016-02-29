# WebScriptHook plugin + server
[![Build status](https://ci.appveyor.com/api/projects/status/u0q7xo205ls01qmq?svg=true)](https://ci.appveyor.com/project/LibertyLocked/webscripthook)

Created by libertylocked

Providing client-side scripts with a REST API for GTAV. Includes server and plugin.
Inspired by [ETS2 Telemetry Server](https://github.com/Funbit/ets2-telemetry-server)

# Installation
#### Requirements
- [Script Hook V](http://www.dev-c.com/gtav/scripthookv)
- [Script Hook V .NET](https://github.com/crosire/scripthookvdotnet)
- [.NET Framework 4.5.2](https://www.microsoft.com/download/details.aspx?id=42642)
- [Visual C++ Redistributable for Visual Studio 2013 x64](https://www.microsoft.com/download/details.aspx?id=40784)

#### WebScriptHook
- Extract "WebScriptHook" folder to "GTAV/scripts"
 - The configuration file should be at GTAV/scripts/WebScriptHook/WebScriptHook.ini
 - The DLLs should be at GTAV/scripts/WebScriptHook/*.dll
 - The server exe should be at GTAV/scripts/WebScriptHook/webscripthook-server.exe
- Run the game

# WebScriptHook apps
#### Running a web app
- Start the game
- Run "webscripthook-server.exe"
 - You need to allow the program to communicate through firewall
 - The default port is **25555**. Can be changed in config (restart game and the server to apply)
- Open your browser and go to **http://localhost:25555** You should see the home page
- To access the page from devices on your LAN, replace "localhost" above with the address of your PC. Google ["How to find my LAN IP"](https://www.google.com/search?q=How%20to%20find%20my%20LAN%20IP) if you don't know how to do that

#### Installing a web app
- Drop the folder of a WebScriptHook app under "/apps" folder
 - If done correctly, the index page of the app should be at **/apps/\<app name\>/index.html**
- Refresh the webpage

#### WebScriptHook REST API
WebScriptHook provides a REST API that allows you to pull real-time data from the game, as well as calling native functions remotely!
- Pulling some real-time data in JSON format
```
GET /pull
```
- Calling a native function
```
POST /input
{
	"Cmd": "native",
	"Arg": "GET_ENTITY_HEALTH", <- Name or hash of the native function
	"Args": ["int", 2]  <- Return type (a string), native function arguments (an int/float/string/bool)
}
```
- See [the wiki page](https://github.com/LibertyLocked/webscripthook/wiki) for details

# WebScriptHook extensions
#### Writing an extension
- You can write your own extensions to handle inputs sent through the web
- Subclass WebScriptHook.Extensions.Extension and override HandleCalled method
- Your extension can also send stuff back to the caller
- You extension instance is persistent, which means it can have state!
- See [the wiki page](https://github.com/LibertyLocked/webscripthook/wiki) for details

#### Calling an extension
- POST to /input with cmd "extension"
- Arg needs to be "filename-without-dll.namespace.classname". e.g. extension-examples.ExampleExtensions.MyExtension
- Args will be used to call the extension

# Other notes
- Whether you start the game first or the server first does not matter
- If things broke, check "GTAV/ScriptHookVDotNet-2016-01-19.log" (replace the date with your current date) for errors

# Screenshots
<img src="/Images/example0.png" width="360">
<img src="/Images/example1.png" width="360">
<img src="/Images/example2.png" width="360">
<img src="/Images/example3.png" width="360">
<img src="/Images/example4.png" width="360">

# Video
[![Video1](http://img.youtube.com/vi/-LCbX3b3LsU/0.jpg)](http://www.youtube.com/watch?v=-LCbX3b3LsU)
> Have fun making your own apps! <img src="https://github.com/favicon.ico" width="32"> Pull requests are welcome
