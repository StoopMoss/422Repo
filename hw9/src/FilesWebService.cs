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
  public class FilesWebService : WebService
  { 
    private FileSys422 _fs;
    private CS422.WebRequest _request;
    private StringBuilder _cwd;

    private string _listingFormatString = "<html>"+
      "<h1>Folders</h1> " +
      "<a href=“/files/{0}”>{0}</a> <br>"+
      "<h1>Files</h1>"+
      "<a href=“files/{1}”>{1}</a> <br> </html>";


    public override string ServiceURI
		{
			get {return "/files"; }
		}

    //Constructor
    public FilesWebService(FileSys422 fs)
    { 
      _fs = fs;
      _request = null;
      _cwd = new StringBuilder();
    }

    // Methods
    public override void Handler(WebRequest request)
    {
      Console.WriteLine("Service Handler: OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
      _request = request;

      // Parse the URI and determine which of the 3 is the case:      
      string[] parsedURI = request.URI.Split('/');
      List<string> dirNames = new List<string>();
      int count = 0;
      foreach (string s in parsedURI)
      {
        if(count > 1)// so we only have traversable dirnames
        {
          Console.WriteLine("adding: " + parsedURI[count]);
          dirNames.Add(parsedURI[count]);
        }
        Console.WriteLine("s: " + s);
        Console.WriteLine("c: " + count);
        count++;
      }
      Console.WriteLine("dirNames.Count: " + dirNames.Count);

      Dir422 currentDir = _fs.GetRoot();
      Dir422 lastDir;
      File422 file = null;

      foreach (string s in dirNames)
      {
        if (!String.IsNullOrEmpty(s))
        {
          Console.WriteLine("locating: " + s);
          lastDir = currentDir;
          currentDir =  currentDir.GetDir(s);
          if (null == currentDir) // check to see if a file
          {
            Console.WriteLine("Null Dir");
            file = lastDir.GetFile(s);
            if (null == file)
            {
              Console.WriteLine("Null File");
              //requested Resource does not exist
              // so 404 write to network and return
              request.WriteNotFoundResponse("Could not find file: " + s + ".");
              return;
            }
            // otherwise write file contents as html
            WriteFileContents(file);
            return;
          }
        }
      }

      // If this point is reached then we should have a dir and 
      // we must write its file listing to the network
      WriteDirListing(currentDir);
   
      // Provide support for partial content responses 
      // (i.e. support the Range header)
    }
		

    public void WriteFileContents(File422 file)
    {
      Console.WriteLine("WriteFileContents(): "  );
      // Generate Html
      string html = "";

      // Stream fileStream = file.OpenReadOnly();
      // byte[] buffer = new byte[8000];


      _request.WriteHTMLResponse(html);
    }


    string BuildDirHTML(Dir422 dir)
    {
      _cwd = new StringBuilder();
      // Generate Html
      var html = new StringBuilder("<html> <h1>Folders</h1>");
			
      // Append dirs
      foreach(Dir422 d in dir.GetDirs())
      {
        // Construct cwd href as we go
        string cwd = ConstructCWD(dir);

        Console.WriteLine("cwd: " + cwd.ToString());
        Console.WriteLine("d.name: " +d.Name);
			  html.AppendFormat("<a href=\"{0}/{1}\">{1}</a> <br>", cwd, d.Name);
			}

      html.AppendLine("<h1>Files</h1>");

			foreach(File422 file in dir.GetFiles())
      {
        string cwd = ConstructCWD(file);

        Console.WriteLine("cwd: " + cwd.ToString());
        Console.WriteLine("file.name: " +file.Name);
			  html.AppendFormat("<a href=\"{0}/{1}\">{1}</a> <br>", cwd, file.Name);
			}
			
			html.AppendLine("</html>");
      return html.ToString();
    }

    public void WriteDirListing(Dir422 dir)
    {
      Console.WriteLine("WriteDirListing():XXXXXXXXXXXXXXXXXXXXXXXXXXXX "  );
      string html = BuildDirHTML(dir);
      _request.WriteHTMLResponse(html);
    }

    public string ConstructCWD(Dir422 dir)
    {
      if (null == dir.Parent)
      {
        return "/files";
      }
      
      Console.WriteLine("inserting: " + dir.Name);
      _cwd.Insert(0, dir.Name + "/");

      return ConstructCWD(dir.Parent);
    }

    public string ConstructCWD(File422 file)
    {
      return ConstructCWD(file.Parent);
    }


		
  }
}