using System;
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
        private readonly string url;

        //private SemaphoreSlim PostDataSemaphore = new SemaphoreSlim(1, 1);
        Object lockThis = new Object();
        bool isStopped;
        GameData cacheData;

        public MainScript()
        {
            // SETUP
            var settings = ScriptSettings.Load(@".\scripts\VStats-plugin.ini");
            string port = settings.GetValue("Core", "PORT", "8080");
            url = "http://localhost:" + port + "/push";

            this.isStopped = true;
            this.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (isStopped)
            {
                Thread workerThread = new Thread(ThreadProc_PostJSON);
                workerThread.Start();
            }
            cacheData = GameData.GetData();
            //ThreadProc_PostJSON();
        }

        private void ThreadProc_PostJSON()
        {
            lock (lockThis)
            {
                isStopped = false;
                try
                {
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values["d"] = JsonConvert.SerializeObject(cacheData);
                        client.UploadValues(url, values);
                    }
                }
                catch 
                {
                    //Thread.Sleep(100);
                    throw new Exception(url);
                }
                isStopped = true;
            }
        }
    }
}
