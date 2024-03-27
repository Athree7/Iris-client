#pragma once

class FuncHook
{
public:
    virtual bool Initialize() = 0;
};

#include "Hooks/Dx12/Dx12.h"

//#include "Hooks/GammaTick.h"
//#include "Hooks/UIControl.h"

void InitializeHooks()
{
    static FuncHook* hooks[] = {
        &Dx12Hook::Instance()
    };

    for (std::size_t i = 0; i < std::size(hooks); ++i)
    {
        if (not hooks[i]->Initialize())
        {
            //error handling
        }
    }
}