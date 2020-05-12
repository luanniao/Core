using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LuanNiao.Core.TextTools
{
    public static class BrotliUTF8
    {
        public static byte[] Compress(string data)
        {
            return Compress(Encoding.UTF8.GetBytes(data));
        }
        public static byte[] CompressHightLevel(string data)
        {
            return CompressHightLevel(Encoding.UTF8.GetBytes(data));
        }
        public static byte[] Compress(ReadOnlySpan<byte> source)
        {
            using var outStream = new MemoryStream();
            using var brotliStream = new System.IO.Compression.BrotliStream(outStream, System.IO.Compression.CompressionLevel.Fastest);
            brotliStream.Write(source);
            brotliStream.Close();
            return outStream.ToArray();
        }
        public static byte[] CompressHightLevel(ReadOnlySpan<byte> source)
        {
            using var outStream = new MemoryStream();
            using var brotliStream = new System.IO.Compression.BrotliStream(outStream, System.IO.Compression.CompressionLevel.Optimal);
            brotliStream.Write(source);
            brotliStream.Close();
            return outStream.ToArray();
        }



        public static byte[] Decompress(ReadOnlySpan<byte> source)
        {
            using var inputStream = new MemoryStream();
            inputStream.Write(source);
            inputStream.Flush();
            inputStream.Position = 0;
            using var outStream = new MemoryStream();
            using var brotliStream = new System.IO.Compression.BrotliStream(inputStream, System.IO.Compression.CompressionMode.Decompress);
            brotliStream.CopyTo(outStream);
            return outStream.ToArray();
        }

        public static string GetString(ReadOnlySpan<byte> source)
        {
            return Encoding.UTF8.GetString(Decompress(source));
        }
    }
}
