using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CS422
{
	public class WebRequest
	{
		private string _method;
		private string _uri;
		private string _httpVersion;
		private Dictionary<string, string> _headers;
		private ConcatStream _body;
		private NetworkStream _networkStream;
		
		private int _studentId = 11282717;

    //////////////
    //Properties
    public string Method
    {
      get{return _method;}
      set{_method = value;}
    }
		public string URI
    {
      get{return _uri;}
      set{ _uri = value;}
    }
		public string HTTPVersion
    {
      get{return _httpVersion;}
      set{_httpVersion = value;}
    }
		public Dictionary<string, string> Headers
    {
      get{return _headers;}
      set{_headers = value;}
    }
		public ConcatStream Body
		{
      get{return _body;}
      set{_body = value;}
    }
    public NetworkStream StreamRef
		{
      get{return _networkStream;}
      set{_networkStream = value;}
    }
    

    //Constructor
		public WebRequest ()
		{
		}

		////////////
		// Methods 
		//
		public void WriteNotFoundResponse(string pageHTML)
		{
			// generate HTTP response 
			string HtmlResponseTemplate = GetHtmlResponseTemplate(404);

			//Write to stream
			WriteToNetworkStream(HtmlResponseTemplate);
		}

		public bool WriteHTMLResponse(string htmlString)
		{
			// generate HTTP response 
			string HtmlResponseTemplate = GetHtmlResponseTemplate(200);
			string HtmlResponse = FormatResponseTemplate(HtmlResponseTemplate);
			//Write to stream
			bool successfulWrite = WriteToNetworkStream(HtmlResponseTemplate);
			return successfulWrite;
		}


		public string FormatResponseTemplate(string template)
		{
			string response = string.Format(template, _studentId, DateTime.Now);

      throw new NotImplementedException();
      //return response;
		}



		public bool WriteToNetworkStream(string htmlResponse)
		{
			if (_networkStream.CanWrite)
			{
				byte[] buffer = Encoding.ASCII.GetBytes(htmlResponse);
				_networkStream.Write(buffer, 0, buffer.Length);
				return true;
			}
			throw new Exception("Network stream was unwriteable");				
		}

		public string GetHtmlResponseTemplate(int statusCode)
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

