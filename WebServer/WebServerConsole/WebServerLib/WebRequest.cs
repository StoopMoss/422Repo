using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CS422
{
  public class WebRequest
  {
    private string _method;
    private string _uri;
    private string _httpVersion;

    private int _statusCode;
    private Dictionary<string, string> _headers;
    //private string[] _headersArray;
    private Stream _body;
    private string _htmlBody;
    private NetworkStream _networkStream;
		
    private int _studentId = 11282717;

    private string _responseTemplate = "{0} {1} {2}\r\n" +
                                       "Content-Type: text/html\r\n" +
                                       "Content-Length: {3}\r\n" +
                                       "\r\n\r\n" +
                                       "{4}";

    //////////////
    //Properties
    public string Method {
      get{ return _method; }
      set{ _method = value; }
    }

    public string URI {
      get{ return _uri; }
      set{ _uri = value; }
    }

    public int StatusCode {
      get{ return _statusCode; }
      set{ _statusCode = value; }
    }

    public string HTTPVersion {
      get{ return _httpVersion; }
      set{ _httpVersion = value; }
    }

    public Dictionary<string, string> Headers {
      get{ return _headers; }
      set{ _headers = value; }
    }

    //    public string[] HeadersArray {
    //      get{ return _headersArray; }
    //      set{ _headersArray = value; }
    //    }

    public Stream Body {
      get{ return _body; }
      set{ _body = value; }
    }

    public NetworkStream StreamRef {
      get{ return _networkStream; }
      set{ _networkStream = value; }
    }

    public void AddHeader (string header, string value)
    {
      
      _headers.Add (header, value);
    }
    

    //Constructor
    public WebRequest ()
    {
      // initialize members 
      _method = "";
      _uri = "";
      _httpVersion = "";
      //_headersArray = new string[1] { "" };
      _body = new MemoryStream ();
      _htmlBody = "";
      _headers = new Dictionary<string, string> ();
      //_networkStream = new NetworkStream();
    }

    ////////////
    // Methods 
    //
    public void WriteNotFoundResponse (string html)
    {
      // put the html thats passed in,
      // into the body of the http response
      _htmlBody = html;
      _statusCode = 404;

      // generate HTTP response 
      string HttpResponse = GetHtmlResponseHeader ();

      //Write to stream
      WriteToNetworkStream (HttpResponse);
    }

    public bool WriteHTMLResponse (string html)
    {
      // put the html thats passed in,
      // into the body of the http response
      _htmlBody = html;
      _statusCode = 200;

      // generate HTTP response 
      string HtmlResponse = GetHtmlResponseHeader ();
      StringBuilder b = new StringBuilder ();
      b.Append (HtmlResponse);
      b.Append ("\r\n\r\n");
      b.Append (html);

      //Write to stream
      bool successfulWrite = WriteToNetworkStream (b.ToString ()); 
      return successfulWrite;
    }


    ///////////////////////////
    /// Helper Methods ////////
    ///////////////////////////
    // public string FormatResponseTemplate(string template)
    // {
    // 	string response = string.Format(template, _studentId, DateTime.Now);

    //   throw new NotImplementedException();
    //   //return response;
    // }

    public bool WriteToNetworkStream (string HttpResponse)
    {

      Console.WriteLine ("WriteToNetworkStream():");

      try
      {
        if (_networkStream.CanWrite)
        {
          Console.WriteLine ("int if");
          byte[] buffer = Encoding.ASCII.GetBytes (HttpResponse);
          _networkStream.Write (buffer, 0, buffer.Length);
          return true;
        }  
      } catch (NullReferenceException)
      {        
        throw new NullReferenceException ("WriteToNetworkStream: _networkStream was null.");
      } catch (Exception)
      {        
        throw new Exception ("Network stream was unwriteable");
      }
      return false;		      
						
    }

    // Creates a valid http response based off the status code given and
    // the member template
    public string GetHtmlResponseHeader ()
    {      
      StringBuilder response = new StringBuilder ();
      response.Append (GetStatusLineAsString ());
      response.Append (GetHeadersAsString ());
      //response.Append (_htmlBody);

      return response.ToString ();
    }

    public string GetStatusLineAsString ()
    {
      //TODO: change hard coded status phrase to some sort of map
      if (_statusCode == 200)
      {
        return string.Format ("{0} {1} {2}\r\n", _httpVersion, _statusCode, "OK");
      }
      if (_statusCode == 404)
      {
        return string.Format ("{0} {1} {2}\r\n", _httpVersion, _statusCode, "NotFound");
      }
      return "Error! BadStatusCode";
    }

    public string GetHeadersAsString ()
    {      
      //AddHeader ("Content-Length", _htmlBody.Length.ToString ());
      StringBuilder b = new StringBuilder ();

      Dictionary<string, string>.KeyCollection keys = _headers.Keys;
      foreach (string header in keys)
      {
        string s = string.Format ("{0}: {1} \r\n", header, _headers [header]); 
        b.Append (s);
      }
      //b.Append ("\r\n\r\n");
      return b.ToString ();
    }

    public void PrintRequest ()
    {
      Console.WriteLine ("method: " + _method);
      Console.WriteLine ("URI: " + _uri);
      Console.WriteLine ("version: " + _httpVersion);

    }

  }
}

