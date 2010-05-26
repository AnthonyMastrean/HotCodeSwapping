using System;
using System.IO;
using System.Threading;
using CommonBootStrapper;

namespace HotCodeSwapping
{
    public delegate void UnloadAction();

    public static class Program
    {
        private const string CodePath = @"..\..\..\ReallyCoolCode\bin\debug";
        private const string AssemblyName = "ReallyCoolCode";
        private const string TypeName = "ReallyCoolCode.SayHelloBootStrapper";

        private static readonly TimeBasedThreshold threshold = new TimeBasedThreshold(TimeSpan.FromSeconds(3));
        private static UnloadAction OnStop = null;

        public static void Main()
        {
            FileSystemEventHandler codeChanged = (sender, args) =>
            {
                if (threshold.EventIsUnderThreshold())
                    return;

                Console.WriteLine("Code changed");
                ThreadPool.QueueUserWorkItem(o =>
                {
                    OnStop();
                });

                OnStop = CreateDomainAndStartExecuting();
            };

            FileSystemWatcher watcher = new FileSystemWatcher(CodePath, "ReallyCoolCode.dll");
            watcher.Changed += codeChanged;
            watcher.Created += codeChanged;
            watcher.EnableRaisingEvents = true;

            OnStop = CreateDomainAndStartExecuting();

            Console.Read();
        }

        private static UnloadAction CreateDomainAndStartExecuting()
        {
            var domain = new HotSwappableAppDomain(CodePath);
            var bootstrapper = domain.CreateInstanceAndUnwrap<IBootStrapper>(AssemblyName, TypeName);
            bootstrapper.AsyncStart();

            return () =>
            {
                bootstrapper.StopAndWaitForCompletion();
                domain.Dispose();
            };
        }
    }
}