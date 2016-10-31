using NUnit.Framework;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using CS422;

namespace WebServerTests
{
  [TestFixture]
  public class WebServerIntegrationTests
  {
    private StandardFileSystem _sys;
    private string _rootPath = "/home/conner/Documents/422/422Repo/hw9/build/FSROOT";

    [SetUp]
    public void Init ()
    {
      //Init...
      //_sys = new StandardFileSystem();
      //_sys =  StandardFileSystem.Create("FSROOT");

      //Set up the filesystem
      //Create Files
      File.Create (_rootPath + "/file1").Close ();
      File.Create (_rootPath + "/file2").Close ();

      //Create Directory
      Directory.CreateDirectory (_rootPath + "/Dir1");

      //Create subDirs
      Directory.CreateDirectory (_rootPath + "/Dir1/SubDir1");
      Directory.CreateDirectory (_rootPath + "/Dir1/SubDir2");
      Directory.CreateDirectory (_rootPath + "/Dir1/SubDir1/DoubleSubDir1");

      //Create subFiles      
      File.Create (_rootPath + "/Dir1/subfile1").Close ();
      File.Create (_rootPath + "/Dir1/SubDir1/doubleSubFile1").Close ();

      Console.WriteLine ("Setup Complete");
    }

    [TearDown]
    public void dispose ()
    {
      // TearDown the Test filesystem
      Console.WriteLine ("Removing test files...");
      try
      {
        File.Delete (_rootPath + "/file1");
        File.Delete (_rootPath + "/file2");
        Directory.Delete (_rootPath + "/Dir1", true); // deletes recursively 
      } catch (DirectoryNotFoundException e)
      {
        Console.WriteLine ("Caught DirectoryNotFoundException");  
      }
      Console.WriteLine ("TearDown finished");
    }

    //[Ignore]
    [Test]
    public void IntegrationTest ()
    {
      bool success = WebServer.Start (8000, 40, "FSROOT");
      Console.WriteLine ("Webserver Finished: success = " + success);
    }

  }
}