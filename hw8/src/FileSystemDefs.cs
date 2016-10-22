using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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

      //TODO: change this....
      public Dir422 Root
      {
        set{_root = value;}
      }

      public override Dir422 GetRoot()
      {
        return _root;
      }

      //constructors
      public StandardFileSystem()
      {     }

      public StandardFileSystem(StdFSDir root)
      {
        _root = root;
      }

      // Methods
      public static StandardFileSystem Create(string rootDir)
      {
        // create rootDir
        DirectoryInfo dir = Directory.CreateDirectory(rootDir);
        string cwd = Directory.GetCurrentDirectory();
        StdFSDir root = new StdFSDir(cwd +"/"+ rootDir, rootDir, null);

        StandardFileSystem fileSystem = new StandardFileSystem(root);

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
        public abstract Dir422 Parent { get; }

        public abstract IList<Dir422> GetDirs();
        public abstract IList<File422> GetFiles();

        public abstract Dir422 GetDir(string name);
        public abstract File422 GetFile(string name);

        public abstract bool ContainsDir(string fileName, bool recursive);
        // public abstract bool ContainsFile(string fileName, bool recursive);

        public abstract Dir422 CreateDir(string name);
        public abstract File422 CreateFile(string name);
    }

    public class StdFSDir : Dir422
    {
      private string _path;
      private string _name;
      private StdFSDir _parent;
      private List<Dir422> _dirs;
      private List<File422> _files;

      // Properties for testing
      public string Path { get{return _path;}}

      // Regular Props
      public override string Name{ get {return _name;}   }
      public override Dir422 Parent { get {return _parent;} }

      public StdFSDir()
      {
        _name = "";
        _dirs = new List<Dir422>();
        _path = "";
      }

      public StdFSDir(string fullPath, string name, StdFSDir parent)
      {
        _name = name;        
        _dirs =  new List<Dir422>();
        _parent = parent;
        _path = fullPath;
      }

      public override IList<File422> GetFiles()
      {        
        List<File422> files = new List<File422>();
        List<string> fullFileNames = Directory.EnumerateFiles(_path).ToList<string>();

        Console.WriteLine("fullFileNames.Count " + fullFileNames.Count);

        int i = 0;
        foreach (string file in fullFileNames)
        {
          Console.WriteLine("in loop   SSSSSSSSSSSSSSSSSSSs!");
          string fileName = GetFileNameFromFullPath(file);
          if (_files.Contains(GetFile(fileName)) == null)
          {  /* do nothing */  
            Console.WriteLine("Doing nothing!!!!!!");
          }
          else
          {
            //Add file to list
            Console.WriteLine("Adding " + fileName);
            File422 newf = GetFile(fileName);
            //Console.WriteLine("GetDir returned : " + newd);
            files.Add(newf); 
            //dirList.Add(GetDir(dir));
            i++;
          }          
        }

        i--;
        //Console.WriteLine("added " + i + " dirs to list");
        //Console.WriteLine("dirList.Count = "+ dirList.Count);

        _files = files;
        return files;
      }

      public override IList<Dir422> GetDirs()
      {                        
        Console.WriteLine("GetDirs():");
        List<string> dirNames = Directory.EnumerateDirectories(_path).ToList<string>();
        List<Dir422> dirList = new List<Dir422>();

        int i = 0;
        foreach (string dir in dirNames)
        {
          string fileName = GetFileNameFromFullPath(dir);
          if (_dirs.Contains(GetDir(dir)))
          { /* do nothing */ }
          else
          {
            //Add dir to list
            //Console.WriteLine("Adding " + fileName);
            Dir422 newd = GetDir(fileName);
            //Console.WriteLine("GetDir returned : " + newd);
            dirList.Add(newd); 
            //dirList.Add(GetDir(dir));
            i++;
          }          
        }

        i--;
        //Console.WriteLine("added " + i + " dirs to list");
        //Console.WriteLine("dirList.Count = "+ dirList.Count);

        _dirs = dirList;
        return dirList;
      }

      public override Dir422 CreateDir(string name)
      {
        StdFSDir newDir = new StdFSDir();

        // validate the name
        if (IsValidName(name))
        {
          // Create dir in actual fileSystem if not there
          Console.WriteLine("path: " + _path);
          Console.WriteLine("cwd = " + Directory.GetCurrentDirectory());
          Directory.CreateDirectory(_path + "/" + name);
          // Create a new stdDir ref
          newDir = new StdFSDir(_path + "/" + name, name, this);
          // Add that to the list of sub dirs for this dir
          _dirs.Add(newDir);
          return newDir;  
        }
        
        Console.WriteLine("CreateDir(): Invalid Name");
        return null;
      }

      public override File422 CreateFile(string name)
      {
        StdFSFile newFile = new StdFSFile();

        // validate the name
        if (IsValidName(name))
        {
          // Create file in actual fileSystem 
          Console.WriteLine("path: " + _path);
          Console.WriteLine("cwd = " + Directory.GetCurrentDirectory());
          File.Create(_path + "/" + name).Close();
          // Create a new stdFile ref
          newFile = new StdFSFile(_path + "/" + name, name, this);
          // Add that to the list of sub dirs for this dir
          _files.Add(newFile);
          return newFile;  
        }
        
        Console.WriteLine("CreateDir(): Invalid Name");
        return null;
      }

      public override Dir422 GetDir(string name)
      {
        Console.WriteLine("GetDir()");
        // Console.WriteLine("cwd: " + _path);
        // Console.WriteLine("name: " + name);
        string dirPath = _path + "/"+ name;
        //Console.WriteLine("Looking for " + dirPath);

        if (IsValidName(name))
        {
          if (Directory.Exists(dirPath))
          {
            // Return dir
            StdFSDir dir = new StdFSDir(dirPath, name, this);
            return dir;
          }
          Console.WriteLine("could not find dir with name: " + name);
          Console.WriteLine("Searched for: " + dirPath);
        }
        return null;               
      }

      public override File422 GetFile(string name)
      {
        Console.WriteLine("GetFile()");
         Console.WriteLine("cwd: " + _path);
         Console.WriteLine("name: " + name);
        string filePath = _path + "/"+ name;
        Console.WriteLine("Looking for " + filePath);

        if (IsValidName(name))
        {
          if (File.Exists(filePath))
          {
            // Return file
            StdFSFile file = new StdFSFile(filePath, name, this);
            return file;
          }
          Console.WriteLine("could not find file with name: " + name);
          Console.WriteLine("Searched for: " + filePath);
        }
        return null;
      }

      public override bool ContainsDir(string fileName, bool recursive)
      {
        if (!IsValidName(fileName))
        { return false; }

        //Prepend the full path for accurate comparison
        string fullFileName = _path + "/" + fileName;

        // See if it exsists
        Console.WriteLine("Looking for " + fullFileName);
        List<string> dirNames = Directory.EnumerateDirectories(_path).ToList<string>();
        Console.WriteLine("dirNames.Count " + dirNames.Count);

        int i = 0;
        foreach (string dirName in dirNames)
        {
          Console.WriteLine("fullFileName " + fullFileName);
          Console.WriteLine("dirName " + dirName);

          if(fullFileName == dirName)
          {
            return true;
          }
        }

        // check if recursive
        if (recursive) 
        {
          // Already Checked imidieat dirs and no match
          // so recurse into each
          // To recurse into dirs we must have thier reference
          //_dirs = this.GetDirs();
          GetDirs();
          Console.WriteLine("YOOOOOO");
          foreach (Dir422 dir in _dirs)
          {
            if (dir.ContainsDir(fileName, true))
            {
              // If contains ever returns true from a recursive Search
              // Then dir was found so return true
              return true;
            }
          }
        }
        // Never found dir so return false
        return false;
      }

      public bool IsValidName(string name)
      {
         if (name.Contains("/") || name.Contains("\\") || String.IsNullOrEmpty(name))
         {
           return false;
         }
         return true;
      }

      public static string GetFileNameFromFullPath(string path)
      {
        char[] delims = {'/'};

        Console.WriteLine("Original text: '{0}'", path);

        string[] parsedArray = path.Split(delims);
        //Console.WriteLine("{0} words in text:", parsedArray.Length);

        foreach (string s in parsedArray)
        {
            //Console.WriteLine(s);
        }

        string ret = parsedArray[parsedArray.Length - 1];
        //Console.WriteLine("ret = " + ret);
        if(string.IsNullOrEmpty(ret))
        {
          ret =  parsedArray[parsedArray.Length - 2];
        }
        Console.WriteLine("ret = " + ret);
        return ret;
      }

    }


    public abstract class File422
    {
        public abstract string Name{ get; }
        public abstract Dir422 Parent { get; }

        // Make sure this does not open a writeable stream
        // public abstract Stream OpenReadOnly();

        // // Make sure this does not open a readable stream
        // public abstract Stream OpenWriteOnly();
    }


    public class StdFSFile : File422
    {
      private string _path;
      private string _name;
      private Dir422 _parent;
      
      // Properties for testing
      public string Path { get{return _path;}}

      //Reg props
      public override string Name{ get {return _name;}   }
      public override Dir422 Parent{ get {return _parent;}   }

      // Constructors
      public StdFSFile()  {      }

      public StdFSFile(string path)
      {
        _path = path;
      }

      public StdFSFile(string fullPath, string name, StdFSDir parent)
      {
        _name = name;        
        _parent = parent;
        _path = fullPath;
      }


      //public override Stream OpenReadOnly() {}
      //public override Stream OpenWriteOnly() {}
    }

}