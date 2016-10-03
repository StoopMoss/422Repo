using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace CS422
{
	public class WebServer
	{	
		//private readonly byte validatorLock;

		public WebServer ()
		{}

		public static bool Start(int port, int numberOfThreads)
		{	
			bool active = true;
						
			if (numberOfThreads <= 0)
			{
				numberOfThreads = 64;
			}
			
			// Set TCP 
			TcpListener listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
			Console.WriteLine ("started TCPListener");

			while (active) 
			{			
				//
				/*
				• Accept	new	TCP	socket	connection
				• Get a	thread	from	the	thread	pool	and	pass	it	the	TCP	socket
				• Repeat
				*/
				Validator validator = new Validator();

				TcpClient client = new TcpClient();
				client = listener.AcceptTcpClient();
				
				NetworkStream networkStream = client.GetStream();

				// read and validate what was read
				active = ReadFromNetworkStream(networkStream);
				
				if (active)
				{
					Console.WriteLine("about to Write");
					WriteResponseToClient (networkStream, responseTemplate, Validator);
				}
				else 
				{
					// read failed, close connection
					networkStream.Dispose();
					client.Close();
					return active;
				}
				networkStream.Dispose();
			}

			client.Close();
			return true;			
		}

		// 
		// Request methods
		//
		// Reads from network stream and validates as it goes
		public static bool ReadFromNetworkStream(Stream networkStream)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0, totalBytesRead = 0;
			
			// //Console.WriteLine("in Read");
			// if (networkStream.CanRead) 
			// {
			// 	do {
			// 		// read from stream until buffer is full or Read() returns 0
			// 		bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
			// 		if (bytesRead == 0)
			// 		{
			// 			break;
			// 		}
			// 		Console.WriteLine("Read " + bytesRead.ToString() + "bytes");
			//		validator.ValidateRequest(buffer);
			// 		validRequest = ValidateRequest(buffer);

			// 		// If valid and full no more reading is required
			// 		// If not full then request has not been read fully
			// 		if ( _fullRequest && validRequest)
			// 		{
			// 			// stop reading and send back a response
			// 			return true;
			// 		}


			// 	} while (validRequest);
			// }
			return false;
		}

		private static WebRequest BuildRequest(TcpClient client)
		{
			return new WebRequest();
		}		

		// 
		// Response methods
		//
		public static bool WriteResponseToClient(NetworkStream networkStream, string responseTemplate, Validator validator)
		{
			int studentId = 11282717;

			// fill 
			string response = string.Format(responseTemplate, studentId, DateTime.Now, Validator.URL);
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

