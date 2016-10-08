using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace CS422
{
	public class WebServer
	{	
		//private readonly byte validatorLock;
    private static Thread[] _threadPool;
    private static BlockingCollection<TcpClient> _tcpClientCollection;
    private static bool _disposeThreads;
    private static int _threadCount;

		public WebServer ()
		{}

		public static bool Start(int port, int numberOfThreads)
		{	
			bool active = true;
      _tcpClientCollection = new BlockingCollection<TcpClient>();
						
      // Set and store thread Count
			_threadCount = numberOfThreads <= 0 ?  64 : numberOfThreads;

			// Set TCP 
			Console.WriteLine ("starting TCPListener...");
			TcpListener listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
      Console.WriteLine ("Started ");

      // Create and start all threads
      int i = 0;
      _threadPool = new Thread[_threadCount];
			for (i = 0; i < _threadCount; i++)
			{        
				//_threadPool [i] = new Thread (() => ConcurrentlyProcessTcpClient);
        _threadPool [i] = new Thread (new ThreadStart (ConcurrentlyProcessTcpClient));
			}

      i = 0;
			while (i < _threadCount) 
			{			
				//• Accept	new	TCP	socket	connection
				TcpClient client = new TcpClient();
				client = listener.AcceptTcpClient();
        _tcpClientCollection.Add(client);

				//• Get a	thread	from	the	thread	pool	and	pass	it	the	TCP	socket
				_threadPool [i].Start ();
        
				//• Repeat
        i++;
			}
      
      // TODO: join threads....
      for (i = 0; i < _threadCount; i++)
			{
         //_threadPool[i].RequestStop();
        _threadPool[i].Join();
      }

      //cleanup listene..
      listener.Stop();

			return true;			
		}


		// 
		// Request methods
		//
    //
    public static void ConcurrentlyProcessTcpClient()
    {
      Console.WriteLine("in ConcurrentlyProcessTcpClient");

      //Get client to process
      TcpClient client = _tcpClientCollection.Take();
      Console.WriteLine("Thread Recived client");

      
      // process client
		  WebRequest request = BuildRequest(client);

      //Find Service from list
      //Pass request to Service
      //Service();
       
    }

		// Reads from network stream and validates as it goes
		public static bool ReadFromClientNetworkStream(TcpClient client)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0, totalBytesRead = 0;
			NetworkStream networkStream = client.GetStream();
			Validator validator = new Validator();
			bool validRequest = false;

			
			Console.WriteLine("in Read");
			if (networkStream.CanRead) 
			{
				do 
				{
					// read from stream until buffer is full or Read() returns 0
					bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
					if (bytesRead == 0)
			 		{
						break;
					}
					Console.WriteLine("Read " + bytesRead.ToString() + "bytes");
					
					validRequest = validator.ValidateRequest(buffer);

					// If valid and full no more reading is required
					// If not full then request has not been read fully
					if (validator.FullRequest && validRequest)
			 		{
						// stop reading and send back a response
						return true;
			 		}

			 	} while (validRequest);
			}
			return false;
		}

		private static WebRequest BuildRequest(TcpClient client)
		{
      Console.WriteLine("in BuildRequest");
			// Read from client 			
			bool requestIsValid = ReadFromClientNetworkStream(client);

			if (requestIsValid)
			{
				// build request object

				// option 1
				// have validator store a WebRequest object
				// That way it can build the request as it validates

				// option 2
				// Have validator store all bytes
				// then parse bytes again 
				// and build request that is known to be valid

			}
			else
			{
				// return null so caller can dispose of TCP client		
        Console.WriteLine("Closing client");
				client.GetStream().Close();
				client.Close();
				//client.Dispose();		
				return null;
			}

			return new WebRequest();
		}		

		// 
		// Response methods
		//
		public static bool WriteResponseToClient(NetworkStream networkStream, string responseTemplate, Validator validator)
		{
			int studentId = 11282717;

			// fill 
			string response = string.Format(responseTemplate, studentId, DateTime.Now, validator.URL);
			byte[] buffer = Encoding.ASCII.GetBytes(response);

			//Write to stream
			if (networkStream.CanWrite)
			{
				networkStream.Write(buffer, 0, buffer.Length);
				return true;
			}

			return false;
		}

   

	}
}

