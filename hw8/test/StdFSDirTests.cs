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

    [Test]
    public void GetName()
    {
      StdFSDir dir = new StdFSDir();
      Assert.AreEqual("", dir.Name);
    }

    [Test]
    public void GetDirs()
    {
      StdFSDir dir = new StdFSDir();
      StandardFileSystem sys = new StandardFileSystem();
      sys =  StandardFileSystem.Create("FSROOT");
      StdFSDir root = (StdFSDir)sys.GetRoot();
      List<StdFSDir> expected = new List<StdFSDir>();

      // set up a dir list to compare
      StdFSDir dir1 = new StdFSDir("Dir1",root);
      expected.Add(dir1);
      // expected.Add(new StdFSDir("dir2",root));
      // expected.Add(new StdFSDir("dir3",root));
      // expected.Add(new StdFSDir("dir4",root));

      IList<Dir422> dirs = root.GetDirs();

      Assert.AreEqual(expected, dirs);
    }

    [Ignore]
    [Test]
    public void GetFiles()
    {
      StdFSDir dir = new StdFSDir();

      IList<File422> files = dir.GetFiles();

      Assert.AreEqual(null, files);
    }

    [Test]
    public void CreateDir()
    {
       StdFSDir dir = new StdFSDir();

    }

    [Test]
    public void GetDir()
    {
       StdFSDir dir = new StdFSDir();

       dir = (StdFSDir)dir.GetDir("TestDir1");

       //Assert.AreEqual(dir,  );
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
      StdFSDir dir = new StdFSDir();

      dir = (StdFSDir)dir.CreateDir("FSROOT");

      Assert.NotNull(dir);
      Assert.AreEqual("FSROOT", dir.Name);
    }

  }
}