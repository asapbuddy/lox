#pragma once
#include "IError.hpp"
#include <ostream>
#include <utility>

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
            << " line: " << line_
            << " error: " << description_
            << std::endl;
    }

    ~ScannerError() override = default;

private:
    unsigned line_ = 0;
    std::string description_;
};
