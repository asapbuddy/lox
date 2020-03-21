#include "Scanner.hpp"
#include "ScannerError.h"

std::vector<Token> Scanner::scan_tokens()
{
    while(!isAtEnd())
    {
        start_ = current_;
        scanToken();
    }

    tokens_.emplace_back(TokenType::EOF, "", nullptr, line_);
    return tokens_;
}

bool Scanner::isAtEnd()
{
    return current_ >= source_.size();
}

void Scanner::addToken(TokenType type)
{
    addToken(type, nullptr);
}

void Scanner::addToken(TokenType type, void* literal)
{
    string text = source_.substr(start_, current_);
    tokens_.emplace_back(type, text, literal, line_);
}

void Scanner::addError(string text)
{
    errors_.push_back(std::make_unique<ScannerError>(line_, text));
}

char Scanner::peek()
{
    return isAtEnd() ? '\0' : source_.at(current_);
}

void Scanner::scanToken()
{
    char c = advance();
    switch(c)
    {
    case '(':
        addToken(TokenType::LEFT_PAREN);
        break;
    case ')':
        addToken(TokenType::RIGHT_PAREN);
        break;
    case '{':
        addToken(TokenType::LEFT_BRACE);
        break;
    case '}':
        addToken(TokenType::RIGHT_BRACE);
        break;
    case ',':
        addToken(TokenType::COMMA);
        break;
    case '.':
        addToken(TokenType::DOT);
        break;
    case '-':
        addToken(TokenType::MINUS);
        break;
    case '+':
        addToken(TokenType::PLUS);
        break;
    case ';':
        addToken(TokenType::SEMICOLON);
        break;
    case '*':
        addToken(TokenType::STAR);
        break;
    case '!':
        addToken(match('=') ? TokenType::BANG_EQUAL : TokenType::BANG);
        break;
    case '=':
        addToken(match('=') ? TokenType::EQUAL_EQUAL : TokenType::EQUAL);
        break;
    case '<':
        addToken(match('=') ? TokenType::LESS_EQUAL : TokenType::LESS);
        break;
    case '>':
        addToken(match('=') ? TokenType::GREATER_EQUAL : TokenType::EQUAL);
        break;
    case '/':
    {
        if(match('/'))
        {
            while(peek() != '\n' && !isAtEnd())
                advance();
        }
        else
        {
            addToken(TokenType::SLASH);
        }
    }

    default:
        addError("Unexpected character.");
        break;
    }
}

char Scanner::advance()
{
    ++current_;
    return source_.at(current_ - 1);
}

bool Scanner::match(char expected)
{
    if(isAtEnd())
        return false;
    if(source_.at(current_) != expected)
        return false;

    ++current_;
    return true;
}
