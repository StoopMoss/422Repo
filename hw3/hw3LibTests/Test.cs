using NUnit.Framework;
using System;
using System.Net.Sockets;
using System.Net;
using CS422;
using System.Text;
using System.IO;

namespace hw3LibTests
{
	[TestFixture ()]
	public class Tests
	{
		private byte[] _validRequest;
		private byte[] _validRequestWithBody;
		private byte[] _validRequestNoHeader;
		private byte[] _inValidRequest;
		private byte[] _inValidRequestBadVersion;
		private byte[] _inValidRequestLowerCaseGet;
		private byte[] _inValidRequestHeaderTwoColons;
		private byte[] _inValidRequestStartingColon;
		private byte[] _inValidRequestBadLineEndings;

		private TcpClient _client;
		private TcpListener _listener;

		private string _validString = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _RequestWithBodyString = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive\r\n" +
			"\r\n" +
			"this is a body\r\n";

		private string _badGetString = "GE /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _lowerCaseGetString = "get /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _badVersionString = "GET /hello.htm HTTP/1.0\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Connection: Keep-Alive";
		
		private string _BadHeaderTwoColonsString = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www:.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _BadHeaderStartingColonString = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n" +
			":Host www:.tutorialspoint.com\n" +
			"Accept-Language: en-us\n";

		private string _NoHeaderString = "GET /hello.htm HTTP/1.1\r\n";			

		private string _badLineEndsString = "GET /hello.htm HTTP/1.0\n" +
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
			_validRequestWithBody = Encoding.ASCII.GetBytes(_RequestWithBodyString);
			_validRequestNoHeader = Encoding.ASCII.GetBytes(_NoHeaderString);
			_inValidRequest = Encoding.ASCII.GetBytes(_badGetString);
			_inValidRequestBadVersion = Encoding.ASCII.GetBytes(_badVersionString);
			_inValidRequestLowerCaseGet = Encoding.ASCII.GetBytes(_lowerCaseGetString);
			_inValidRequestHeaderTwoColons = Encoding.ASCII.GetBytes (_BadHeaderTwoColonsString);
			_inValidRequestStartingColon = Encoding.ASCII.GetBytes (_BadHeaderStartingColonString);
			_inValidRequestBadLineEndings = Encoding.ASCII.GetBytes (_badLineEndsString);
		}

		[TearDown]
		public void Dispose()
		{
			_client.Close ();
			_listener.Stop ();
		}

		[Test ()]
		[TestCase()]
		public void ReadFromStream()
		{
			// Arrange
			bool result = false;

			// Case  
			Stream networkStream = new MemoryStream(_validRequest);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsTrue(result);

			// Case 
			result = false;
			networkStream = new MemoryStream(_validRequestWithBody);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsTrue(result);

			// Case 
			result = false;
			networkStream = new MemoryStream(_validRequestNoHeader);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsTrue(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequest);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequestBadVersion);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequestLowerCaseGet);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequestHeaderTwoColons);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequestStartingColon);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);

			// Case 
			result = true;
			networkStream = new MemoryStream(_inValidRequestBadLineEndings);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithValidString()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequestMethod(_validRequest);
				
			// Assert
			Assert.True(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithInvalidRequest()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequestMethod(_inValidRequest);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithBadGet()
		{
			// Arrange
			bool result = false;
			byte[] buffer = _inValidRequest;

			// Act 
			result = WebServer.ValidateRequestMethod(buffer);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateRequestVersionWithBadHTTPVersion()
		{
			// Arrange
			bool result = false;

			byte[] buffer = _inValidRequestBadVersion;

			// Act 
			result = WebServer.ValidateRequestVersion(buffer);

			// Assert
			Assert.False(result);
		}
		
		[Test ()]
		public void ValidateRequestTests()
		{
			// Arrange
			bool result = true;

			// Case 1
			byte[] buffer = _inValidRequestBadVersion;
			result = WebServer.ValidateRequest(buffer, buffer.Length);
			Assert.IsFalse(result);

			// Case 2
			result = true;
			buffer = _inValidRequest;
			result = WebServer.ValidateRequest(buffer, buffer.Length);
			Assert.IsFalse(result);

			// Case 3
			result = true;
			buffer = _inValidRequestLowerCaseGet;
			result = WebServer.ValidateRequest(buffer, buffer.Length);
			Assert.IsFalse(result);

			// Case 4
			result = false;
			buffer = _validRequest;
			result = WebServer.ValidateRequest(buffer, buffer.Length);
			Assert.IsTrue(result);
		}



	}
}

