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
				ReadFromNetworkStream(networkStream, ref buffer);
				active = ValidateRequest (buffer);

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

		public static bool ReadFromNetworkStream(NetworkStream networkStream, ref byte[] buf)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0;
			int totalBytesRead = 0;

			if (networkStream.CanRead) 
			{
				do {
					// read from stream until buffer is full or Read() returns 0
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
			if (ValidateHTTPRequestLine (buffer))
			{
				if (ValidateHTTPHeaders (buffer)) 
				{
					return true;
				}
			} 
			return false;			
		}

		public static bool ValidateHTTPRequestLine(byte[] buffer)
		{			
			// get first line of request (The request line)
			string requestLine = GetRequestLine(buffer);
			string versionSubstring;

			//1. Only allow GET
			// Check HTTP Method for "GET " at start of string
			if (requestLine.Length > 4)
			{
				string actual = requestLine.Substring (0, 4);

				if (actual != "GET ")
				{
					return false;
				}
			}

			//2. Allow any valid URI
			//TODO: what to do about URI's

			//3. Allow HTTP/1.1
			// Traverse over two whitespaces then validate substring
			int i = 0;
			int whiteSpaceToSkip = 2;
			while(requestLine[i] != '\r' && requestLine[i+1] != '\n' && i < requestLine.Length)
			{
				if (0 == whiteSpaceToSkip)
				{
					versionSubstring = requestLine.Substring (i);
					break;
				}
				else if(' ' == requestLine[i])
				{
					whiteSpaceToSkip--;
				}

				i++;
			}

			// validate version
			// TODO: make more robust i.e. garbage after version, how are line ends compared?
			if (versionSubstring != "HTTP/1.1")
			{
				return false;
			}

			// CRLF
			//TODO: make sure request line ends correctly
//			string ending = requestLine.Substring(requestLine.Length - 2);
//			if (ending != "\r\n")
//			{
//				return false;
//			}

			return true;
		}

		public static string GetRequestLine (byte[] buffer)
		{
			char[] requestLine = new char[2048];
			int i = 0;

			// Read first line of request
			while (buffer[i] != '\r' && buffer[i+1] != '\n' && i != 2048) 
			{
				requestLine [i] = (char)buffer[i];	
				i++;
			}
			return new string (requestLine);
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

