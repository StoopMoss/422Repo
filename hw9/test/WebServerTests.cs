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
  public class WebServerTests
	{
    private byte[] _byteArray;
    private NoSeekMemoryStream _stream;
    
    private string _validHttpRequestURI = "GET /files HTTP/1.1\r\n" + 
      "SomeHeader: balh blah blah\r\n" +
      "AnotherHeader: somthing bout the blues\r\n"+
      "\r\n"+"This is the body of an http request";

    private string _inValidHttpRequestURI = "GET not a URI HTTP/1.1\r\n" + 
      "SomeHeader: balh blah blah\r\n" +
      "AnotherHeader: somthing bout the blues\r\n"+
      "\r\n"+"This is the body of an http request";

    private string _inValidRequestURITooManySpaces = "GET nota URI HTTP/1.1\r\n" + 
      "SomeHeader: balh blah blah\r\n" +
      "AnotherHeader: somthing bout the blues\r\n"+
      "\r\n"+"This is the body of an http request";

    private string _inValidRequestURIExtraSpaceAtEnd = "GET URI  HTTP/1.1\r\n" + 
      "SomeHeader: balh blah blah\r\n" +
      "AnotherHeader: somthing bout the blues\r\n"+
      "\r\n"+"This is the body of an http request";

      
    public string _validUnfinishedRequestURI1 = "GET /";
    public string _validUnfinishedRequestURI2 = "GET /file";

    // [Test]
    // [Ignore]
    // public void TestOne()
    // {
    //   bool success = WebServer.Start(8002, 1);
    // }


    ////////////////////////////////////////////////////
    // Validator Tests                                
    ////////////////////////////////////////////////////

    [Test]
    public void ValidateURIWithValidRequest()
    {
      _byteArray = Encoding.ASCII.GetBytes(_validHttpRequestURI);
      //TODO: Validater needs a major refactor because its a mess      
      Validator validator = new Validator(_byteArray);
      
      bool result = validator.ValidateRequestURI(_byteArray);

      Assert.IsTrue(result);
      Assert.AreEqual("/files", validator.Request.URI);
    }

    [Test]
    public void ValidateURIWithValidButUnfinishedRequest()
    {
      _byteArray = Encoding.ASCII.GetBytes("GET /f");
      //TODO: Validater needs a major refactor because its a mess      
      Validator validator = new Validator(_byteArray);
      
      bool result = validator.ValidateRequestURI(_byteArray);

      Assert.IsTrue(result);
    }

    // [Test]
    // public void ValidateURIWithTooManySpacesInRequestLine()
    // {
    //   _byteArray = Encoding.ASCII.GetBytes(_inValidRequestURITooManySpaces);
    //   //TODO: Validater needs a major refactor because its a mess      
    //   Validator validator = new Validator(_byteArray);
      
    //   bool result = validator.ValidateRequestURI(_byteArray);

    //   Assert.IsFalse(result);
    //   Assert.IsNullOrEmpty(validator.Request.URI);
    // }
    
    // [Test]
    // public void ValidateURIWithSpaceAtEndOfRequestLine()
    // {
    //   _byteArray = Encoding.ASCII.GetBytes(_inValidRequestURISpaceAtEnd);
    //   //TODO: Validater needs a major refactor because its a mess      
    //   Validator validator = new Validator(_byteArray);
      
    //   bool result = validator.ValidateRequestURI(_byteArray);

    //   Assert.IsTrue(result);
    //   Assert.NotNull(validator.Request.URI);
    // }


  }
}