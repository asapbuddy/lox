#pragma once
#include <utility>
#include "IError.hpp"
#include <ostream>

class ScannerError : public IError
{
public:
    ScannerError(unsigned line, std::string desc)
        : line_(line)
        , description_(std::move(desc))
    {
    }

    void what(std::ostream& os) const override
    {
        os
            << "[ScannerError] "
            << " line_: " << line_
            << " description_: " << description_
            << std::endl;
    }

    ~ScannerError() override = default;

private:
    unsigned line_ = 0;
    std::string description_;
};
