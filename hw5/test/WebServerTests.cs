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

    [Test]
    [Ignore]
    public void TestOne()
    {
      bool success = WebServer.Start(8081, 10);
    }
  }
}