#pragma once
#include <utility>
#include <xstring>
#include <vector>
#include "Token.hpp"
#include "IError.hpp"

class Scanner
{
public:
    /**
     * \brief Store source
     * \param source contains full source code of file
     */
    Scanner(string source)
        : source_(std::move(source))
    {
    }

    Scanner() = delete;

    /**
     * \brief parse source code
     * \return all parsed tokens
     */
    std::vector<Token> scan_tokens();

private:
    int start_ = 0;
    int current_ = 0;
    int line_ = 1;

    const string source_;
    std::vector<Token> tokens_;
    std::vector<std::unique_ptr<IError>> errors_;

    // Helpers
    bool isAtEnd();
    void addToken(TokenType);
    void addToken(TokenType, void*);
    void addError(string);
    char peek();

    // Core
    void scanToken();
    char advance();
    bool match(char expected);
};
