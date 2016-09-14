using NUnit.Framework;
using System;
using System.Net.Sockets;
using System.Net;
using CS422;
using System.Text;

namespace hw3LibTests
{
	[TestFixture ()]
	public class Tests
	{
		private byte[] _validRequest;
		private byte[] _inValidRequest;
		private byte[] _inValidRequestBadVersion;
		private byte[] _inValidRequestLowerCaseGet;
		private TcpClient _client;
		private TcpListener _listener;

		private string _validString = "GET /hello.htm HTTP/1.1\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n" +
			"Host: www.tutorialspoint.com\n" +
			"Accept-Language: en-us\n" +
			"Accept-Encoding: gzip, deflate\n" +
			"Connection: Keep-Alive";

		private string _badGetString = "GE /hello.htm HTTP/1.1\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n" +
			"Host: www.tutorialspoint.com\n" +
			"Accept-Language: en-us\n" +
			"Accept-Encoding: gzip, deflate\n" +
			"Connection: Keep-Alive";

		private string _lowerCaseGetString = "get /hello.htm HTTP/1.1\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n" +
			"Host: www.tutorialspoint.com\n" +
			"Accept-Language: en-us\n" +
			"Accept-Encoding: gzip, deflate\n" +
			"Connection: Keep-Alive";

		private string _badVersionString = "GET /hello.htm HTTP/1.0\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n" +
			"Host: www.tutorialspoint.com\n" +
			"Accept-Language: en-us\n" +
			"Accept-Encoding: gzip, deflate\n" +
			"Connection: Keep-Alive";
		
		[SetUp]
		public void Init()
		{
			_validRequest = new byte[4096];
			_inValidRequest = new byte[4096];
			_client = new TcpClient();
			_listener = new TcpListener(IPAddress.Any, 4220);
			_listener.Start();

			_validRequest = Encoding.ASCII.GetBytes(_validString);
			_inValidRequest = Encoding.ASCII.GetBytes(_badGetString);
			_inValidRequestBadVersion = Encoding.ASCII.GetBytes(_badVersionString);
			_inValidRequestLowerCaseGet = Encoding.ASCII.GetBytes(_lowerCaseGetString);

		}

		[TearDown]
		public void Dispose()
		{
			_client.Close ();
			_listener.Stop ();
		}

		[Test ()]
		public void ReadFromStream()
		{
			// Arrange
			bool result = false;
			NetworkStream networkStream =  _client.GetStream ();

			// Act 
			result = WebServer.ReadFromNetworkStream(networkStream);

			// Assert
			Assert.True(result);
		}

		[Test ()]
		public void ValidateRequestWithValidRequest()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequest(_validRequest);
				
			// Assert
			Assert.True(result);
		}

		[Test ()]
		public void ValidateRequestWithInvalidRequest()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequest(_inValidRequest);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateHTTPRequestLineWithBadGet()
		{
			// Arrange
			bool result = false;
			byte[] buffer = _inValidRequest;

			// Act 
			result = WebServer.ValidateHTTPRequestLine(buffer);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateHTTPRequestLineWithBadHTTPVersion()
		{
			// Arrange
			bool result = false;

			byte[] buffer = _inValidRequestBadVersion;

			// Act 
			result = WebServer.ValidateHTTPRequestLine(buffer);

			// Assert
			Assert.False(result);
		}


	}
}

