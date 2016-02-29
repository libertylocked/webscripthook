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

        public static ExtensionManager Instance
        {
            get { return instance; }
        }

        public ExtensionManager()
        {
            // Load all extensions
            var targetDir = @".\scripts\WebScriptHook\extensions";
            if (Directory.Exists(targetDir)) extMap = ExtensionLoader.LoadAllExtensionsFromDir(targetDir);
        }

        public void Update()
        {
            // Tick all the extensions
            //foreach (var ext in extMap.Values)
            //{
            //    ext.Update();
            //}
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
