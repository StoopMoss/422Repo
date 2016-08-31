using System;
using CS422;

namespace FirstMonoProject
{
	class MainClass
	{
		public static NumberedTextWriter writer;
		public static IndexedNumsStream stream;

		public static void Main (string[] args)
		{
			// Uncomment for Text Writer demonstration
			Console.WriteLine ("Hello");
        	//writer = new NumberedTextWriter(Console.Out);

			//writer.WriteLine("Hello.  Welcome.");
        	//writer.WriteLine("blah");
			//Console.WriteLine ();


			// Uncomment for Stream demonstration
			//byte[] buffer = new byte[1000];
			//stream = new IndexedNumsStream (2500);

			//stream.Read(buffer, 0, 4);
			//stream.Read(buffer, 250, 4);
			//stream.Read(buffer, -68, 4);
			//stream.Read(buffer, 2499, 4);
			//stream.Read(buffer, 1588, 600);
		}
	}
}
