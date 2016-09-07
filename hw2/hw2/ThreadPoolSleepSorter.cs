using System;
using System.IO;
using System.Threading;

namespace CS422	
{
	public class ThreadPoolSleepSorter : IDisposable
	{
		private TextWriter _textWriter;
		private ushort _threadCount;
		private Thread[] _threadPool;//[1000000];
		private bool _stopThread;

		public ThreadPoolSleepSorter ()
		{}

		public ThreadPoolSleepSorter(TextWriter output, ushort threadCount)
		{
			_textWriter = output;
			_stopThread = false;

			if (threadCount == 0) 
			{_threadCount = 64;	}
			else
			{_threadCount = threadCount;}

			// Create and start all threads
			_threadPool = new Thread[10000];
			for (int i = 0; i < _threadCount; i++)
			{
				_threadPool [i] = new Thread (new ThreadStart(Work));
				_threadPool [i].Start ();
			}

		}

		public void Sort(byte[] values)
		{
			
			
		}

		public void Work()
		{
			while (_stopThread != true) 
			{
				
			}
			
		}

		public void Dispose()
		{				
			Dispose();
			GC.SuppressFinalize(this);
		}
	}
}

