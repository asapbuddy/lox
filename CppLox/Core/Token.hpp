#pragma once
#include <ostream>
#include "TokenType.hpp"
#include <iomanip>

using std::string;

///
/// TODO Token.literal_ void*
/// need to fix Token class and operator <<
///
class Token
{

public:
    Token(TokenType type, string& lexeme, string& literal, int line)
        : type_(type)
        , lexeme_(lexeme)
        , literal_(literal)
        , line_(line)
    {
    }

    friend std::ostream& operator<<(std::ostream& os, const Token& obj)
    {
        return os
               << "[Token] type: " << obj.type_
               << " lexeme: [" << obj.lexeme_ << "]"
               << " literal: [" << obj.literal_ << "]"
               << " line: " << obj.line_;
    }

private:
    const TokenType type_;
    const string lexeme_;
    const string literal_ = nullptr;
    const int line_ = -1;
};
