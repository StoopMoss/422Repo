using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace CS422
{
	public class WebServer
	{
		//private readonly byte validatorLock;
    private static Thread[] _threadPool;
    private static BlockingCollection<TcpClient> _tcpClientCollection;
    private static bool _disposeThreads;
    private static int _threadCount;
    private static TcpListener _listener;
    private static List<WebService> _webServices;
    private static string _serviceLock = "a lock";

		public WebServer ()
		{

    }

		public static bool Start(int port, int numberOfThreads)
		{
			bool active = true;
      _tcpClientCollection = new BlockingCollection<TcpClient>();
  		_threadCount = numberOfThreads <= 0 ?  64 : numberOfThreads;

      // add services that this server can use
      _webServices = new List<WebService>();
      AddService(new DemoService());

			// Set TCP
			Console.WriteLine ("starting TCPListener");
			_listener = new TcpListener(IPAddress.Any, port);
			_listener.Start();
      Console.WriteLine ("listening on port: "+ port);

      // Create and start all threads
      _threadPool = new Thread[_threadCount];
			for (int i = 0; i < _threadCount; i++)
			{
        _threadPool [i] = new Thread (new ThreadStart (ConcurrentlyProcessTcpClient));
			}

			for (int i = 0; i < _threadCount; i++)
			{
				//• Accept	new	TCP	socket	connection
				TcpClient client = new TcpClient();
				client = _listener.AcceptTcpClient();
        _tcpClientCollection.Add(client);

				//• Get a	thread	from	the	thread	pool	and	pass	it	the	TCP	socket
				_threadPool [i].Start ();
			}

      Stop();

			return true;
		}

    public static void Stop()
    {
      for (int i = 0; i < _threadCount; i++)
			{
        _threadPool[i].Join();
      }
      _listener.Stop(); // cleanup listener..
    }

    public static void ConcurrentlyProcessTcpClient()
    {
      Console.WriteLine("in ConcurrentlyProcessTcpClient");

      //Get client to process
      TcpClient client = _tcpClientCollection.Take();
      Console.WriteLine("Thread Recived client");

      // process client
		  WebRequest request = BuildRequest(client);

      if(request == null)
      {
         Console.WriteLine("null request returned from Request builder. Returning");
         return;
      }
      Console.WriteLine("IN ConcurrentlyProcessTcpClient: Request was built");
      Console.WriteLine("Servicing request");
      //Find Service from list
      //Pass request to Service
      Service(request);
      Console.WriteLine("Request serviced");

    }

    public static void AddService(WebService service)
    {
      lock (_serviceLock)
      {
        _webServices.Add(service);
      }
    }

    public static void Service(WebRequest request)
    {
      bool serviceUsed = false;
      foreach (WebService service in _webServices)
      {
        Console.WriteLine("service.ServiceURI: "+ service.ServiceURI );
        request.PrintRequest();
        if (service.ServiceURI == request.URI)
        {
          Console.WriteLine("URI's matched, servicing");
          service.Handler(request);
          serviceUsed = true;
          break;
        }
      }

      if(serviceUsed == false)
      {
        Console.WriteLine("no service found for uri");
      }

    }

		// Reads from network stream and validates as it goes
		public static CS422.WebRequest ReadFromClientNetworkStream(TcpClient client)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0, totalBytesRead = 0;
			NetworkStream networkStream = client.GetStream();
			Validator validator = new Validator();
      validator.Request.StreamRef = networkStream;
			bool validRequest = false;

			// Create a 10 sec timer
			System.Timers.Timer timer = new System.Timers.Timer(10000);
			// Hook up the Elapsed event for the timer.
			timer.Elapsed += OnTimedEvent;
    	timer.Enabled = true;

			Console.WriteLine("in Read");
			if (networkStream.CanRead)
			{
				try // using this try to catch the IOException thrown by the timer event
				{
					// Set read timeout
					networkStream.ReadTimeout = 1500;
					timer.Start();
					do {
						try
						{
							// read from stream until buffer is full or Read() returns 0
							bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
							totalBytesRead += bytesRead;
						}
						catch (IOException)
						{
							return null;
						}
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
	            Console.WriteLine("Request is full and valid");
							// stop reading and send back a response
							return validator.Request;
				 		}
				 	} while (validRequest);
					timer.Stop();
				}
				catch (IOException)
				{
					return null;
				}
			}
			return null;
		}

		private static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
	    throw new IOException("ten seconds passed before double line break was reached");
		}

		private static WebRequest BuildRequest(TcpClient client)
		{
      Console.WriteLine("in BuildRequest");
			WebRequest request = ReadFromClientNetworkStream(client);
      Console.WriteLine("BuildRequest read FullRequest");

      // Read call builds request and returns it
      // Read will return null if request was invalid
			if (request != null)
			{
        Console.WriteLine("in if");
				return request;
			}

			// return null so caller can dispose of TCP client
      Console.WriteLine("Closing client");
			client.GetStream().Close();
			client.Close();
			return null;
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

