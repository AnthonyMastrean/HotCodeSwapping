using System;

namespace HotCodeSwapping
{
    public class HotSwappableAppDomain : IDisposable
    {
        private readonly AppDomain domain;

        public HotSwappableAppDomain(string codePath)
        {
            this.domain = AppDomain.CreateDomain("foo", null, new AppDomainSetup
            {
                ApplicationBase = codePath,
                ShadowCopyFiles = "true",
            });
        }

        public T CreateInstanceAndUnwrap<T>(string assemblyName, string typeName)
        {
            return (T)domain.CreateInstanceAndUnwrap(assemblyName, typeName);
        }

        public void Dispose()
        {
            AppDomain.Unload(domain);
        }
    }
}