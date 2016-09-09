using System;
using CS422;

namespace demo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			ThreadPoolSleepSorter sorter = new ThreadPoolSleepSorter (Console.Out, 10);
			byte[] values = { 1, 7, 8, 2, 3, 4, 5, 6, 9, 10 };

			sorter.Sort (values);
		}
	}
}
