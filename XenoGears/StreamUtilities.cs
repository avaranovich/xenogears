using System;
using System.Diagnostics;
using System.IO;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class StreamUtilities
    {
        public static Stream CacheInMemory(this Stream s)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                lock (s)
                {
                    var m = new MemoryStream();
                    var originalPos = s.Position;

                    try
                    {
                        int b;
                        while ((b = s.ReadByte()) != -1)
                        {
                            m.WriteByte((byte)b);
                        }

                        return m;
                    }
                    finally
                    {
                        s.Seek(originalPos, SeekOrigin.Begin);
                        m.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
        }
        public static String DumpToString(this Stream s)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                lock (s)
                {
                    var originalPos = -1L;
                    if (s.CanSeek) originalPos = s.Position;

                    try
                    {
                        return new StreamReader(s).ReadToEnd();
                    }
                    finally
                    {
                        if (s.CanSeek) s.Seek(originalPos, SeekOrigin.Begin);
                    }
                }
            }
        }

        public static byte[] DumpToByteArray(this Stream s)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                lock (s)
                {
                    var originalPos = -1L;
                    if (s.CanSeek) originalPos = s.Position;

                    try
                    {
                        var readBuffer = new byte[4096];

                        var totalBytesRead = 0;
                        int bytesRead;

                        while ((bytesRead = s.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                        {
                            totalBytesRead += bytesRead;

                            if (totalBytesRead == readBuffer.Length)
                            {
                                var nextByte = s.ReadByte();
                                if (nextByte != -1)
                                {
                                    var temp = new byte[readBuffer.Length * 2];
                                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                                    Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                                    readBuffer = temp;
                                    totalBytesRead++;
                                }
                            }
                        }

                        var buffer = readBuffer;
                        if (readBuffer.Length != totalBytesRead)
                        {
                            buffer = new byte[totalBytesRead];
                            Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                        }

                        return buffer;
                    }
                    finally
                    {
                        if (s.CanSeek) s.Seek(originalPos, SeekOrigin.Begin);
                    }
                }
            }
        }

        public static void DumpToFile(this Stream stream, String fileName)
        {
            File.WriteAllBytes(fileName, stream.DumpToByteArray() ?? new byte[0]);
        }

        public static String DumpToTempFile(this Stream stream)
        {
            var tempFile = Path.GetTempFileName();
            stream.DumpToFile(tempFile);
            return tempFile;
        }

        public static void ReadFromByteArray(this Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }

        public static void ReadFromFile(this Stream stream, String fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            stream.ReadFromByteArray(bytes);
        }
    }
}