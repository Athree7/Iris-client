#include <windows.h>
#include <sstream>
#include <chrono>
#include <map>
#include <string>
#include <iostream>
#include <psapi.h>
#include <cassert>
#include <cstdarg>
#include <sysinfoapi.h>
#include <corecrt_math.h>
#include <unordered_map>
#include <wininet.h>
#include <tchar.h>

#include "Utils/MemoryUtil.h"

#include "../Iris/Libs/minhook/minhook.h"

#include "Hook/Hooks.h"

void InitClient()
{
    if (MH_Initialize() == MH_OK)
    {
        InitializeHooks();
    }
}

BOOL APIENTRY DllMain(HMODULE module, DWORD reason, LPVOID reserved)
{
    if (reason == DLL_PROCESS_ATTACH)
    {
        DisableThreadLibraryCalls(module);
        CreateThread(0, 0, (LPTHREAD_START_ROUTINE)InitClient, module, 0, 0);
    }
    return TRUE;
}