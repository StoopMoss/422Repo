using System;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using CS422;

namespace WebServerConsole
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      Console.WriteLine ("initializing filesystem");
      Console.WriteLine ("Root of fs: " + _rootPath);

      Init ();
			 
      Console.WriteLine ("Starting Web Server");
      WebServer.Start (8000, 40, "ROOT");

      dispose ();
      Console.WriteLine ("Disposing filesystem");
    }


    private static string _rootPath = Directory.GetCurrentDirectory () + "/ROOT";

    public static void Init ()
    {
      //Init...
      //_sys = new StandardFileSystem();
      //_sys =  StandardFileSystem.Create("FSROOT");

      //Set up the filesystem
      //Create Files
      FileStream file = File.Create (_rootPath + "/file1");
      String s = "this is a file with content";
      byte[] buffer = Encoding.ASCII.GetBytes (s);
      file.Write (buffer, 0, s.Length);
      file.Close ();
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

    public static void dispose ()
    {
      // TearDown the Test filesystem
      Console.WriteLine ("Removing test files...");
      try
      {
        File.Delete (_rootPath + "/file1");
        File.Delete (_rootPath + "/file2");
        Directory.Delete (_rootPath + "/Dir1", true); // deletes recursively 
      } catch (DirectoryNotFoundException)
      {
        Console.WriteLine ("Caught DirectoryNotFoundException");  
      }
      Console.WriteLine ("TearDown finished");
    }


  }
}
