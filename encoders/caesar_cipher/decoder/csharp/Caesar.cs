using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] buf = new byte[123] { 0xfe, 0x4a, 0x85, 0xe6... };

            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(((uint)buf[i] - 2) & 0xFF);
            }
        }
    }
}
