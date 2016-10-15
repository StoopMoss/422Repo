using System;
using System.Text;
using System.IO;
using CS422;

namespace ConsoleApp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Hello World!");

			testConcatStream();
		}

		static public void testConcatStream()
		{
			byte[] buffer1 = Encoding.ASCII.GetBytes ("");
			byte[] buffer2 = Encoding.ASCII.GetBytes ("");

			Stream stream1 = new MemoryStream();
			Stream stream2 = new MemoryStream();

			Console.WriteLine("Test Concat stream constructor 1: ");
			ConcatStream stream = new ConcatStream(stream1, stream2);
			if(stream == null)
			{
				Console.WriteLine("Failed: Stream created was null");
			}
			else
			{
				Console.WriteLine("Passed");
			}
		}
	}
}
