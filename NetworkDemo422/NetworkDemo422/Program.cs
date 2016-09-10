using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkDemo422
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//(TCP client and TCP listener)
			TcpListener listener = new  TcpListener (IPAddress.Any, 4220);
			listener.Start ();
			var client = listener.AcceptTcpClient();

			NetworkStream ns = client.GetStream();
			byte[] buf = new byte[4096];
			int read = ns.Read(buf, 0, buf.Length);

			//1. Only allow GET
			//2. Allow any valid URI
			//3. Allow HTTP/1.1

			if(Encoding.ASCII.GetString(buf, 0 , buf.Length), "GET ")
			{
				//Kill connection
			}

			string fromBrowser = Encoding.ASCII.GetString (buf, 0,read);
			Console.WriteLine (fromBrowser);

			string response = "put an HTTP response here..";
			byte[] responseBytes;
			ns.Write(responseBytes, 0 , responseBytes.Length);
			ns.Dispose();

		}
	}
}
