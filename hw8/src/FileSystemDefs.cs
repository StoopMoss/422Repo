using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace CS422
{
    public abstract class FileSys422
    {
        public abstract Dir422 GetRoot();

        public virtual bool Contains (File422 file)
        {
            // Dir422 parent = file.Parent;
            // while (parent != null)
            // {
                //return Contains(file.Parent);
            // }
            return false;
        }

        public virtual bool Contains (Dir422 dir)
        {
            //if (dir.Parent == null) { return false; }

            //if (GetRoot() == dir) { return true; }

            //else { return Contains(dir.Parent); }
            return false;
        }

    }
    public class StandardFileSystem: FileSys422
    {
      private Dir422 _root;
      
      public override Dir422 GetRoot()
      {
        return _root;
      }

      //constructor
      public StandardFileSystem()
      {
        Console.WriteLine(Directory.GetCurrentDirectory());
      }

      public static StandardFileSystem Create(string rootDir)
      { 
        StandardFileSystem fileSystem = new StandardFileSystem();
        
        // // create rootDir
        // DirectoryInfo dir = Directory.CreateDirectory(rootDir);
        // _root = new StdFSDir();
        // _root = _root.InitDir(dir.name);

        // // loop through each dir and file calling create on each 
        // // creating the filesystem
        // IList<Dir422> dirs = _root.GetDirs();
        // for (int i = 0; i < dirs.Length; i ++)
        // {
        //   dir = dir.CreateDir();
        // }

        
        return fileSystem;        
      }


      // public virtual bool Contains (File422 file)
      //{
      //   // Dir422 parent = file.Parent;
      //   // while (parent != null)
      //   // {
      //       //return Contains(file.Parent);
      //   // }
      //   return false;
      // }

      //   public virtual bool Contains (Dir422 dir)
      //   {
      //       //if (dir.Parent == null) { return false; }

      //       //if (GetRoot() == dir) { return true; }

      //       //else { return Contains(dir.Parent); }
      //       return false;
      //   }

    }





    public abstract class Dir422
    {
        public abstract string Name{ get; }
        //public abstract Dir422 Parent { get; }

        public abstract IList<Dir422> GetDirs();
        public abstract IList<File422> GetFiles();

        //public abstract Dir422 GetDir(string name);
        // public abstract File422 GetFile(string name);

        // public abstract bool ContainsDir(string fileName, bool recursive);
        // public abstract bool ContainsFile(string fileName, bool recursive);
        
        public abstract Dir422 CreateDir(string name);
        // public abstract File422 CreateFile(string name);
    }

    public class StdFSDir : Dir422
    {
      private string _path;
      private string _name;
      private StdFSDir _parent;
      private IList<Dir422> _dirs;
      private IList<File422> _files;

      public override string Name{ get {return _name;}   }

      public StdFSDir()
      {
        _name = "";
        _dirs = null;
      }

      public StdFSDir(string name)
      {
        _name = name;
        _dirs = null;
      }

      public override IList<File422> GetFiles()
      {
           List<File422> files = new List<File422>();
      //   foreach (string file in Directory.GetFiles(_path))
      //   {
      //     files.Add(new StdFSDir(file));
      //   }
         return files;
      }

      public override IList<Dir422> GetDirs()
      {
        return _dirs;
      }

      public override Dir422 CreateDir(string name)
      {        
        StdFSDir newDir = new StdFSDir();

        // validate the name
        if (name.Contains("/") || name.Contains("\\") || String.IsNullOrEmpty(name))
        {
          Console.WriteLine("in if");
          return null;
        }
        
        Directory.CreateDirectory(name);
        //_name = name;
        return newDir;
      }

      public void InitDir(string name, StdFSDir parent)
      {
        _parent = parent;
        _name = name;
      }

    }


    public abstract class File422
    {
        public abstract string Name{ get; }
        //public abstract Dir422 Parent { get; }

        // Make sure this does not open a writeable stream
        // public abstract Stream OpenReadOnly();

        // // Make sure this does not open a readable stream
        // public abstract Stream OpenWriteOnly();
    }
    

    public class StdFSFile : File422
    {
      private string _path;
      private string _name;

      public override string Name{ get {return _name;}   }

      public StdFSFile(string path)
      {
        _path = path;
      }  
      //public override Stream OpenReadOnly() {}
      //public override Stream OpenWriteOnly() {}
    }

}