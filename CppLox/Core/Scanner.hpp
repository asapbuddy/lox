#pragma once
#include <utility>
#include <xstring>
#include <vector>
#include "Token.hpp"
#include "IError.hpp"
#include <unordered_map>
#include "ScannerError.h"

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
        errors_.emplace_back(new ScannerError(line_, "TEST ERROR"));
    }

    Scanner() = delete;

    /**
     * \brief parse source code
     * \return all parsed tokens
     */
    std::vector<Token> scan_tokens();
    std::vector<IError*> get_errors();

    ~Scanner()
    {
        for(auto* error : errors_)
            delete error;
    }

private:
    unsigned start_ = 0;
    unsigned current_ = 0;
    unsigned line_ = 1;

    const string source_;
    std::vector<Token> tokens_;
    std::vector<IError*> errors_;

    // Helpers
    bool isAtEnd();
    void addToken(TokenType);
    void addToken(TokenType, string&);
    void addError(string);
    char peek();

    void add_string();
    char peekNext();
    void add_number();
    bool isDigit(char c);
    bool isAlphaNumeric(char peek);
    void add_identifier();
    bool isAlpha(char c);
    // Core
    void scanToken();
    char advance();
    bool match(char expected);

    const std::unordered_map<string, TokenType>
        keywords_{
            {"and", TokenType::AND},
            {"class", TokenType::CLASS},
            {"else", TokenType::ELSE},
            {"false", TokenType::FALSE},
            {"for", TokenType::FALSE},
            {"fun", TokenType::FUN},
            {"if", TokenType::IF},
            {"nil", TokenType::NIL},
            {"or", TokenType::OR},
            {"print", TokenType::PRINT},
            {"return", TokenType::RETURN},
            {"super", TokenType::SUPER},
            {"this", TokenType::THIS},
            {"true", TokenType::TRUE},
            {"var", TokenType::VAR},
            {"while", TokenType::WHILE}};
};
