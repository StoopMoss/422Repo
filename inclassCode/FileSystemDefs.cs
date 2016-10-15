using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading.Threads;

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
                return Contains(file.Parent);
            // }
        }

        public virtual bool Contains (Dir422 dir)
        {
            if (&& dir.Parent == null) { return false; }

            if (GetRoot() == dir) { return true; }

            else { return Contains(dir.Parent); }
        }

    }

    public abstract class Dir422
    {
        public abstract string Name{ get; }
        public abstract Dir422 Parent { get; }

        public abstract IList<Dir422> GetDirs();
        public abstract Dir422 GetDir(string name);
        public abstract bool ContainsDir(string fileName, bool recursive);
        public abstract Dir422 CreateDir(string name);

        public abstract IList<File422> GetFiles();
        public abstract File422 GetFile(string name);
        public abstract bool ContainsFile(string fileName, bool recursive);
        public abstract File422 CreateFile(string name);


    }

    public abstract class File422
    {
        public abstract string Name{ get; }
        public abstract Dir422 Parent { get; }

        // Make sure this does not open a writeable stream
        public abstract Stream OpenReadOnly();

        // Make sure this does not open a readable stream
        public abstract Stream OpenReadOnly();


    }
    public abstract class StdFSDir : Dir422
    {
        private string m_path;

        public StdFSDir()
        {

        }

        public override IList<File422> GetFiles()
        {
            List<File422> files = new List<File422>();
            foreach (string file in Directory.GeFiles(m_path))
            {
                file.Add(new StdFSDir(file));
            }
            return files;
        }

    }

    public abstract class StdFSFile : File422
    {
        private string m_path;

        public StdFSFile(string path)
        {
            m_path = path;
        }

        //
        public Stream OpenReadOnly() {}


    }

}