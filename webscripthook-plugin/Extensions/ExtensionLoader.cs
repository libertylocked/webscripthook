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
            try
            {
                var asm = Assembly.LoadFrom(fileName);
                var types = asm.GetTypes().Where(t => t.BaseType == typeof(Extension));
                foreach (var type in types)
                {
                    try
                    {
                        extensions.Add(new Tuple<Type, Extension>(type, Activator.CreateInstance(type) as Extension));
                    }
                    catch (Exception ex)
                    {
                        // catch exceptions thrown by individual extensions in this assembly
                        Logger.Log("Failed to load " + type + " from " + fileName + ": " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to load assembly " + fileName + ": " + ex);
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
            try
            {
                var fileNames = Directory.GetFiles(dirName, "*.dll", SearchOption.AllDirectories);
                foreach (var fileName in fileNames)
                {
                    var loadedExtensions = LoadExtensionsFromAssembly(fileName);
                    foreach (var pair in loadedExtensions)
                    {
                        // The string used to identify this extension
                        string id = Path.GetFileNameWithoutExtension(fileName) + "." + pair.Item1;
                        if (extMap.ContainsKey(id))
                        {
                            Logger.Log("Failed to load extension (extension signature collision): " + id);
                        }
                        else
                        {
                            extMap.Add(id, pair.Item2);
                            Logger.Log("Extention loaded: " + id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to load extensions in " + dirName + ": " + ex);
            }
            return extMap;
        }
    }
}
