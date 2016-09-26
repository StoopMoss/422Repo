
//		public static bool ValidateHTTPRequestLine(byte[] buffer)
//		{			
//			// get first line of request (The request line)
//			string requestLine = GetRequestLine(buffer);
//			string versionSubstring;
//
//			//1. Only allow GET
//			// Check HTTP Method for "GET " at start of string
//			if (requestLine.Length > 4)
//			{
//				string actual = requestLine.Substring (0, 4);
//
//				if (actual != "GET ")
//				{
//					return false;
//				}
//			}
//
//			//2. Allow any valid URI
//			//TODO: what to do about URI's
//
//			//3. Allow HTTP/1.1
//			// Traverse over two whitespaces then validate substring
//			int i = 0;
//			int whiteSpaceToSkip = 2;
//			while(requestLine[i] != '\r' && requestLine[i+1] != '\n' && i < requestLine.Length)
//			{
//				if (0 == whiteSpaceToSkip)
//				{
//					versionSubstring = requestLine.Substring (i);
//					break;
//				}
//				else if(' ' == requestLine[i])
//				{
//					whiteSpaceToSkip--;
//				}
//
//				i++;
//			}
//
//			// validate version
//			// TODO: make more robust i.e. garbage after version, how are line ends compared?
//			if (versionSubstring != "HTTP/1.1")
//			{
//				return false;
//			}
//
//			// CRLF
//			//TODO: make sure request line ends correctly
////			string ending = requestLine.Substring(requestLine.Length - 2);
////			if (ending != "\r\n")
////			{
////				return false;
////			}
//
//			return true;
//		}


//string substring = new string(buffer);
//if (substring.Contains("\r\n") && !checkedVersion)
//{
//	// Check  for “HTTP/1.1”
//	bool validVersion = ValidateRequestVersion(buffer);
//
//	if (!validVersion)
//	{ return false; }
//	checkedVersion = true;
//}




//		public static bool ReadFromNetworkStream(NetworkStream networkStream, ref byte[] buf)
//		{
//			byte[] buffer = new byte[4096];
//			int bytesRead = 0;
//			int totalBytesRead = 0;
//
//			if (networkStream.CanRead) 
//			{
//				do {
//					// read from stream until buffer is full or Read() returns 0
//					bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
//					totalBytesRead += bytesRead;
//				} while (bytesRead != 0);
//			} 
//			else 
//			{
//				return false;
//			}
//			return true;
//
//			// Must validate as you read.
//
//		}




//public static bool ReadFromNetworkStream(NetworkStream networkStream)
//{
//	byte[] buffer = new byte[4096];
//	int bytesRead = 0, totalBytesRead = 0;
//	bool checkedMethod = false, checkedVersion = false;
//	int whiteSpace = 2;
//
//	if (networkStream.CanRead) 
//	{
//		do {
//			// read from stream until buffer is full or Read() returns 0
//			bytesRead = networkStream.Read (buffer, totalBytesRead, buffer.Length - totalBytesRead);
//			totalBytesRead += bytesRead;
//			string substring =  buffer.ToString();
//
//			if (!checkedMethod)
//			{
//				// Check method for "GET "
//				if (ValidateRequestMethod(buffer, totalBytesRead))
//				{
//					if (totalBytesRead > 4)	
//					{ 
//						checkedMethod = true; 
//					}
//				}
//				else
//				{ return false; }
//			}
//			else if (!checkedVersion)
//			{			
//
//				if (substring.Contains(" "))
//				{
//					validateURI(buffer, totalBytesRead);
//				}
//
//				// Check  for “HTTP/1.1”
//				if( ValidateRequestVersion(buffer, totalBytesRead) )
//				{
//					if (substring.Contains("\r\n"))
//					{
//						checkedVersion = true;
//					}								
//				}
//				else
//				{ return false; }							
//			}
//			//TODO: Header validation
//			else
//			{
//				//check to see if double return has been reached
//				string requestReadSoFar = buffer.ToString();
//				if (requestReadSoFar.Contains("\r\n\r\n") )
//				{
//					// stop reading and send back a response
//					return true;
//				}
//			}
//		} while (bytesRead != 0);
//	}
//	return false;
//}




//public static bool ValidateHTTPHeaders(byte[] buffer)
//{
//	// ..
//	// CRLF
//	return true;
//}

//
//// Get the headers as a string
//Console.WriteLine("Validating request headers");
//string headerString = Encoding.ASCII.GetString(buffer);
//Console.WriteLine();
////Console.WriteLine("string: " + headerString.ToString());
//int startOfHeadersIndex = GetStartOfHeaderBufferIndex (buffer);
//Console.WriteLine("Index: " + startOfHeadersIndex.ToString());
//
//if(startOfHeadersIndex == 0)
//{
//	// Error: shouldn't be in this function if 0 is returned from getIndex
//	return false;
//}
//
////headerString = headerString.Substring (startOfHeadersIndex);
////Console.WriteLine();
////Console.WriteLine("HeaderSection: " + headerString);
////Console.WriteLine(headerString.Length.ToString());
//
//int i = startOfHeadersIndex;
//bool firstCharacter = true, hitColon = false;
//
//Console.WriteLine("About to enter loooop");
//while (i < headerString.Length && !_fullHeaders ) 
//{
//	Console.WriteLine("Next Header Line");
//	// inner loop is for one header line
//	while (headerString [i] != '\r' && i < headerString.Length) 
//	{				
//		Console.WriteLine("Char[" + i.ToString() + "]: " + headerString[i]);
//		// check each header...
//		if (firstCharacter) 
//		{
//			if (!char.IsLetter (headerString [i])) 
//			{							
//				return false;
//			} 
//			else 
//			{
//				firstCharacter = false;
//			}
//		} 
//		else 
//		{
//			if (headerString [i] == ':') 
//			{
//				if(hitColon)
//				{
//					// Already hit a colon on this line
//					return false;
//				}
//				hitColon = true;
//			}
//		}
//		i++;
//	}
//
//	//Check for a valid line end
//	if(headerString [i+1] != '\n')
//	{
//		// hit an \r but not a \n
//		return false;
//	}
//
//	if (!hitColon) {
//		//Invalid: finished line and no colon was encountered
//		return false;
//	}
//
//	// check for double return
//	if (headerString[i] == '\r' && headerString[i+2] == '\r')
//	{
//		// Account for double Newline
//		_fullHeaders = true;
//		return true;
//	}
//
//	// Reset flags for next line
//	hitColon = false;
//	firstCharacter = true;
//
//	i ++;
//}
//return true;




//
//public static bool ValidateRequestHeaders(byte[] buffer)
//{
//	string[] headers = Encoding.ASCII.GetString(buffer).Split('\n');
//
//	for (int i = 1; i < headers.Length; i++)
//	{
//		Console.WriteLine(headers[i]);
//		if (!Char.IsLetter(headers[i][0]) || !headers[i].Contains(":"))
//		{	
//			return false;
//		}
//	}
//	_fullHeaders = true;
//	return true;
//}