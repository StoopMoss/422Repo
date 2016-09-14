using System;


namespace CS422
{
	public class CjHTTPRequest
	{
		private string Method;
		private string URL;
		private string Version;
		//private Hashtable Args;
		private bool Execute;
		//private Hashtable Headers;
		private int BodySize;
		private byte[] BodyData;

		public CjHTTPRequest ()
		{
		}
	}

	public class CjHTTPResponse
	{
		private int status;
		private string version;
		//private JSON Headers;
		private int BodySize;
		private byte[] BodyData;
		private System.IO.FileStream fs;

		public CjHTTPResponse ()
		{
		}
	}
}

