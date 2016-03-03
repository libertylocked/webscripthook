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
        List<Extension> tickableExtensions = new List<Extension>();

        public static ExtensionManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the names of loaded extensions
        /// </summary>
        public string[] ExtensionNames
        {
            get { return extMap.Keys.ToArray(); }
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
                    tickableExtensions.Add(ext);
                    Logger.Log("Found tickable extension: " + ext.GetType());
                }
            }
        }

        public void Update()
        {
            // Tick all tickable extensions
            for (int i = tickableExtensions.Count - 1; i >= 0; i--)
            {
                try
                {
                    (tickableExtensions[i] as ITickable).Tick();
                }
                catch (Exception ex)
                {
                    DropExtension(tickableExtensions[i]);
                    Logger.Log(ex);
                }
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

        private void DropExtension(Extension extension)
        {
            // Drop from being ticked
            if (extension != null && tickableExtensions.Contains(extension))
            {
                tickableExtensions.Remove(extension);
            }

            // Drop from being called
            foreach (var item in extMap.Where(pair => pair.Value == extension).ToList())
            {
                extMap.Remove(item.Key);
            }
        }
    }
}
