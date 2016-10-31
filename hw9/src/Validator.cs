using System;
using System.Text;

namespace CS422
{
	public class Validator
	{
    private WebRequest _webRequest;
		private byte[] _buffer;
		private string _URL;

		//Private members used for validation
		private bool _validRequestLine;
		private bool _validRequestHeaders;

    private int _requestSizeThreshold1 = 2048;
    private int _requestSizeThreshold2 = 100	*	1024;

		private bool _fullMethod;
		private bool _fullVersion;
		private bool _fullURI;
		private bool _fullHeaders;
		private bool _fullRequest;

		////////////////
		// Properties
		////////////////
		public string URL
		{
			get{return _URL;}
			set{_URL = value;}
		}
		public byte[] Buffer
		{
			get{return _buffer;}
			set{_buffer = value;}
		}
		public bool FullRequest
		{
			get{return _fullRequest;}
			//set{_fullRequest = value;}
		}
    public WebRequest Request
    {
      get{return _webRequest;}      
    }

		/////////////
		//Methods
		////////////
		public Validator()
		{			
			SetMembers();			
		}

		public Validator(byte[] buffer)
		{
			Buffer = buffer;
			SetMembers();			
		}

		public void SetMembers()
		{
      _webRequest = new WebRequest();
			_URL = "";
			_validRequestLine = false;
			_validRequestHeaders = false;

			_fullMethod = false;
			_fullVersion = false;
			_fullURI = false;
			_fullHeaders = false;
			_fullRequest = false;
		}

		public bool ValidateRequest(byte[] buffer)
		{	
			if (!_fullMethod)
			{
				// Check method for "GET "
				Console.WriteLine("Validate: Check Method");
				if (!ValidateRequestMethod(buffer))
				{ return false; }
			}
			if (!_fullURI && _fullMethod)
			{
				//check uri
				Console.WriteLine("Validate: Check URI");
				if (!ValidateRequestURI (buffer)) 
				{ return false;	}
			}
			if (!_fullVersion && _fullURI && _fullMethod)
			{
				// Check  for “HTTP/1.1”
				Console.WriteLine("Validate: Check Version");
				if(!ValidateRequestVersion(buffer) )
				{ return false; }
					if(_fullVersion) 
				{
					// Entire first line is full and valid
					_validRequestLine = true;
				}
			}
			if (!_fullHeaders && _fullVersion && _fullURI && _fullMethod)
			{
				Console.WriteLine("Validate: Check Headers");
				if( !ValidateRequestHeaders(buffer) )
				{
					Console.WriteLine("--------------------------------");
					return false; 
				}											
			}
				// Console.WriteLine("_fullMethod "+ 
				// _fullMethod.ToString()+"_fullURI "+ 
				// _fullURI.ToString()+"_fullVersion"+
				// _fullVersion.ToString()+"  _fullHeaders)"+
				// _fullHeaders.ToString());
				// //TODO: do i need full headers?
			if (_fullMethod && _fullURI && _fullVersion && _fullHeaders)
			{
				_fullRequest = true;
			}
				return true;
		}

		//accounts for space  "GET "
		public bool ValidateRequestMethod (byte[] buffer)
		{			
			string requestSoFar = Encoding.ASCII.GetString(buffer);
			string expectedString = "GET ";
			int bytesToCheck = 4;

			Console.WriteLine("Validating request Method");
			if (requestSoFar.Length == 0)
			{
				//error...
				return false;
			}
			else if (requestSoFar.Length >= 4) 
			{				
        // check size limit
        if (requestSoFar.Length > 2048 && requestSoFar.Substring(0, _requestSizeThreshold1).Contains("\r\n") != true)
        {
          return false;
        }

				_fullMethod = true;
        //Console.WriteLine("before");
        _webRequest.Method = requestSoFar.Substring(0, 4);
        //Console.WriteLine("after");
			}
			else 
			{				
				bytesToCheck = requestSoFar.Length;
			}
			for (int i = 0; i < bytesToCheck; i++)
			{
				if (requestSoFar[i] != expectedString[i])
				{
					return false;
				}
			}
			return true;				
		}

		public bool ValidateRequestURI(byte[] buffer)
		{
			// account for space "URI/ "
			// assume everything before is correct (Request method and version)
			// Set _URL to be used later
			Console.WriteLine("Validating request URI");
			string requestSoFar = Encoding.ASCII.GetString(buffer);
      Console.WriteLine("requestSoFar "+ requestSoFar);

      // parse on newlines to get just the first line of the request
      char delimiter = '\r';
      string[] parsedLine = requestSoFar.Split(delimiter);

      // debug
      // foreach (string s in parsedLine)
      // {        Console.WriteLine("s = " + s);      }
      // Console.WriteLine("1");

      // parse on whitespaces so we can make sure the URI 
      // comes after method 
      delimiter = ' ';
      parsedLine = parsedLine[0].Split(delimiter);
      int count = 0;

      foreach (string s in parsedLine)
      {
        // Console.WriteLine("s = :" + s + ":");
        count++;      
      }

      if (count <= 2 && requestSoFar.Contains("\r\n"))
      {
        Console.WriteLine("ValidateRequestURI: not enough fields in request line");
        return false;
      }

      if (count > 3)
      {
        // if the fourth element is null or empty then
        // a space was added at the end of the request line before the CRLF
        // This is okay but if not null then the request is invalid
        bool valid = (String.IsNullOrEmpty(parsedLine[3]) == true) ? true  :  false;

        if (valid ) {       }// carry on
        else
        {
          Console.WriteLine("ValidateRequestURI: too many spaces in request line");
          return false;
        }
      }
      
      // request may not have been read in all the way
      // if ()
      // {}
      
      string UriField = parsedLine[1];
      Console.WriteLine("UriField = " + UriField);
      _URL = UriField;
			_fullURI = true;
      _webRequest.URI = _URL;
			return true;
		}

		// Must account for newline
		public bool ValidateRequestVersion (byte[] buffer)
		{			
			string requestSoFar = Encoding.ASCII.GetString(buffer);
			string versionSubstring;
			int i = 0;
			int whiteSpaceToSkip = 2;
			Console.WriteLine("Validating request Version");
      //Console.WriteLine("request so far: " + requestSoFar);

			while(requestSoFar[i] != '\r' && requestSoFar[i+1] != '\n' && i < requestSoFar.Length)
			{
				if (0 == whiteSpaceToSkip)
				{
					versionSubstring = requestSoFar.Substring (i, 10);// 10 is for "HTTP/1.1" + '\r' + '\n'
					if (versionSubstring != "HTTP/1.1\r\n")
					{
            Console.WriteLine("returning false");
						return false;
					}
					_fullVersion = true;
          _webRequest.HTTPVersion = versionSubstring;
					return true;
				}
				else if(' ' == requestSoFar[i])
				{
					whiteSpaceToSkip--;
				}
				i++;
			}
			// TODO: make more robust i.e. garbage after version, how are line ends compared?						
			return true;
		}

		public bool ValidateRequestHeaders(byte[] buffer)
		{
			String bufferAsString = Encoding.ASCII.GetString(buffer);
			bufferAsString = bufferAsString.Substring(0, bufferAsString.IndexOf("\r\n\r\n"));

      if(bufferAsString.Length > _requestSizeThreshold2)
      {
        return false;
      }

			string[] headers = bufferAsString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

			for (int i = 1; i < headers.Length; i++)
			{
				String header = headers[i];
				//Console.WriteLine("'header' = '{0}'", header);

				if (String.IsNullOrEmpty(header))
				{
					Console.WriteLine("Empty String!!!");
				}
				else
				{
					if (Char.IsLetter(header[0]) && header.Contains(":"))
					{
						//Console.WriteLine("This line contains a colon AND a first-position letter.");
					}
					else
					{
						Console.WriteLine("DARN it's returning false!!!!");
						return false;
					}
				}
			}
			_fullHeaders = true;
      _webRequest.HeadersArray = headers;
			return true;
		}

		public int GetStartOfHeaderBufferIndex(byte[] buffer)
		{
			string requestReadSoFar = Encoding.ASCII.GetString(buffer);
			int i = 0;

			Console.WriteLine("getting Index...");
			if(_validRequestLine)
			{
				// at this point the entire first line is valid
				// Therefore search to first "\r\n" and return index;
				while(requestReadSoFar[i] != '\r' && requestReadSoFar[i+1] != '\n')
				{
					//Console.WriteLine("GetIndex: requestReadSoFar[" + i.ToString() +"]: " +requestReadSoFar[i].ToString());
					//Console.WriteLine("GetIndex: requestReadSoFar[" + (i+1).ToString() +"]" + requestReadSoFar[i+1].ToString());
					i++;
				}
				Console.WriteLine("requestReadSoFar[i+2]: "+ requestReadSoFar[i+2].ToString());
				return i + 2;
			}
			return 0;
		}
	}
}