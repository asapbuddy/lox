#pragma once
#include <ostream>
#include "TokenType.hpp"

using std::string;

///
/// TODO Token.literal_ void*
/// need to fix Token class and operator <<
///
class Token
{

public:
    explicit Token(TokenType type, string& lexeme, void* literal, int line)
        : type_(type)
        , lexeme_(lexeme)
        , literal_(literal)
        , line_(line)
    {
    }

    friend std::ostream& operator<<(std::ostream& os, const Token& obj)
    {
        return os
               << "type_: " << obj.type_
               << " lexeme_: " << obj.lexeme_
               << " literal_: " << obj.literal_
               << " line_: " << obj.line_;
    }

private:
    const TokenType type_;
    const string lexeme_;
    const void* literal_ = nullptr;
    const int line_ = -1;
};
