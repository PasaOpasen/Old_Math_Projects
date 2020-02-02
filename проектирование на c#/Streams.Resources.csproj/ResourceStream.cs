using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        public override bool CanRead=>testStream.CanRead;

        public override bool CanSeek=>testStream.CanSeek; 

        public override bool CanWrite=>testStream.CanWrite; 

        public override long Length=>testStream.Length; 

        public override long Position
        {
            get
            {
                return testStream.Position;
            }
            set
            {
                testStream.Position = value;
            }

        }

        Stream testStream;
        string key;
        byte[] bufferIT = new byte[Constants.BufferSize];
        int indexValue = -1;
        List<byte> value;

        public ResourceReaderStream(Stream stream, string key)
        {
            testStream = stream;
            this.key = key;
            SeekValue();
            if (indexValue > 0) ReadFieldValue();
            outVal = 0;
        }

        private static int outVal = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (indexValue == -1) return 0;
            if (value == null) return 0;
            var read = Math.Min(value.Count - outVal - offset, count);
            for (int i = offset; i < offset + read; i++)
            {
                buffer[i] = value[i + outVal];
            }
            outVal += read;
            return read;
        }

        private void SeekValue()
        {
            bool find = false;
            indexValue = -1;
            var read = testStream.Read(bufferIT, 0, 1024);
            string asString = Encoding.ASCII.GetString(bufferIT);

            while (read > 0 && !find)
            {
                if (asString.Contains(key))
                {
                    find = true;
                    indexValue = asString.IndexOf(key) + key.Length + 2;
                }
                else
                    read = testStream.Read(bufferIT, 0, Constants.BufferSize);
            };
        }

        private void ReadFieldValue()
        {
            bool find = false;
            int read = 1;
            int first = indexValue;
            value = new List<byte>();
            while (read > 0 && !find)
            {
                int end = Array.IndexOf(bufferIT, (byte)1, indexValue);
                if (end > 0)
                {
                    value = value.Concat((bufferIT.Skip(indexValue).Take(end - indexValue - 1))).ToList();
                    find = true;
                }
                else
                {
                    end = 1024;
                    value = value.Concat((bufferIT.Skip(indexValue).Take(end - indexValue))).ToList();
                    read = testStream.Read(bufferIT, 0, Constants.BufferSize);
                    indexValue = 0;
                }
            }
            int ind = 0;
            while (true)
            {
                int r = value.IndexOf((byte)0, ind);
                if (r < 0) break;
                ind = r + 1;
                if (r > 0 && r < (value.Count - 1) && value[r + 1] == 0) value.RemoveAt(r);
            }
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }

}
