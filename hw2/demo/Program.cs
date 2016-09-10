using System;
using CS422;
using System.Threading;

namespace demo
{
	class MainClass
	{
		public static PCQueue queue;

		public static void Main (string[] args)
		{
			int outvalue = 0;
			Console.WriteLine ("QueueTests");

			queue = new PCQueue ();

			for (int i = 0; i < 20; i++)
			{
				queue.Enqueue (i);
			}

			queue.printList ();

			for (int i = 0; i < 20; i++)
			{
				queue.Dequeue (ref outvalue);
			}

			Thread t1 = new Thread (() => Dtest());
			Thread t2 = new Thread (() => Etest());

			t1.Start();
			t2.Start();
					
			Console.WriteLine ("SorterTests");
			ThreadPoolSleepSorter sorter = new ThreadPoolSleepSorter (Console.Out, 10);
			byte[] values = { 1, 7, 8, 2, 3, 4, 5, 6, 9, 10 };
			sorter.Sort (values);
			sorter.Dispose();

			Console.WriteLine("done");
		}

		static public void Dtest()
		{
			int outvalue = 0;
			for (int i = 0; i < 500; )
			{
				queue.Dequeue (ref outvalue);
			}
		}
			
		static public void Etest()
		{
			for (int i = 0; i < 500; )
			{
				queue.Enqueue (1);
			}
		}
	}
}
