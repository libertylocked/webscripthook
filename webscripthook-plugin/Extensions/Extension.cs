using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScriptHook.Extensions
{
    public abstract class Extension
    {
        /// <summary>
        /// Gets the callable ID of the extension.
        /// </summary>
        /// <returns>ID string in format assemblyname.namespace.classname</returns>
        public string GetExtensionID()
        {
            return this.GetType().Assembly.GetName().Name + "." + this.GetType().FullName;
        }

        /// <summary>
        /// Answers a call to this extension
        /// </summary>
        /// <param name="args">Arguments passed with the call</param>
        /// <returns>Object to be returned to caller</returns>
        public abstract object HandleCall(object[] args);

        /// <summary>
        /// Calls another extension loaded in Extension Manager
        /// </summary>
        /// <param name="targetID">Callee's extension ID</param>
        /// <param name="args">Arguments passed with the call</param>
        /// <returns>Object returned by the callee. Null if call failed</returns>
        protected object CallExtension(string targetID, object[] args)
        {
            if (ExtensionManager.Instance != null)
            {
                return ExtensionManager.Instance.CallExtension(targetID, args);
            }
            else
            {
                return null;
            }
        }
    }
}
