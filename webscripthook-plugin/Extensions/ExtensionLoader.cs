using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebScriptHook.Extensions
{
    class ExtensionLoader
    {
        /// <summary>
        /// Get Extension subclasses from an assembly and instantiate them
        /// </summary>
        /// <param name="fileName">Filename of the assembly containing extensions</param>
        /// <returns>A list containing extension type and the extension instance</returns>
        public static List<Tuple<Type, Extension>> LoadExtensionsFromAssembly(string fileName)
        {
            var extensions = new List<Tuple<Type, Extension>>();
            var asm = Assembly.LoadFrom(fileName);
            var types = asm.GetTypes().Where(t => t.BaseType == typeof(Extension));
            foreach (var type in types)
            {
                extensions.Add(new Tuple<Type, Extension>(type, Activator.CreateInstance(type) as Extension));
            }
            return extensions;
        }

        /// <summary>
        /// Loads all extensions from a directory
        /// </summary>
        /// <param name="dirName">Directory to search the extensions in</param>
        /// <returns>A map from extension identifier string to the extension instance</returns>
        public static Dictionary<string, Extension> LoadAllExtensionsFromDir(string dirName)
        {
            var extMap = new Dictionary<string, Extension>();
            var fileNames = Directory.GetFiles(dirName, "*.dll", SearchOption.AllDirectories);
            foreach (var fileName in fileNames)
            {
                var loadedExtensions = LoadExtensionsFromAssembly(fileName);
                foreach (var pair in loadedExtensions)
                {
                    // The string used to identify this extension
                    string id = Path.GetFileNameWithoutExtension(fileName) + "." + pair.Item1;
                    extMap.Add(id, pair.Item2);
                    Logger.Log("Loaded extension: " + id);
                }
            }
            return extMap;
        }
    }
}
