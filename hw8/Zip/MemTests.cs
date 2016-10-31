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
	public class MemFileSystemTests
	{
    private  MemoryFileSystem _sys;
    private  Dir422 _root;
    
    [SetUp]
    public void Inti()
    {
      _sys = new MemoryFileSystem();
      _root = _sys.GetRoot();
    }

    [Test]
    public void StartFileSystemTest()
    {
      Assert.NotNull(_sys);
      Assert.AreEqual("root", _sys.GetRoot().Name);
    }

    [Test]
    public void CreateADir()
    {
      Assert.NotNull(_sys);
      Assert.AreEqual("root", _sys.GetRoot().Name);

      MemFSDir dir = new MemFSDir("dir", _root);

      Assert.AreEqual(dir.Parent, _root);
      Assert.AreEqual("dir", dir.Name);
      
    }

  }
}
