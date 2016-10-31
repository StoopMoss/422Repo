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
	public class StdFSFileTests
	{
    private StdFSDir _root;
    private string _rootPath = "/home/conner/Documents/422/422Repo/hw8/build/FSROOT";

    [SetUp]
    public void Init()
    {
      StandardFileSystem sys = new StandardFileSystem();
      sys =  StandardFileSystem.Create("FSROOT");      
      _root = (StdFSDir)sys.GetRoot();

     File.Create(_rootPath + "/file1").Close();
      Directory.CreateDirectory(_rootPath + "/Dir1");
      Directory.CreateDirectory(_rootPath + "/Dir1" + "/SubDir1");
      File.Create(_rootPath + "/Dir1/subfile1").Close();

      Console.WriteLine("Setup Complete");
      Console.WriteLine("Root: " + _root);
    }

    [TearDown]
    public void Dispose()
    {
      Console.WriteLine("Removing test files...");
      try 
      {
        File.Delete(_rootPath + "/file1");
        File.Delete(_rootPath + "/file2");
        File.Delete(_rootPath + "/Dir1/subfile1");
        Directory.Delete(_rootPath + "/Dir1" + "/SubDir1");
        Directory.Delete(_rootPath +"/Dir1");
        Directory.Delete(_rootPath +"/Dir2");
      }
      catch(DirectoryNotFoundException e)
      {
        Console.WriteLine("Caught DirectoryNotFoundException");  
      }
      Console.WriteLine("TearDown finished");
    }


    [Test]
    public void OpenReadOnly()
    {
      File422 file =  _root.GetFile("file1");
      Assert.NotNull(file);

      Stream stream = file.OpenReadOnly();
      Assert.NotNull(stream);
      Assert.IsFalse(stream.CanWrite);
    }

    [Test]
    public void OpenReadOnlyFailReturnsNull()
    {
      StdFSFile file =  new StdFSFile();
      Stream stream = file.OpenReadOnly();
      Assert.Null(stream);
    }

    [Test]
    public void OpenReadWrite()
    {
      File422 file =  _root.GetFile("file1");
      Assert.NotNull(file);

      Stream stream = file.OpenReadWrite();
      Assert.NotNull(stream);
    }

    [Test]
    public void OpenReadWriteFailReturnsNull()
    {
      StdFSFile file =  new StdFSFile();
      Stream stream = file.OpenReadWrite();
      Assert.Null(stream);
    }



  }
}