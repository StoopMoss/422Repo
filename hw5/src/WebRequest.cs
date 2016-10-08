using System;
using System.IO;
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
		private Stream _body;
    private string _htmlBody;
		private NetworkStream _networkStream;
		
		private int _studentId = 11282717;

    private string _responseTemplate = "{0} {1} {2}\r\n"+ 
    "Content-Type: text/html\r\n"+
    "Content-Length: {3}\r\n"+
    "\r\n\r\n"+
    "{4}";

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
		public Stream Body
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
		public void WriteNotFoundResponse(string html)
		{
      // put the html thats passed in,
      // into the body of the http response
      _htmlBody = html;

			// generate HTTP response 
			string HttpResponse = GetHtmlResponse(404);

			//Write to stream
			WriteToNetworkStream(HttpResponse);
		}

		public bool WriteHTMLResponse(string html)
		{
      // put the html thats passed in,
      // into the body of the http response
      _htmlBody = html;

			// generate HTTP response 
			string HtmlResponseTemplate = GetHtmlResponse(200);

			//Write to stream
			bool successfulWrite = WriteToNetworkStream(HtmlResponseTemplate); 
			return successfulWrite;
		}


    ///////////////////////////
    /// Helper Methods ////////
    ///////////////////////////
		// public string FormatResponseTemplate(string template)
		// {
		// 	string response = string.Format(template, _studentId, DateTime.Now);

    //   throw new NotImplementedException();
    //   //return response;
		// }

		public bool WriteToNetworkStream(string HttpResponse)
		{
      Console.WriteLine("hello");
			if (_networkStream.CanWrite)
			{
        Console.WriteLine("int if");
				byte[] buffer = Encoding.ASCII.GetBytes(HttpResponse);
				_networkStream.Write(buffer, 0, buffer.Length);
				return true;
			}
			throw new Exception("Network stream was unwriteable");				
		}

    // Creates a valid http response based off the status code given and 
    // the member template
		public string GetHtmlResponse(int statusCode)
		{
      // {0} Version 
      // {1} statusCode 
      // {2} ReasonPhrase
      // {3} Content length (use the length of the body for this in exact bytes)      
      // {4} Body
      // TODO: find elegant way to add many headers
			switch(statusCode)
			{
				case 200:
				return   string.Format(_responseTemplate,
         "HTTP/1.1", 200, "OK", _body.Length, _body );
        case 404:
				return   string.Format(_responseTemplate,
         "HTTP/1.1", 404, "NotFound", _body.Length, _body);

				default:
				return "";
			}
		}
	}
}

