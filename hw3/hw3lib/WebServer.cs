using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

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

		public WebServer ()
		{}

		public static bool Start(int _port, string responseTemplate)
		{
			bool active = true;

			// Set validation flags
			_validRequestLine = false;
			_validRequestHeaders = false;

			_fullMethod = false;
			_fullVersion = false;
			_fullURI = false;
			_fullHeaders = false;


			// Set TCP 
			TcpClient client = new TcpClient();
			TcpListener listener = new TcpListener(IPAddress.Any, _port);
			listener.Start();

			while (active) 
			{
				// blocking call to accept client
				client = listener.AcceptTcpClient();
				NetworkStream networkStream = client.GetStream();

				// read and validate what was read
				active = ReadFromNetworkStream(networkStream);

				if (active)
				{
					WriteResponseToClient (networkStream, responseTemplate);
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
		public static bool ReadFromNetworkStream(NetworkStream networkStream)
		{
			byte[] buffer = new byte[4096];
			int bytesRead = 0, totalBytesRead = 0;
			string requestReadSoFar;
			bool validRequest = false;

			if (networkStream.CanRead) 
			{
				do {
					// read from stream until buffer is full or Read() returns 0
					bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
					totalBytesRead += bytesRead;
					validRequest = ValidateRequest(buffer, totalBytesRead);

					//check to see if double return has been reached
					if (validRequest)
					{
						// stop reading and send back a response
						return true;
					}

				} while (bytesRead != 0);
			}
			return false;
		}

		public static bool ValidateRequest(byte[] buffer, int bytesRead)
		{	
			if (!_fullMethod)
			{
				// Check method for "GET "
				if (!ValidateRequestMethod(buffer))
				{ return false; }
			}
			else if (!_fullURI)
			{
				//check uri
				if (!ValidateRequestURI (buffer)) 
				{ return false;	}
			}
			else if (!_fullVersion)
			{
				// Check  for “HTTP/1.1”
				if(!ValidateRequestVersion(buffer) )
				{ return false; }

				if(_fullVersion) 
				{
					// Entire first line is full and valid
					_validRequestLine = true;
				}
			}
			//TODO: Header validation
			else if (!_fullHeaders)
			{
				if( !ValidateRequestHeaders(buffer) )
				{ return false; }											
			}
			return true;
		}


		//accounts for space  "GET "
		public static bool ValidateRequestMethod (byte[] buffer)
		{
			string requestSoFar = buffer.ToString();
			string expectedString = "Get ";
			int bytesToCheck = 4;

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

			string requestSoFar = buffer.ToString ();
			int i = 0;
			int whitespaces = 0;

			while (i < requestSoFar.Length )
			{
				if (whitespaces == 2) 
				{
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
			return true;
		}

		// Must account for newline
		public static bool ValidateRequestVersion (byte[] buffer)
		{			
			string requestSoFar = buffer.ToString();
			string versionSubstring;
			int i = 0;
			int whiteSpaceToSkip = 2;

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
			// Get the headers as a string
			string headerString = buffer.ToString();
			int startOfHeadersIndex = GetStartOfHeaderBufferIndex (buffer);
			if(startOfHeadersIndex == 0)
			{
				// Error: shouldn't be in this function if 0 is returned from getIndex
				return false;
			}
			headerString = headerString.Substring (startOfHeadersIndex);

			int i = 0;
			bool firstCharacter = true, hitColon = false;
			while (i < headerString.Length && !_fullHeaders) 
			{
				// inner loop is for one header line
				while (headerString [i] != '\r') 
				{
					// check each header...
					if (firstCharacter) 
					{
						if (!char.IsLetter (headerString [i])) 
						{
							return false;
						} 
						else 
						{
							firstCharacter = false;
						}
					} 
					else 
					{
						if (headerString [i] == ':') 
						{
							if(hitColon)
							{
								// Already hit a colon on this line
								return false;
							}
							hitColon = true;
						}
					}
					i++;
				}

				if (!hitColon) {
					//Invalid: finished line and no colon was encountered
					return false;
				}

				string endOfString = headerString.Substring (i, 4);
				if (endOfString.Equals ("\r\n\r\n")) {
					// Account for double Newline
					_fullHeaders = true;
				}

				// skip '\r\n'
				i += 2;
			}
			return true;
		}

		public static int GetStartOfHeaderBufferIndex(byte[] buffer)
		{
			string requestReadSoFar = buffer.ToString();
			int i = 0;

			if(_validRequestLine)
			{
				// at this point the entire first line is valid
				// Therefore search to first "\r\n" and return index;
				while(requestReadSoFar[i] != '\r' && requestReadSoFar[i+1] != '\n')
				{
					i++;
				}
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
			_URL = "/blah/ForNow"; // TODO: need to get from request

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

