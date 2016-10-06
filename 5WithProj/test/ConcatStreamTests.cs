using NUnit.Framework;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
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
		public void FirstConstructorSetPositionToZeroWhenFirstStreamPositionIsNotZero()
		{
			//Arrange
			byte[] byteArray = Encoding.ASCII.GetBytes("memoryStream string");
			MemoryStream stream1 = new MemoryStream(byteArray);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(byteArray);

			stream1.Position = 4;

			//Act
			ConcatStream concatStream = new ConcatStream(stream1, stream2);

			// //Assert
			Assert.NotNull(concatStream);
			Assert.IsFalse(concatStream.CanSeek);
			Assert.IsTrue(concatStream.CanRead);
			Assert.IsTrue(concatStream.CanWrite);
			Assert.AreEqual(0, concatStream.Position);						
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
		public void ReadAllBytesFromConcatStream()
		{
			// Arrange
			byte[] buffer = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expectedBuffer = new byte[buffer1.Length + buffer2.Length];			
			System.Buffer.BlockCopy(buffer1, 0, expectedBuffer, 0, buffer1.Length);
			System.Buffer.BlockCopy(buffer2, 0, expectedBuffer, buffer1.Length, buffer2.Length);
			
			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			// Act
			int numberOfBytesRead = streamToTest.Read(buffer, 0, expectedBuffer.Length);

			//Assert
			Assert.IsTrue(streamToTest.CanSeek);
			Assert.AreEqual(expectedBuffer.Length, numberOfBytesRead);

			for (int i = 0; i < expectedBuffer.Length; i++)
			{
				Assert.AreEqual(expectedBuffer[i], buffer[i]);				
			}
		}

		[Test]
		public void AttemptToReadMoreBytesThanAvailableFromConcatStream()
		{
			// Arrange
			byte[] buffer = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expectedBuffer = new byte[buffer1.Length + buffer2.Length + 4];			
			System.Buffer.BlockCopy(buffer1, 0, expectedBuffer, 0, buffer1.Length);
			System.Buffer.BlockCopy(buffer2, 0, expectedBuffer, buffer1.Length, buffer2.Length);

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			// Act
			int numberOfBytesRead = streamToTest.Read(buffer, 0, expectedBuffer.Length);

			//Assert
			Assert.IsTrue(streamToTest.CanSeek);
			Assert.AreEqual(streamToTest.Length, numberOfBytesRead);
			for (int i = 0; i < streamToTest.Length; i++)
			{
				Assert.AreEqual(expectedBuffer[i], buffer[i]);				
			}
			
		}
		
		[Test]
		public void ReadFromFirstStreamOnly()
		{
			// Arrange
			byte[] buffer = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expectedBuffer = new byte[buffer1.Length + buffer2.Length];			
			System.Buffer.BlockCopy(buffer1, 0, expectedBuffer, 0, buffer1.Length);
			System.Buffer.BlockCopy(buffer2, 0, expectedBuffer, buffer1.Length, buffer2.Length);
			
			MemoryStream stream1 = new MemoryStream(buffer1);			
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);			

			// Act
			int numberOfBytesRead = streamToTest.Read(buffer, 0,  buffer1.Length);

			//Assert
			Assert.IsFalse(streamToTest.CanSeek);
			Assert.AreEqual(buffer1.Length, numberOfBytesRead);

			for (int i = 0; i < buffer1.Length; i++)
			{
				Assert.AreEqual(buffer1[i], buffer[i]);				
			}

		}
				
		[Test]
		public void ReadFromSecondStreamOnly()
		{
			// Arrange
			byte[] buffer = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expectedBuffer = new byte[buffer1.Length + buffer2.Length];			
			System.Buffer.BlockCopy(buffer1, 0, expectedBuffer, 0, buffer1.Length);
			System.Buffer.BlockCopy(buffer2, 0, expectedBuffer, buffer1.Length, buffer2.Length);
			
			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			streamToTest.Position = buffer1.Length;
			Console.WriteLine(buffer1.Length.ToString());
			Console.WriteLine("Position " + streamToTest.Position.ToString());
			Console.WriteLine("Length " + streamToTest.Length.ToString());

			// Act
			int numberOfBytesRead = streamToTest.Read(buffer, 0,  buffer2.Length);

			//Assert
			Assert.AreEqual(3, numberOfBytesRead);

			for (int i = 0; i < buffer2.Length; i++)
			{
				Console.WriteLine("buffer[i]" + buffer[i].ToString());
				Console.WriteLine("buffer2[i]" + buffer2[i].ToString());
				Assert.AreEqual(buffer2[i], buffer[i]);				
			}

		}

		[Test]
		public void WritePastEndOfCurrentStreamShouldThrowException()
		{
			// Arrange
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] bufferToWrite = Encoding.ASCII.GetBytes("123456789");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			// Act
			Assert.Throws<NotSupportedException>(() => streamToTest.Write(bufferToWrite, 0, bufferToWrite.Length));

			// streamToTest.Write(bufferToWrite, 0, bufferToWrite.Length)

		}
		
		[Test]
		public void WriteOverMiddleBoundryOfBothStreamsWithSeekEnabled()
		{
			// Arrange
			byte[] result = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] bufferToWrite = Encoding.ASCII.GetBytes("abcdef");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			// Act
			streamToTest.Write(bufferToWrite, 0, bufferToWrite.Length);
			streamToTest.Position = 0;
			streamToTest.Read(result, 0, bufferToWrite.Length);

			for (int i = 0; i < bufferToWrite.Length; i++)
			{
				Console.WriteLine("Position " + streamToTest.Position);
				Console.WriteLine("result[i]: " + result[i]);
				Assert.AreEqual(bufferToWrite[i], result[i]);
			}
		
		}

		[Test]
		public void WriteOverMiddleBoundryOfBothStreamsWithSeekDisabled()
		{
				// Arrange
			byte[] result = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] bufferToWrite = Encoding.ASCII.GetBytes("abcdef");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			// Act
			streamToTest.Write(bufferToWrite, 0, bufferToWrite.Length);

			//Assert
			string actual = streamToTest.ToString();
			Assert.IsFalse(streamToTest.CanSeek);			

			for (int i = 0; i < bufferToWrite.Length; i++)
			{
				Assert.AreEqual(actual[i], bufferToWrite[i]);				
			}
		}





		
	}
}

