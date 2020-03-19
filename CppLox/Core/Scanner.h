#pragma once
#include <xstring>
#include <vector>
#include "Token.h"

class Scanner
{
public:
    Scanner(const std::string& source);
    std::vector<Token> scan_tokens();
};
