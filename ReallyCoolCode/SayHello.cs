using System;
using System.Threading;
using CommonBootStrapper;

namespace ReallyCoolCode
{
	public class SayHello
	{
        public bool Run { get; set; }

        public SayHello()
        {
            Run = true;
        }
		
        public void Execute()
		{
			while(Run)
			{
				Console.WriteLine("{0:HH:mm:ss:ffff} Hello World - ver 4.3", DateTime.Now);
				Thread.Sleep(250);
			}
		}
	}
}
