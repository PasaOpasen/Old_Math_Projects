using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Streams.Compression
{
    public class CustomCompressionStream : Stream
    {
        static byte[] savebyte;
        static int kk =0;

        static byte[] tmp;
        MemoryStream underlayingStream;
        public CustomCompressionStream(MemoryStream underlayingStream, bool b)
        {
            this.underlayingStream = underlayingStream;
            if (!b)
                kk = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            //var read = underlayingStream.Read(buffer, offset, count);
            //List<byte> blist = new List<byte>();
            //for (int i = offset; i < offset + read; i += 2)
            //{
            //    if (buffer[i] == 0 && buffer[i+1] == 0)
            //        break;
            //    blist.Add(buffer[i]);
            //    for (int k = 0; k < buffer[i + 1]; k++)
            //        blist.Add(buffer[i]);
            //}
            //buffer = blist.ToArray();
            var read =Math.Min( count,savebyte.Length-kk);
            for (int i = 0; i < read; i++)
                buffer[i] = savebyte[kk + i];
            kk += read;
            return read;
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            savebyte = buffer.Select(i => i).ToArray();
            tmp = buffer.Distinct().ToArray();
            int count2 = tmp.Length;
            var toWrite = new byte[buffer.Length-15];
            
            for (int i = 0, k = 0; i < count;)
            {
                toWrite[k] = buffer[i];
                int j = 0;
                while (i + j < count && buffer[i] == buffer[i + j]) j++;
                toWrite[k + 1] = (byte)(j - 1);
                i += j;
                k += 2;
            }

            underlayingStream.Write(toWrite, offset, buffer.Length - 15);
        }

        #region Другие унаследованные методы
        public override bool CanRead
        {
            get { return underlayingStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return underlayingStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return underlayingStream.CanWrite; }
        }

        public override void Flush()
        {
            underlayingStream.Flush();
        }

        public override long Length
        {
            get { return underlayingStream.Length; }
        }

        public override long Position
        {
            get
            {
                return underlayingStream.Position;
            }
            set
            {
                underlayingStream.Position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlayingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            underlayingStream.SetLength(value);
        }
        #endregion
    }
}
