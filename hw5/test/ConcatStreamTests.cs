using NUnit.Framework;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using CS422;

namespace hw5Tests
{
	[TestFixture ]
	public class ConcatStreamTests
	{		
		[Test ]
		public void FirstConstructorWithTwoMemoryStreams ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			MemoryStream stream2 = new MemoryStream(byteArray);

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2);

			//Assert
			Assert.NotNull(concatStream);
			Assert.IsTrue(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
		}

		[Test ]
		public void FirstConstructorWithNoSeekAsSecondStream ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2);

			//Assert
			Assert.NotNull(concatStream);
			Assert.IsFalse(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
		}


		[Test ]
		public void FirstConstructorWithNoSeekAsFirstStream ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			NoSeekMemoryStream stream1 = new NoSeekMemoryStream(byteArray);
			MemoryStream stream2 = new MemoryStream(byteArray);

			//Assert
			Assert.Throws<ArgumentException>(()=> new ConcatStream(stream1, stream2) );
		}

		[Test ]
		public void FirstConstructorStreamWithNoLengthForFirstStream ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			NoSeekMemoryStream stream1 = new NoSeekMemoryStream(byteArray);
			MemoryStream stream2 = new MemoryStream(byteArray);

			//Assert
			Assert.Throws<ArgumentException>(()=> new ConcatStream(stream1, stream2) );
		}

		[Test ]
		public void FirstConstructorStreamWithNoLengthForSecondStream ()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2);

			//Assert
			Assert.NotNull(concatStream);
			Assert.IsFalse(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
		}

		[Test ]
		public void SecondConstructorWithNoSeekStreamAsSecondParameter()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);
			long length =  2 * stream1.Length;

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2, length);

			//Assert
			Assert.NotNull(concatStream);
			Assert.IsFalse(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
			Assert.NotNull(concatStream.Length);			
		}

		[Test ]
		public void SecondConstructorWithNoSeekStreamAsFirstParameter()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			NoSeekMemoryStream stream1 = new NoSeekMemoryStream(byteArray);
			MemoryStream stream2 = new MemoryStream(byteArray);
			long length =  2 * stream2.Length;

			// //Assert
			Assert.Throws<ArgumentException>(()=> 
				new ConcatStream(stream1, stream2, length) );			
		}

		[Test ]
		public void SecondConstructorWithLengthParameterThatIsLessThanFirstStreamLength()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);
			long length =  stream1.Length - 1; 

			// //Assert
			Assert.Throws<ArgumentException>(()=> 
				new ConcatStream(stream1, stream2, length) );			
		}

		[Test ]
		public void SecondConstructorSetPositionToZeroWhenFirstStreamPositionIsNotZero()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);
			long length =  2 * stream1.Length;

			stream1.Position = 4;

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2, length);

			// //Assert
			Assert.NotNull(concatStream);
			Assert.IsFalse(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
			Assert.NotNull(concatStream.Length);
			Assert.AreEqual(0, concatStream.Position);						
		}

		////////////////////////////////////////////////////////
		//Read and Write Tests
		////////////////////////////////////////////////////////
		[Test]
		public void ReadAllBytesFromConcatStreamWithSeekCapability()
		{
			byte[] byteArray1 = Encoding.ASCII.GetBytes("first");
			MemoryStream stream1 = new MemoryStream(byteArray1);
			byte[] byteArray2 = Encoding.ASCII.GetBytes("second");
			MemoryStream stream2 = new MemoryStream(byteArray2);
		}

		[Test]
		public void ReadAllBytesFromConcatStreamWithNoSeekCapability()
		{

		}
		
		[Test]
		public void ReadAllBytesFromConcatStreamWithSpacesInStream()
		{

		}
	}
}

