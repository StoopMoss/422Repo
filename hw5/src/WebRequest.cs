using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CS422
{
	public class WebRequest
	{
		private string _requestMethod;
		private string _requestURI;
		private string _requestHTTPVersion;
		private Dictionary<string, string> _requestHeaders;
		private ConcatStream _requestBody;
		private NetworkStream _networkStream;

		private int _studentId = 11282717;

		public WebRequest ()
		{
		}

		////////////
		// Methods 
		//
		public void WriteNotFoundResponse(string pageHTML)
		{
			// generate HTTP response 
			string HTTPResponseTemplate = GetHTMLResponseTemplate(404);

			//Write to stream
			WriteToNetworkStream(HTTPResponseTemplate);
		}

		public bool WriteHTMLResponse(string htmlString)
		{
			// generate HTTP response 
			string HTTPResponseTemplate = GetHTMLResponseTemplate(200);

			//Write to stream
			bool successfulWrite = WriteToNetworkStream(HTTPResponseTemplate);
			return successfulWrite;
		}


		public bool WriteToNetworkStream(string HTTPResponseTemplate)
		{
			string response = string.Format(HTTPResponseTemplate, _studentId, DateTime.Now);

			if (_networkStream.CanWrite)
			{
				byte[] buffer = Encoding.ASCII.GetBytes(response);
				_networkStream.Write(buffer, 0, buffer.Length);
				return true;
			}
			throw new Exception("Network stream was unwriteable");				
		}

		public string GetHTMLResponseTemplate(int statusCode)
		{
			switch(statusCode)
			{
				case 200:
				return   "";					
				default:
				return "";
			}

			//response line, 
			//response headers with the following at a minimum
			//Content-Type: text/html
			//Content-Length: ___
			//	where “___” is replaced with the actual content (body) length, in bytes.
			// Double break


		}
	}
}

