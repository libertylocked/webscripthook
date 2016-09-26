# WebScriptHook example extensions
This project includes some example extensions for WebScriptHook

#### ClockPlugin
- A tickable extension that is ticked every frame by WebScriptHook
- Turns on clock display on screen if ["on"] is received
- Turns off when ["off"] is received

#### DrivingPlugin
- A super advanced demo
- This plugin allows the user to drive with their phone using gyroscope and buttons
- See [/apps/driving/index.html](/webscripthook-server/apps/driving/index.html) for the front-end of this extension

#### ExceptionPlugin
- Throws an exception when ["throw"] is received
- Used to test WebScriptHook exception handling
- WebScriptHook should drop the extension when exception is thrown

#### RelayPlugin
- Relays messages to subtitle plugin and returns whatever subtitle plugin returns
- Used to test extension-to-extension calling

#### SubtitlePlugin
- A stateless extension that displays subtitles by concatenating all the input arguments
- Returns the shown subtitle text
