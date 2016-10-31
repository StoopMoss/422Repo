using System;
using System.Collections.Generic;

namespace CS422
{
  public class MimeMapper
  {
    private Dictionary<string,string> _map;

    public MimeMapper ()
    {
      _map = new Dictionary<string, string> ();

      _map.Add ("txt", "text/plain");
      _map.Add ("html", "text/html");
      _map.Add ("htm", "text/html");
      _map.Add ("htmls", "text/html");
      _map.Add ("xml", "text/xml");
      _map.Add ("jpeg", "image/jpeg");
      _map.Add ("png", "image/png");
      _map.Add ("pdf", "application/pdf");
    }

    public string GetMimeType (string filename)
    {      
      char[] delims = { '.' };
      string[] extenstion = filename.Split (delims);
      try
      {
        return _map [extenstion [1]];
      } catch (IndexOutOfRangeException)
      {
        return null;
      }
    }
  }
}

