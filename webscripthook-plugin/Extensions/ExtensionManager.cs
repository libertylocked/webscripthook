using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScriptHook.Extensions
{
    class ExtensionManager
    {
        static ExtensionManager instance = new ExtensionManager();

        Dictionary<string, Extension> extMap;
        List<ITickable> tickables = new List<ITickable>();

        public static ExtensionManager Instance
        {
            get { return instance; }
        }

        public ExtensionManager()
        {
            // Load all extensions
            var targetDir = @".\scripts\WebScriptHook\extensions";
            if (Directory.Exists(targetDir)) extMap = ExtensionLoader.LoadAllExtensionsFromDir(targetDir);
            // Add tickable extensions to a list
            foreach (var ext in extMap.Values)
            {
                if (ext is ITickable)
                {
                    tickables.Add(ext as ITickable);
                    Logger.Log("Found tickable extension: " + ext.GetType());
                }
            }
        }

        public void Update()
        {
            // Tick all tickable extensions
            foreach (var t in tickables)
            {
                t.Tick();
            }
        }

        public object CallExtension(string extensionId, object[] args)
        {
            Extension callee;
            if (extMap.TryGetValue(extensionId, out callee))
            {
                return callee.HandleCall(args);
            }
            else
            {
                return null;
            }
        }

        public static ExtensionManager CreateInstance()
        {
            instance = new ExtensionManager();
            return instance;
        }
    }
}
