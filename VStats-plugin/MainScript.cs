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

namespace VStats_plugin
{
    class MainScript : Script
    {
        private string url;
        private int sleepTime;

        GameData cacheData;
        ConcurrentQueue<WebInput> inputQueue;
        Thread workerThread;
        WebClient client;

        public MainScript()
        {
            ParseConfig();

            this.inputQueue = new ConcurrentQueue<WebInput>();
            this.client = new WebClient();
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

        private void CreateWorkerThread()
        {
            workerThread = new Thread(ThreadProc_PostJSON);
            workerThread.Start();
        }

        private void ThreadProc_PostJSON()
        {
            while (true)
            {
                try
                {
                    var values = new NameValueCollection();
                    values["d"] = JsonConvert.SerializeObject(cacheData);
                    var response = client.UploadValues(url, values);
                    // Read response
                    if (response != null && response.Length > 0)
                    {
                        WebInput input = JsonConvert.DeserializeObject<WebInput>(Encoding.ASCII.GetString(response));
                        if (input != null)
                        {
                            inputQueue.Enqueue(input);
                            Logger.Log(Encoding.ASCII.GetString(response));
                        }
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(sleepTime);
            }
        }

        private void ParseConfig()
        {
            var settings = ScriptSettings.Load(@".\scripts\VStats.ini");
            string port = settings.GetValue("Core", "PORT", "8080");
            int interval = settings.GetValue("Core", "INTERVAL", 10);
            Logger.Enable = settings.GetValue("Core", "LOGGING", false);

            url = "http://localhost:" + port + "/push";
            sleepTime = interval;
        }
    }
}
