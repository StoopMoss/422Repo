using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using CS422;
namespace hw5Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			byte[] result = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] bufferToWrite = Encoding.ASCII.GetBytes("abcdef");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);


			streamToTest.Position = 5;			
			streamToTest.SetSeek = false;
			Assert.IsFalse(streamToTest.CanSeek);			
			streamToTest.Write(bufferToWrite, 0, 2);
			streamToTest.SecondStream.Position = 2;
			int bytesRead = streamToTest.Read(result, 0, 2);
		}
	}
}
