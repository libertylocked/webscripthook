using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GTA;
using Newtonsoft.Json;
using WebScriptHook.Extensions;
using WebScriptHook.Serialization;
using WebSocketSharp;

namespace WebScriptHook
{
    class MainScript : Script
    {
        private string url;

        static AutoResetEvent networkWaitHandle = new AutoResetEvent(false);
        WebSocket ws;

        GameData cacheData;
        ConcurrentQueue<WebInput> inputQueue;
        ConcurrentQueue<KeyValuePair<string, object>> retQueue;

        JsonSerializerSettings outSerializerSettings;

        public MainScript()
        {
            ParseConfig();

            this.inputQueue = new ConcurrentQueue<WebInput>();
            this.retQueue = new ConcurrentQueue<KeyValuePair<string, object>>();
            this.Tick += OnTick;
            this.Tick += delegate { networkWaitHandle.Set(); }; // Signal network thread when a frame is ticked

            // Set up network worker, which exchanges data between plugin and server
            ws = new WebSocket(url);
            ws.OnMessage += WS_OnMessage;
            CreateWorkerThread();

            // Create Extension Manager instance
            ExtensionManager.CreateInstance();

            // Serializer settings
            outSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new WritablePropertiesOnlyResolver()
            };
        }

        private void OnTick(object sender, EventArgs e)
        {
            cacheData = GameData.GetData();
            
            // Tick extension manager
            ExtensionManager.Instance.Update();

            // Check for inputs
            WebInput input;
            while (inputQueue.TryDequeue(out input))
            {
                try
                {
                    Logger.Log("Executing " + input.Cmd + " " + input.Arg);
                    object retVal = input.Execute();
                    retQueue.Enqueue(new KeyValuePair<string, object>(input.UID, retVal));
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                }
            }
        }

        private void ParseConfig()
        {
            ScriptSettings settings;
            string path1 = @".\scripts\WebScriptHook.ini";
            string pathFallback = @".\scripts\WebScriptHook\WebScriptHook.ini";
            if (File.Exists(path1))
            {
                settings = ScriptSettings.Load(path1);
            }
            else
            {
                // fallback
                settings = ScriptSettings.Load(pathFallback);
            }
            string host = settings.GetValue("Core", "HOST", "localhost");
            string port = settings.GetValue("Core", "PORT", "25555");
            int interval = settings.GetValue("Core", "INTERVAL", 10);
            Logger.Enable = settings.GetValue("Core", "LOGGING", false);
            Logger.Location = @".\scripts\WebScriptHook.log";

            url = "ws://" + host + ":" + port + "/pushws";
        }

        // Worker exchanges data between plugin and server
        private void CreateWorkerThread()
        {
            Thread networkThread = new Thread(Worker_OnTick);
            networkThread.IsBackground = true;
            networkThread.Start();
        }

        private void Worker_OnTick()
        {
            while (true)
            {
                // Wait until a tick happens in the game
                // Because otherwise we'll waste CPU sending the same GameData to the server
                networkWaitHandle.WaitOne();

                try
                {
                    // Check if connection is alive. If not, attempt to connect to server
                    // WS doesn't throw exceptions when connection fails or unconnected
                    if (!ws.IsAlive) ws.Connect();
                    // Send game stats data (updates the stats cached on server)
                    ws.Send(JsonConvert.SerializeObject(cacheData, outSerializerSettings));
                    // Send return values
                    KeyValuePair<string, object> retPair;
                    while (retQueue.TryDequeue(out retPair))
                    {
                        // Serialize the object to JSON then send back to server.
                        // "RET:" is the "header" for return values
                        // Word of warning: If some extension attempts to send an object that cannot be seralized, 
                        // this iteration of worker update will be terminated. Whatever is left on the queue may be unsent.
                        ws.Send("RET:" + JsonConvert.SerializeObject(retPair, outSerializerSettings));
                    }
                }
                catch (Exception exc)
                {
                    Logger.Log(exc.ToString());
                }
            }
        }

        private void WS_OnMessage(object sender, MessageEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data)) return;
            WebInput input = JsonConvert.DeserializeObject<WebInput>(e.Data);
            if (input != null)
            {
                inputQueue.Enqueue(input);
            }
        }
    }
}
