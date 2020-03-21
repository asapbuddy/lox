#pragma once
#include <string>

struct IError
{
    virtual ~IError() = default;

    virtual std::string what() = 0;
};
