using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace CS422	
{
	public class ThreadPoolSleepSorter : IDisposable
	{
		private TextWriter _textWriter;
		private BlockingCollection<byte> _collection;
		private ushort _threadCount;
		private Thread[] _readyThreadPool;
		private bool _disposeThreads;

		public ThreadPoolSleepSorter ()
		{}

		public ThreadPoolSleepSorter(TextWriter output, ushort threadCount)
		{
			_collection = new BlockingCollection<byte>();
			_textWriter = output;

			if (threadCount == 0) 
			{_threadCount = 64;	}
			else
			{_threadCount = threadCount;}

			// Create and start all threads
			_readyThreadPool = new Thread[_threadCount];
			for (int i = 0; i < _threadCount; i++)
			{
				//_readyThreadPool [i] = new Thread(() => ThreadWorkFunc());
				_readyThreadPool [i] = new Thread (new ThreadStart (ThreadWorkFunc));
				_readyThreadPool [i].Start ();
			}
		}

		public void Sort(byte[] values)
		{
			foreach (byte b in values)
			{
//				if (_disposeThreads) 
//				{
//					return;
//				}
				//awaken thread
				_collection.Add(b);
			}			
		}

		// ThreadWorkFunc (performs sleeping and displaying of values)
		void ThreadWorkFunc() 
		{
			byte value = 0;
			while (true) 
			{
				value = _collection.Take ();

				if (_disposeThreads)
				{ break; }

				Thread.Sleep (value*1000);
				_textWriter.WriteLine (value);
			}
		}

		public void Dispose()
		{	
			_collection.Add (1);
			_disposeThreads = true;
			GC.SuppressFinalize(this);
		}
	}
}

