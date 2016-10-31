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
    private StandardFileSystem _sys;
    private string _rootPath = "/home/conner/Documents/422/422Repo/hw8/build/FSROOT";
    
    [SetUp]
    public void Inti()
    {
      _sys = new StandardFileSystem();
      _sys =  StandardFileSystem.Create("FSROOT");
    }

    [Test]
    public void StartFileSystemTest()
    {
      Assert.NotNull(_sys);
      Assert.AreEqual("FSROOT", _sys.GetRoot().Name);
    }

    [Test]
    public void ContainsFile()
    {
      //StdFSFile file = new StdFSFile(_rootPath+"/file1", "file1", _sys.GetRoot());
      //_sys.Contains();
    }
    
    [Test]
    public void ContainsDir()
    {
      
    }
    
  }
}