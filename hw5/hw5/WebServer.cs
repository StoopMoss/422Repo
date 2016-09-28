using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace CS422
{
	public class WebServer
	{				
		private static string _URL;

		//Private members used for validation
		private static bool _validRequestLine;
		private static bool _validRequestHeaders;

		private static bool _fullMethod;
		private static bool _fullVersion;
		private static bool _fullURI;
		private static bool _fullHeaders;
		private static bool _fullRequest;

		public WebServer ()
		{}

		public static bool Start(int port, int numberOfThreads)
		{	
			bool active = true;

			// Set validation flags
			_validRequestLine = false;
			_validRequestHeaders = false;

			_fullMethod = false;
			_fullVersion = false;
			_fullURI = false;
			_fullHeaders = false;
			_fullRequest = false;

			if (numberOfThreads <= 0)
			{
				numberOfThreads = 64;
			}
			
			// Set TCP 
			TcpClient client = new TcpClient();
			TcpListener listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
			Console.WriteLine ("started TCPListener");


			/*
			• Accept	new	TCP	socket	connection
			• Get a	thread	from	the	thread	pool	and	pass	it	the	TCP	socket
			• Repeat
			*/

//			while (active) 
//			{
//				Console.WriteLine("Hi from ");
//				// blocking call to accept client
//				client = listener.AcceptTcpClient();
//				NetworkStream networkStream = client.GetStream();
//
//				// read and validate what was read
//				active = ReadFromNetworkStream(networkStream);
//				Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
//				Console.WriteLine("Just read from ");
//
//				if (active)
//				{
//					Console.WriteLine("about to Write");
//					WriteResponseToClient (networkStream, responseTemplate);
//				}
//				else 
//				{
//					// read failed, close connection
//					networkStream.Dispose();
//					client.Close();
//					return active;
//				}
//				networkStream.Dispose();
//			}

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
			bool validRequest = false;

			//Console.WriteLine("in Read");
			if (networkStream.CanRead) 
			{
				do {
					// read from stream until buffer is full or Read() returns 0
					bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
					if (bytesRead == 0)
					{
						break;
					}
					Console.WriteLine("Read " + bytesRead.ToString() + "bytes");
					validRequest = ValidateRequest(buffer);

					// If valid and full no more reading is required
					// If not full then request has not been read fully
					if ( _fullRequest && validRequest)
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
			
		}

		public static bool ValidateRequest(byte[] buffer)
		{	
			if (!_fullMethod)
			{
				// Check method for "GET "
				Console.WriteLine("Validate: Check Method");
				if (!ValidateRequestMethod(buffer))
				{ return false; }
			}
			if (!_fullURI && _fullMethod)
			{
				//check uri
				Console.WriteLine("Validate: Check URI");
				if (!ValidateRequestURI (buffer)) 
				{ return false;	}
			}
			if (!_fullVersion && _fullURI && _fullMethod)
			{
				// Check  for “HTTP/1.1”
				Console.WriteLine("Validate: Check Version");
				if(!ValidateRequestVersion(buffer) )
				{ return false; }

				if(_fullVersion) 
				{
					// Entire first line is full and valid
					_validRequestLine = true;
				}
			}
			if (!_fullHeaders && _fullVersion && _fullURI && _fullMethod)
			{
				Console.WriteLine("Validate: Check Headers");
				if( !ValidateRequestHeaders(buffer) )
				{
					Console.WriteLine("--------------------------------");
					return false; 
				}											
			}

			Console.WriteLine("_fullMethod "+ 
				_fullMethod.ToString()+"_fullURI "+ 
				_fullURI.ToString()+"_fullVersion"+
				_fullVersion.ToString()+"  _fullHeaders)"+
				_fullHeaders.ToString());

			//TODO: do i need full headers?
			if (_fullMethod && _fullURI && _fullVersion && _fullHeaders)
			{
				_fullRequest = true;
			}

			return true;
		}


		//accounts for space  "GET "
		public static bool ValidateRequestMethod (byte[] buffer)
		{			
			string requestSoFar = Encoding.ASCII.GetString(buffer);
			string expectedString = "GET ";
			int bytesToCheck = 4;

			Console.WriteLine("Validating request Method");

			if (requestSoFar.Length == 0)
			{
				//error...
				return false;
			}
			else if (requestSoFar.Length >= 4) 
			{				
				_fullMethod = true;
			}
			else 
			{				
				bytesToCheck = requestSoFar.Length;
			}

			for (int i = 0; i < bytesToCheck; i++)
			{
				if (requestSoFar[i] != expectedString[i])
				{
					return false;
				}
			}
			return true;				
		}

		public static bool ValidateRequestURI(byte[] buffer)
		{
			// account for space "URI/ "
			// assume everything before is correct "GET "
			// Set _URL to be used later
			Console.WriteLine("Validating request URI");

			string requestSoFar = Encoding.ASCII.GetString(buffer);
			int i = 0;
			int whitespaces = 0;
			int uriStart = 0, uriEnd = 0;

			while (i < requestSoFar.Length )
			{
				if(whitespaces == 1)
				{
					uriStart = i;
				}
				if (whitespaces == 2) 
				{
					uriEnd = i;
					_fullURI = true;
					break; 
				}
				else if (requestSoFar[i] == ' ')
				{
					whitespaces++;
				}
				else if (requestSoFar[i] == '\r' || requestSoFar[i] == '\n')
				{
					return false;
				}
				i++;
			}
			_URL = requestSoFar.Substring(uriStart, uriEnd);
			return true;
		}

		// Must account for newline
		public static bool ValidateRequestVersion (byte[] buffer)
		{			
			string requestSoFar = Encoding.ASCII.GetString(buffer);
			string versionSubstring;
			int i = 0;
			int whiteSpaceToSkip = 2;
			Console.WriteLine("Validating request Version");

			while(requestSoFar[i] != '\r' && requestSoFar[i+1] != '\n' && i < requestSoFar.Length)
			{
				if (0 == whiteSpaceToSkip)
				{
					versionSubstring = requestSoFar.Substring (i, 10);// 10 is for "HTTP/1.1" + '\r' + '\n'
					if (versionSubstring != "HTTP/1.1\r\n")
					{
						return false;
					}
					_fullVersion = true;
					return true;
				}
				else if(' ' == requestSoFar[i])
				{
					whiteSpaceToSkip--;
				}
				i++;
			}

			// TODO: make more robust i.e. garbage after version, how are line ends compared?						
			return true;
		}

		public static bool ValidateRequestHeaders(byte[] buffer)
		{
			String bufferAsString = Encoding.ASCII.GetString(buffer);
			bufferAsString = bufferAsString.Substring(0, bufferAsString.IndexOf("\r\n\r\n"));


			string[] headers = bufferAsString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

			for (int i = 1; i < headers.Length; i++)
			{
				String header = headers[i];
				Console.WriteLine("'header' = '{0}'", header);

				if (String.IsNullOrEmpty(header))
				{
					Console.WriteLine("Empty String!!!");
				}
				else
				{
					if (Char.IsLetter(header[0]) && header.Contains(":"))
					{
						Console.WriteLine("This line contains a colon AND a first-position letter.");
					}
					else{
						Console.WriteLine("DARN it's returning false!!!!");
						return false;
					}
				}
			}
			_fullHeaders = true;
			return true;
		}

		public static int GetStartOfHeaderBufferIndex(byte[] buffer)
		{
			string requestReadSoFar = Encoding.ASCII.GetString(buffer);
			int i = 0;

			Console.WriteLine("getting Index...");
			if(_validRequestLine)
			{
				// at this point the entire first line is valid
				// Therefore search to first "\r\n" and return index;
				while(requestReadSoFar[i] != '\r' && requestReadSoFar[i+1] != '\n')
				{
					//Console.WriteLine("GetIndex: requestReadSoFar[" + i.ToString() +"]: " +requestReadSoFar[i].ToString());
					//Console.WriteLine("GetIndex: requestReadSoFar[" + (i+1).ToString() +"]" + requestReadSoFar[i+1].ToString());
					i++;
				}
				Console.WriteLine("requestReadSoFar[i+2]: "+ requestReadSoFar[i+2].ToString());
				return i + 2;
			}
			return 0;
		}

		// 
		// Response methods
		//
		public static bool WriteResponseToClient(NetworkStream networkStream, string responseTemplate)
		{
			int studentId = 11282717;

			// fill 
			string response = string.Format(responseTemplate, studentId, DateTime.Now, _URL);
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

