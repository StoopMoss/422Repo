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
	public class StdFSDirTests
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
    public void GetFileNameFromFullPathTest()
    {
      string returned = StdFSDir.GetFileNameFromFullPath(_rootPath);

      Assert.AreEqual("FSROOT", returned);
    }

    [Test]
    public void GetFileNameFromFullPathEndingInslashShouldBeSafe()
    {
      string returned = StdFSDir.GetFileNameFromFullPath(_rootPath + "/");

      Assert.AreEqual("FSROOT", returned);
    }

    [Test]
    public void GetName()
    {
      StdFSDir dir = new StdFSDir();
      Assert.AreEqual("", dir.Name);
    }

    [Test]
    public void GetDirs()
    {
      List<Dir422> expected = new List<Dir422>();

      // set up a dir list to compare
      Dir422 dir1 = new StdFSDir(_rootPath + "/Dir1", "Dir1", _root);
      expected.Add(dir1);
      // expected.Add(new StdFSDir("dir2",root));
      // expected.Add(new StdFSDir("dir3",root));
      // expected.Add(new StdFSDir("dir4",root));

      List<Dir422> dirs = (List<Dir422>)_root.GetDirs();

      Assert.NotNull(dirs);
      Assert.AreEqual(expected.Count, dirs.Count);
      Assert.AreEqual(expected[0].Name, dirs[0].Name);

      //This for some reason fails on same types???
      //Assert.AreEqual(expected, dirs);

    }

    [Test]
    public void GetFiles()
    {

      File422 file1 = new StdFSFile(_rootPath + "/file1", "file1", _root);
      List<File422>  expected = new List<File422>();
      expected.Add(file1);
      
      List<File422> files = (List<File422>)_root.GetFiles();

      Assert.NotNull(files);
      Assert.AreEqual(expected.Count, files.Count);      
      //Assert.AreEqual(expected, files);
    }
    //TODO: Get many files

    [Test]
    public void GetFile()
    {
      StdFSFile file = (StdFSFile)_root.GetFile("file1");

       Assert.NotNull(file);
       Assert.AreEqual("file1", file.Name);
       Assert.AreEqual(_rootPath + "/file1", file.Path);
       Assert.AreEqual(_root, file.Parent);
    }

    [Test]
    public void GetFileThatDoesntExist()
    {
      StdFSFile file = (StdFSFile)_root.GetFile("file2");

       Assert.Null(file);
    }

    [Test]
    public void CreateFileWhenFileExists()
    {
      File422 file = _root.CreateFile("file1");

      Assert.NotNull(file);
      Assert.AreEqual("file1", file.Name);
      Assert.AreEqual("FSROOT", file.Parent.Name);
    }
    
    [Test]
    public void CreateFileWhenFileDoesNotExist()
    {
      File422 file = _root.CreateFile("file2");

      Assert.NotNull(file);
      Assert.AreEqual("file2", file.Name);
      Assert.AreEqual("FSROOT", file.Parent.Name);
    }




    [Test]
    public void GetDir()
    {
       StdFSDir dir = (StdFSDir)_root.GetDir("Dir1");

       Assert.NotNull(dir);
       Assert.AreEqual("Dir1", dir.Name);
       Assert.AreEqual(_rootPath + "/Dir1", dir.Path);
       Assert.AreEqual(_root, dir.Parent);
    }
    
    [Test]
    public void GetDirThatDoesntExist()
    {
       StdFSDir dir = (StdFSDir)_root.GetDir("Dir2");

       Assert.Null(dir);
    }

    //TODO: Add checks for bad name...

    [Test]
    public void GetDirWithForwardSlashInNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.GetDir("/bad/name");
      Assert.Null(dir);
    }

    [Test]
    public void GetDirWithBackwardSlashInNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.GetDir("\\bad\\name");
      Assert.Null(dir);
    }

    [Test]
    public void GetDirWithEmptyNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.GetDir("");
      Assert.Null(dir);
    }
        

    [Test]
    public void CreateDirWithForwardSlashInNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.CreateDir("/bad/name");
      Assert.Null(dir);
    }

    [Test]
    public void CreateDirWithBackwardSlashInNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.CreateDir("\\bad\\name");
      Assert.Null(dir);
    }

    [Test]
    public void CreateDirWithEmptyNameShouldReturnNull()
    {
      StdFSDir dir = new StdFSDir();
      dir = (StdFSDir)dir.CreateDir("");
      Assert.Null(dir);
    }

    [Test]
    public void CreateDirWhenDirExists()
    {
      Dir422 dir = _root.CreateDir("Dir1");

      Assert.NotNull(dir);
      Assert.AreEqual("Dir1", dir.Name);
      Assert.AreEqual("FSROOT", dir.Parent.Name);
    }
    
    [Test]
    public void CreateDirWhenDirDoesNotExist()
    {
      Dir422 dir = _root.CreateDir("Dir2");

      Assert.NotNull(dir);
      Assert.AreEqual("Dir2", dir.Name);
      Assert.AreEqual("FSROOT", dir.Parent.Name);
    }

    [Test]
    public void ContainsFileNonRecursiveAndFileExists()
    {
      bool containsFile = _root.ContainsFile("file1", false);
      Assert.IsTrue(containsFile);
    }

    [Test]
    public void ContainsFileNonRecursiveAndFileDoesntExist()
    {
      bool containsFile = _root.ContainsFile("file2", false);
      Assert.IsFalse(containsFile);
    }


  [Test]
    public void ContainsFileRecursiveAndFileExists()
    {
      bool containsFile = _root.ContainsFile("subfile1", true);
      Assert.IsTrue(containsFile);
    }

    [Test]
    public void ContainsFileRecursiveAndFileDoesntExist()
    {
      bool containsFile = _root.ContainsFile("subfile2", true);
      Assert.IsFalse(containsFile);
    }



    [Test]
    public void ContainsDirNonRecursiveForDirectoryThatExists()
    {
      bool containsDir = _root.ContainsDir("Dir1", false);
      Assert.IsTrue(containsDir);
    }
    

    [Test]
    public void ContainsDirNonRecursiveForDirectoryThatDoesntExist()
    {
      bool containsDir = _root.ContainsDir("Dir2", false);
      Assert.IsFalse(containsDir);
    }

    [Test]
    public void ContainsDirRecursivelyForDirectoryThatExists()
    {
      bool containsDir = _root.ContainsDir("SubDir1", true);
      Assert.IsTrue(containsDir);
    }

    [Test]
    public void ContainsDirRecursivelyForDirectoryThatDoesntExist()
    {
      bool containsDir = _root.ContainsDir("SubDir2", true);
      Assert.IsFalse(containsDir);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void ContainsDirWithEmptyNameShouldReturnFalse(bool recursive)
    {
      bool containsDir = _root.ContainsDir("", recursive);
      Assert.IsFalse(containsDir);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void ContainsDirWithForwardSlashInNameShouldReturnFalse(bool recursive)
    {
      bool containsDir = _root.ContainsDir("bad/name", recursive);
      Assert.IsFalse(containsDir);
    }
    
    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void ContainsDirWithBackwardSlashInNameShouldReturnFalse(bool recursive)
    {
      bool containsDir = _root.ContainsDir("bad\\name", recursive);
      Assert.IsFalse(containsDir);
    }




  }
}