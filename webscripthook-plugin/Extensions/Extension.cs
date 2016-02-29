using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScriptHook.Extensions
{
    public abstract class Extension
    {
        public abstract object HandleCall(object[] args);
    }
}
