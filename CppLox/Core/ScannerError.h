#pragma once
#include <utility>
#include "IError.hpp"
#include <ostream>

class ScannerError : public IError
{
public:
    ScannerError(int line, std::string desc)
        : line_(line)
        , description_(std::move(desc))
    {
    }

    friend std::ostream& operator<<(std::ostream& os, const ScannerError& obj)
    {
        return os
               << "[ScannerError] "
               << " line_: " << obj.line_
               << " description_: " << obj.description_;
    }

    std::string what() override;
    ~ScannerError() = default;

private:
    const int line_ = -1;
    std::string description_;
};
