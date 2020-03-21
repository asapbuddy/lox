#include "ScannerError.h"

std::string ScannerError::what()
{
    return "Line " + line_ + description_;
}
