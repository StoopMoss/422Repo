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
		private byte[] _validRequestBuffer;
		private byte[] _validRequestWithBodyBuffer;
		private byte[] _validRequestNoHeaderBuffer;
		private byte[] _inValidRequestBuffer;
		private byte[] _inValidRequestBadVersionBuffer;
		private byte[] _inValidRequestLowerCaseGetBuffer;
		private byte[] _inValidRequestHeaderTwoColonsBuffer;
		private byte[] _inValidRequestStartingColonBuffer;
		private byte[] _inValidRequestBadLineEndingsBuffer;
		private byte[] _inValidRequestBufferHeaderWithNonLetterAtStart;
		private byte[] _inValidRequestBufferHeaderWithNoColons;

		private TcpClient _client;
		private TcpListener _listener;

		private string _validString = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			"Host: www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _headerWithNonLetterAtStart = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			": www.tutorialspoint.com\r\n" +
			"Accept-Language: en-us\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"Connection: Keep-Alive";

		private string _headerWithNoColons = "GET /hello.htm HTTP/1.1\r\n" +
			"User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\r\n" +
			": www.tutorialspoint.com\r\n" +
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
			_client = new TcpClient();
			_listener = new TcpListener(IPAddress.Any, 4220);
			_listener.Start();

			_validRequestBuffer = Encoding.ASCII.GetBytes(_validString);
			_validRequestWithBodyBuffer = Encoding.ASCII.GetBytes(_RequestWithBodyString);
			_validRequestNoHeaderBuffer= Encoding.ASCII.GetBytes(_NoHeaderString);
			_inValidRequestBuffer = Encoding.ASCII.GetBytes(_badGetString);
			_inValidRequestBadVersionBuffer = Encoding.ASCII.GetBytes(_badVersionString);
			_inValidRequestLowerCaseGetBuffer = Encoding.ASCII.GetBytes(_lowerCaseGetString);
			_inValidRequestHeaderTwoColonsBuffer = Encoding.ASCII.GetBytes (_BadHeaderTwoColonsString);
			_inValidRequestStartingColonBuffer = Encoding.ASCII.GetBytes (_BadHeaderStartingColonString);
			_inValidRequestBadLineEndingsBuffer = Encoding.ASCII.GetBytes (_badLineEndsString);
			_inValidRequestBufferHeaderWithNonLetterAtStart = Encoding.ASCII.GetBytes(_headerWithNonLetterAtStart);
			_inValidRequestBufferHeaderWithNoColons = Encoding.ASCII.GetBytes(_headerWithNoColons);
		}

		[TearDown]
		public void Dispose()
		{
			_client.Close ();
			_listener.Stop ();
		}

		[Test()]
		public void ValidateRequestHeadersWithValidBuffer()
		{
			bool result = WebServer.ValidateRequestHeaders(_validRequestBuffer);
			Assert.IsTrue(result);

		}

		[Test()]
		public void ValidateRequestHeadersWithInValidBuffer()
		{
			bool result = WebServer.ValidateRequestHeaders(_inValidRequestBufferHeaderWithNonLetterAtStart);
			Assert.IsFalse(result);
		}

		[Test()]
		public void ValidateRequestHeadersWithNoColonsInHeader()
		{
			bool result = WebServer.ValidateRequestHeaders(_inValidRequestBufferHeaderWithNoColons);
			Assert.IsFalse(result);
		}


		[Test ()]
		public void ReadFromStreamThatHasValidRequest()
		{			
			bool result = false;
			Stream networkStream = new MemoryStream (_validRequestBuffer);

			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsTrue (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasValidRequestAndABody()
		{				
			bool result = false;
			Stream networkStream = new MemoryStream(_validRequestWithBodyBuffer);

			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsTrue(result);
		}

		[Test ()]
		public void ReadFromStreamThatHasNoHeader()
		{				
			//TODO: 
			bool result = false;
			Stream networkStream = new MemoryStream(_validRequestNoHeaderBuffer);

			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsTrue(result);
		}

		[Test ()]
		public void ReadFromStreamThatHasInvalidRequest()
		{				
			bool result = true;
			Stream networkStream = new MemoryStream (_inValidRequestBuffer);
			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasBadVersion()
		{	
			bool result = true;
			Stream networkStream = new MemoryStream (_inValidRequestBadVersionBuffer);
			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasLowercaseGetMethod()
		{				
			bool result = true;
			Stream networkStream = new MemoryStream (_inValidRequestLowerCaseGetBuffer);
			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasTwoColonsInAHeader()
		{
			bool result = true;
			Stream networkStream = new MemoryStream (_inValidRequestHeaderTwoColonsBuffer);
			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasAStartingColonInTheHeader()
		{
			bool result = true;
			Stream networkStream = new MemoryStream (_inValidRequestStartingColonBuffer);
			result = WebServer.ReadFromNetworkStream (networkStream);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ReadFromStreamThatHasBadNewlines()
		{
			
			bool result = true;
			Stream networkStream = new MemoryStream(_inValidRequestBadLineEndingsBuffer);
			result = WebServer.ReadFromNetworkStream(networkStream);
			Assert.IsFalse(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithValidString()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequestMethod(_validRequestBuffer);
				
			// Assert
			Assert.True(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithInvalidRequest()
		{
			// Arrange
			bool result = false;

			// Act 
			result = WebServer.ValidateRequestMethod(_inValidRequestBuffer);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateRequestMethodWithBadGet()
		{
			// Arrange
			bool result = true;

			// Act 
			result = WebServer.ValidateRequestMethod(_inValidRequestBuffer);

			// Assert
			Assert.False(result);
		}

		[Test ()]
		public void ValidateVersionWithBadHTTPVersion()
		{
			// Arrange
			bool result = true;
			// Act 
			result = WebServer.ValidateRequestVersion(_inValidRequestBadVersionBuffer);
			// Assert
			Assert.False(result);
		}
		
		[Test ()]
		public void ValidateRequestBadVersion()
		{
			// Arrange
			bool result = true;
			result = WebServer.ValidateRequest (_inValidRequestBadVersionBuffer);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ValidateRequestInvalidRequest()
		{				
			bool result = true;
			result = WebServer.ValidateRequest (_inValidRequestBuffer);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ValidateRequestLowercaseGet()
		{			
			bool result = true;
			result = WebServer.ValidateRequest (_inValidRequestLowerCaseGetBuffer);
			Assert.IsFalse (result);
		}

		[Test ()]
		public void ValidateRequestTwoHeaderColons()
		{			
			bool result = true;
			result = WebServer.ValidateRequest(_inValidRequestHeaderTwoColonsBuffer);
			Assert.IsFalse(result);
		}

		[Test ()]
		public void ValidateRequestStartingHeaderColon()
		{			
			bool result = true;
			result = WebServer.ValidateRequest(_inValidRequestStartingColonBuffer);
			Assert.IsFalse(result);
		}

		[Test ()]
		public void ValidateRequestBadLineEndings()
		{			
			bool result = true;
			result = WebServer.ValidateRequest(_inValidRequestBadLineEndingsBuffer);
			Assert.IsFalse(result);
		}


		[Test ()]
		public void ValidateRequestValidRequest()
		{			
			bool result = false;
			result = WebServer.ValidateRequest(_validRequestBuffer);
			Assert.IsTrue(result);
		}



	}
}

