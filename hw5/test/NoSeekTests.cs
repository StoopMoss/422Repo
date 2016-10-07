using NUnit.Framework;
using NUnit;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using CS422;

namespace hw5Tests
{
	[TestFixture ]
	public class NoSeekMemoryStreamTests
	{
        private byte[] _byteArray;
        private NoSeekMemoryStream _stream;

        // [TestFixtureSetUp]
        // public void Init()
        // {
        //     byte[] _byteArray = Encoding.ASCII.GetBytes("memoryStream string");
        //     NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);
        // }

        [Test]
        public void ConstructorOne()
        {
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);      
            
            Assert.NotNull(_stream);
            Assert.IsFalse(_stream.CanSeek);
            Assert.IsTrue(_stream.CanRead);
            Assert.IsTrue(_stream.CanWrite);
            Assert.That(()=> _stream.Length, Throws.Exception );
        }

        [Test]
        public void ConstructorTwo()
        {
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray, 0, _byteArray.Length);
           
            Assert.NotNull(_stream);
            Assert.IsFalse(_stream.CanSeek);
            Assert.IsTrue(_stream.CanRead);
            Assert.IsTrue(_stream.CanWrite);
            Assert.That(()=> _stream.Length, Throws.Exception );
        }

        [Test]
        public void CanSeekPropertyShouldReturnFalse()
        {            
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);

            bool result = _stream.CanSeek;

            Assert.IsFalse(result);
        }
        
        [Test]
        public void SeekShouldThrowNotSupportedException()
        {
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);
           
            Assert.Throws<NotSupportedException>(
                ()=> _stream.Seek(0, SeekOrigin.Begin) );
        }

        [Test]
        public void LengthPropertyShouldThrowNotSupportedException()
        {
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);
            
            Assert.That(()=> _stream.Length, Throws.Exception );
        }

        [Test]
        public void PositionPropertySetShouldThrowNotSupportedException()
        {
            byte[] _byteArray = Encoding.ASCII.GetBytes("Stream string");
            NoSeekMemoryStream _stream = new NoSeekMemoryStream(_byteArray);
            
            Assert.That(()=> _stream.Position = 4, Throws.Exception );
        }

    }
}