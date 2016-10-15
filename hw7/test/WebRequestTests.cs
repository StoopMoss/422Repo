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
  public class WebRequestTests
	{
    private string _responseTemplate;    
    private CS422.WebRequest _request;  

    [SetUp]
    public void initialization()
    {
      _request = new CS422.WebRequest();
      _request.Method = "GET";
      _request.URI = "/";
      _request.HTTPVersion = "HTTP/1.1";
      _request.Body = new MemoryStream();
      int bodySize = (int)_request.Body.Length;
      
      _responseTemplate = "{0} {1} {2}\r\n"+
      "Content-Type: text/html\r\n"+
      "Content-Length: {3}\r\n"+
      "\r\n\r\n"+
      "{4}";
      
    }

    [Test]
    public void GetResponse200()
    {
      _responseTemplate = string.Format(_responseTemplate,
         "HTTP/1.1", 200, "OK", _request.Body.Length, _request.Body);

      string result = _request.GetHtmlResponse(200);

      Assert.NotNull(_request);
      Assert.AreEqual(_responseTemplate, result);
    }
    
    [Test]
    public void GetResponse404()
    {
      _responseTemplate = string.Format(_responseTemplate,
         "HTTP/1.1", 404, "NotFound", _request.Body.Length, _request.Body);
         
      string result = _request.GetHtmlResponse(404);

      Assert.NotNull(_request);
      Assert.AreEqual(_responseTemplate, result);
    }



  }
}