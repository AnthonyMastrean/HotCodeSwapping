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

        private static readonly Threshold Threshold = new Threshold(TimeSpan.FromSeconds(3));
        private static UnloadAction onStop;

        public static void Main()
        {
            var watcher = new FileSystemWatcher(CodePath, "ReallyCoolCode.dll");
            watcher.Changed += OnCodeChanged;
            watcher.Created += OnCodeChanged;
            watcher.EnableRaisingEvents = true;

            onStop = CreateDomainAndStartExecuting();

            Console.Read();
        }

        private static void OnCodeChanged(object sender, FileSystemEventArgs args)
        {
            if(Threshold.EventIsUnderThreshold())
                return;

            Console.WriteLine("Code changed");
            ThreadPool.QueueUserWorkItem(o => onStop());

            onStop = CreateDomainAndStartExecuting();
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