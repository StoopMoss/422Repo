using NUnit.Framework;
using NUnit;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using CS422;

namespace hw5Tests
{
  [TestFixture ]
  public class DemoServiceTests
	{    
    private const string c_template =
    "<html>This is the response to the request:<br>" +
    "Method: {0}<br>Request-Target/URI: {1}<br>" +
    "Request body size, in bytes: {2}<br><br>" +
    "Student ID: {3}</html>";

    private const string _formattedString =
    "<html>This is the response to the request:<br>" +
    "Method: GET<br>Request-Target/URI: /<br>" +
    "Request body size, in bytes: 0<br><br>" +
    "Student ID: 11282717</html>";

    [Test]
    public void FormatResponseTest()
    {
      DemoService service = new DemoService();
      CS422.WebRequest request = new CS422.WebRequest();
      request.Method = "GET";
      request.URI = "/";
      request.HTTPVersion = "HTTP/1.1";

      TcpListener listener = new TcpListener(IPAddress.Any, 8000);
			listener.Start();
      TcpClient client = new TcpClient();
			client = listener.AcceptTcpClient();

      request.Body = client.GetStream();
      int bodySize = (int)request.Body.Length;
      //NetworkStream networkStream = request.StreamRef;
      
      
      service.Handler(request);
      string result = service.FormatResponseTemplate(c_template);
      
      Assert.NotNull(service);
      Assert.AreEqual(result, _formattedString); 
      
      client.Close();
      //listener.Close();
      listener.Stop();
      //Assert.Fail();   
    }
  }
}