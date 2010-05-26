using System;
using System.Threading;
using CommonBootStrapper;

namespace ReallyCoolCode
{
	public class SayHelloBootStrapper : MarshalByRefObject, IBootStrapper
	{
		private readonly SayHello hello = new SayHello();
		private Thread thread;

		public void AsyncStart()
		{
			Console.WriteLine("Starting");
			thread = new Thread(() =>
			{
				Console.WriteLine("Started");
				hello.Execute();
			});
			thread.Start();
		}

		public void StopAndWaitForCompletion()
		{
			Thread.Sleep(2000);
			Console.WriteLine("Stopping");
			hello.Run = false;
			thread.Join();
			Console.WriteLine("Stopped");
		}
	}
}