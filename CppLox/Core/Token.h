#pragma once
#include <ostream>

class Token
{
public:
    friend std::ostream& operator<<(std::ostream& stream, const Token& token);
};

inline std::ostream& operator<<(std::ostream& stream, const Token& token)
{
    stream << token;
    return stream;
}
