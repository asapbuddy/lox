#pragma once
#include <string>

struct IError
{
    virtual ~IError() = default;

    virtual void what(std::ostream&) const = 0;
};
