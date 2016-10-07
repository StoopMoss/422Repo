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
		public void ReadAllBytesFromConcatStreamThatHasNoSeekAsSecondStream()
		{
			// Arrange
			byte[] buffer = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expectedBuffer = Encoding.ASCII.GetBytes("123456");;			
			
			MemoryStream stream1 = new MemoryStream(buffer1);			
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);			

			// Act
			int numberOfBytesRead = streamToTest.Read(buffer, 0,  expectedBuffer.Length);

			//Assert
			Assert.IsFalse(streamToTest.CanSeek);
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
			int numberOfBytesRead = streamToTest.Read(buffer, 0, expectedBuffer.Length + 100);

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
		}

		[Test]
		public void ReadBackStreamDataInRandomChunks()
		{
			// Arrange
			byte[] result = new byte[1046];
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expected = Encoding.ASCII.GetBytes("123456");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			Random random = new Random();
			int bytesRead  = 0;
			int bytesToRead = 0;
			int totalBytesLeft = (int) (stream1.Length + stream2.Length);
			int i = 0;
			int offset = 0;

			// Act
			while (totalBytesLeft != 0 )
			{
				bytesToRead = random.Next(1, totalBytesLeft);
				bytesRead = streamToTest.Read(result, offset, bytesToRead);
				Console.WriteLine("bytesRead " + bytesRead);
				for (i = 0; i < bytesRead; i++)
				{
					Console.WriteLine("expected[i]" + expected[i]);
					Console.WriteLine("result[i]" + result[i]);
					Assert.AreEqual(expected[i], result[i]);
				}

				totalBytesLeft-= bytesRead;
				offset += bytesRead;

				if(bytesRead == 0)
				{ break;}
			}
		}

		[Test]
		public void ReadALotOfBytesFromStream()
		{
			byte[] result = new byte[100000];
			byte[] expected = new byte[100000];
			byte[] buffer1 = new byte[50000];
			byte[] buffer2 = new byte[50000];
			
			long i = 0;
			for (i = 0; i < 50000; i++)
			{
				buffer1[i] = Convert.ToByte(i % 250);
				expected[i] =  Convert.ToByte(i % 250);
				Console.WriteLine("i = " + i);
			}
			for (; i < 100000; i++)
			{
				buffer2[i] = Convert.ToByte(i% 250);
				expected[i] =  Convert.ToByte(i% 250);
				Console.WriteLine("i = " + i);
			}

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			
			ConcatStream stream = new ConcatStream(stream1, stream2);

			stream.Read(result, 0, 100000);

			Assert.AreEqual(expected, result);
		}



		///////////////////////////////////
		//////////////////////////////////
		// Property Tests
		[Test]
		public void LengthPropertyAllowAccess()
		{
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expected = Encoding.ASCII.GetBytes("123456");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			Assert.NotNull(streamToTest.Length);
		}

		[Test]
		public void LengthPropertyThrowException()
		{
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expected = Encoding.ASCII.GetBytes("123456");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2);

			bool errorWasThrown = false;
			try
			{
				long num = streamToTest.Length;
			}
			catch
			{
				errorWasThrown = true;
			}	
			finally
			{
				Assert.IsTrue(errorWasThrown);		
			}			
		}

		[Test]
		public void LengthPropertyAllowAccessWithSecondConstructorAndSeekCapabilities()
		{
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expected = Encoding.ASCII.GetBytes("123456");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			MemoryStream stream2 = new MemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2, stream1.Length + stream2.Length);

			Assert.NotNull(streamToTest.Length);
		}


		[Test]
		public void LengthPropertyAllowAccessWithSecondConstructorAndNoSeekCapabilities()
		{
			byte[] buffer1 = Encoding.ASCII.GetBytes("123");
			byte[] buffer2 = Encoding.ASCII.GetBytes("456");
			byte[] expected = Encoding.ASCII.GetBytes("123456");

			MemoryStream stream1 = new MemoryStream(buffer1);			
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream(buffer2);
			ConcatStream streamToTest = new ConcatStream(stream1, stream2, 6);

			Assert.NotNull(streamToTest.Length);
			Assert.AreEqual(6, streamToTest.Length);
		}



		
	}
}

