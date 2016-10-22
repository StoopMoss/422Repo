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

      Directory.CreateDirectory(_rootPath + "/Dir1");
      Directory.CreateDirectory(_rootPath + "/Dir1" + "/SubDir1");

      Console.WriteLine("Setup Complete");
      Console.WriteLine("Root: " + _root);
    }

    [TearDown]
    public void Dispose()
    {
      Console.WriteLine("Removing test files...");
      try 
      {
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



  }
}