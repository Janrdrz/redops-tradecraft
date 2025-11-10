using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] buf = new byte[123] {0xfc,0x48,0x83,0xe4,0xf0,0xe8...};

            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(((uint)buf[i] + 2) & 0xFF);
            }

            StringBuilder hex = new StringBuilder(encoded.Length * 2);
            int size = encoded.Length;

            foreach (byte b in encoded)
            {
                hex.AppendFormat("0x{0:x2}, ", b);
            }
            Console.WriteLine("Encoded bytes: " + hex.ToString());
            Console.WriteLine("Buffer size: " + size);
            Console.ReadLine();
        }
    }
}
