using System;
using System.IO;

/// <summary>
/// Represents a memory stream that does not support seeking, but otherwise has
/// functionality identical to the MemoryStream class.
/// </summary>
namespace CS422
{
    public class NoSeekMemoryStream : MemoryStream
    {        
        private bool _canSeek;        
        
        //Properties
        public override bool  CanSeek
        {
            get {return false;}
        }

        public override long  Length
        {
            get 
            {
                throw new NotSupportedException(
                    "NoSeekMemoryStream: LengthNotSupported");
            }          
        }

        // Constructors
        public NoSeekMemoryStream(byte[] buffer):base(buffer)
        {}

        public NoSeekMemoryStream(byte[] buffer, int offset, int count):base(buffer,offset, count)
        {}        
        

        // Methods
        public override long Seek(long offset, SeekOrigin loc)
        {
            throw new NotSupportedException("NoSeekMemoryStream: seeking not supported.");
        }

    }
}