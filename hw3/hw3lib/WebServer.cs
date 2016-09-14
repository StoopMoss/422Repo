using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace CS422
{
	public class WebServer
	{				
		public WebServer ()
		{}

		public static bool Start(int _port, string responseTemplate)
		{
			bool active = true;
			byte[] buffer = new byte[4096];
			TcpClient client = new TcpClient();

			TcpListener listener = new TcpListener(IPAddress.Any, _port);
			listener.Start();


			while (active) 
			{
				// blocking call to accept client
				client = listener.AcceptTcpClient();
				NetworkStream networkStream = client.GetStream();

				//read and validate what was read (TODO: exstract method(s))
				ReadFromNetworkStream(networkStream);
				//active = validateRequest (buffer);
//
//				if (active)
//				{
//					WriteResponseToClient (responseTemplate);
//				}
//				else 
//				{// read failed, close connection
//					networkStream.Dispose();
//					client.Close();
//					return active;
//				}
//				networkStream.Dispose();
			}

			client.Close();
			return true;			
		}

		public static bool ReadFromNetworkStream(NetworkStream networkStream)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0;
			int totalBytesRead = 0;

			if (networkStream.CanRead) 
			{
				do {
					bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
					totalBytesRead += bytesRead;
				} while (bytesRead != 0);
			} 
			else 
			{
				return false;
			}
			return true;
		}

		public static bool ValidateRequest (byte[] buffer)
		{			
			ValidateHTTPRequestLine(buffer);
			ValidateHTTPHeaders (buffer);
			return true;
		}

		public static bool ValidateHTTPRequestLine(byte[] buffer)
		{


			//1. Only allow GET
			//2. Allow any valid URI
			//3. Allow HTTP/1.1
			// CRLF
			return true;
		}

		public static char[] GetRequestLine (byte[] buffer)
		{
			char[] requestLine = new char[2048];
			int i = 0;

			// Read first line of request
			while (buffer[i] != '\r' && buffer[i+1] != '\n' && i != 2048) 
			{
				requestLine [i] = buffer[i];	
				i++;
			}
			return requestLine;
		}

		public static bool ValidateHTTPHeaders(byte[] buffer)
		{
			// ..
			// CRLF
			return true;
		}

		public static bool WriteResponseToClient(string responseTemplate)
		{
			// fill 
			return true;
		}

	}
}

