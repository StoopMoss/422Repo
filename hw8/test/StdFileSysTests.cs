using NUnit.Framework;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using CS422;

namespace hw8Tests
{
	[TestFixture ]
	public class FileSystemTests
	{
    [Test]
    public void StartFileSystemTest()
    {
      StandardFileSystem sys = new StandardFileSystem();
      sys =  StandardFileSystem.Create("FSROOT");

      Assert.NotNull(sys);
      Assert.AreEqual("FSROOT", sys.GetRoot().Name);
    }
  }
}