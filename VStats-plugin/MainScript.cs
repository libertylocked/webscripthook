using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GTA;
using Newtonsoft.Json;
using WebSocketSharp;

namespace VStats_plugin
{
    class MainScript : Script
    {
        private string url;
        private int sleepTime;

        GameData cacheData;
        ConcurrentQueue<WebInput> inputQueue;
        Thread workerThread;

        public MainScript()
        {
            ParseConfig();

            this.inputQueue = new ConcurrentQueue<WebInput>();
            this.Tick += OnTick;

            CreateWorkerThread();
        }

        private void OnTick(object sender, EventArgs e)
        {
            cacheData = GameData.GetData();
            
            // Check for inputs
            WebInput input;
            if (inputQueue.TryDequeue(out input))
            {
                try
                {
                    input.Execute();
                }
                catch (Exception ex)
                {
                    UI.Notify("VStats func failed: " + input.Cmd + " " + input.Arg);
                    Logger.Log(ex.ToString());
                }
            }
        }

        private void ParseConfig()
        {
            var settings = ScriptSettings.Load(@".\scripts\VStats.ini");
            string port = settings.GetValue("Core", "PORT", "8080");
            int interval = settings.GetValue("Core", "INTERVAL", 10);
            Logger.Enable = settings.GetValue("Core", "LOGGING", false);

            //url = "http://localhost:" + port + "/push";
            url = "ws://localhost:" + port + "/pushws";
            sleepTime = interval;
        }

        private void CreateWorkerThread()
        {
            workerThread = new Thread(ThreadProc_PostJSON);
            workerThread.Start();
        }

        private void ThreadProc_PostJSON()
        {
            WebSocket ws = new WebSocket(url);
            ws.OnMessage += WS_OnMessage;

            while (true)
            {
                try
                {
                    if (!ws.IsAlive) ws.Connect();
                    ws.Send(JsonConvert.SerializeObject(cacheData));
                }
                catch
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(sleepTime);
            }
        }

        private void WS_OnMessage(object sender, MessageEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data)) return;
            WebInput input = JsonConvert.DeserializeObject<WebInput>(e.Data);
            if (input != null)
            {
                inputQueue.Enqueue(input);
                Logger.Log(e.Data);
            }
        }
    }
}
