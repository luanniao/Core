using System;
using System.Reflection;
using System.Runtime.Loader;

namespace LuanNiao.Yggdrasil
{
    public class RootContext : AssemblyLoadContext
    { 
        private readonly AssemblyDependencyResolver _resolver;

        public RootContext(string pluginPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }
        
        protected override Assembly Load(AssemblyName name)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
            { 
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }

}
