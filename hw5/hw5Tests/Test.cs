using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using CS422;

namespace hw5Tests
{
	[TestFixture ]
	public class Test
	{
		[Test ]
		public void ConcatStreamConstructor ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			MemoryStream stream2 = new MemoryStream(byteArray);

			Console.WriteLine("hjgfkjghkjgkjgkjhgkjhgkjhg");
			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2);

			//Assert
			Assert.NotNull(concatStream);
			//concatStream
		}
	}
}

