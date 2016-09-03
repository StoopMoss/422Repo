using System;
using NUnit.Framework;
using CS422;

namespace HW1Tests
{
	[TestFixture]
	public class TestClass
	{
		// Text Writer Tests
		[Test]
		public void ConstructorTest()
		{
			//Arrange
			//Act
			NumberedTextWriter testWriter = new NumberedTextWriter(Console.Out);

			//Assert
			Assert.AreEqual(1 , testWriter.CurrentLine);
		}

		[Test]
		[TestCase(42)]
		public void SecondConstructorTest(int line)
		{
			//Arrange
			//Act
			NumberedTextWriter testWriter = new NumberedTextWriter(Console.Out, line);

			//Assert
			Assert.AreEqual(line , testWriter.CurrentLine);
			Assert.AreSame (Console.Out, testWriter.Writer);
		}

		[Test]
		public void WriteLineTest()
		{
			//Arrange
			//NumberedTextWriter testWriter = new NumberedTextWriter(Console.Out);

			//Act

			//Assert
		}


		//Arrange
		//Act
		//Assert

		// Index Stream tests
		[TestCase(0, 3)]
		[Test]
		public void IndexReadWithNoOffset(int offset, int count)
		{
			//Arrange
			IndexedNumsStream stream = new IndexedNumsStream();
			byte[] buffer = new byte[1000];

			//Act
			stream.Read(buffer, offset, count);

			//Assert
			Assert.AreEqual(stream.Length, 255);
			Assert.AreEqual(0, buffer[0]);
			Assert.AreEqual(1, buffer[1]);
			Assert.AreEqual(2, buffer[2]);
		}


		[TestCase(0, 3)]
		[Test]
		public void IndexReadOverModBoundry(int offset, int count)
		{
			//Arrange
			IndexedNumsStream stream = new IndexedNumsStream(550);
			byte[] buffer = new byte[1000];
			stream.Position = 254;

			//Act
			stream.Read(buffer, offset, count);

			//Assert
			Assert.AreEqual(550, stream.Length);
			Assert.AreEqual(254, buffer[0]);
			Assert.AreEqual(255, buffer[1]);
			Assert.AreEqual(0, buffer[2]);
		}

		[TestCase(0, 3)]
		[Test]
		public void IndexReadToEnd(int offset, int count)
		{
			//Arrange
			IndexedNumsStream stream = new IndexedNumsStream(30);
			byte[] buffer = new byte[1000];
			stream.Position = 29;

			//Act
			stream.Read(buffer, offset, count);

			//Assert
			Assert.AreEqual(30, stream.Length);
			Assert.AreEqual(29, buffer[0]);
			Assert.AreEqual(30, buffer[1]);
			Assert.AreEqual(0, buffer[2]);
		}

	}
}

