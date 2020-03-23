#include "Scanner.hpp"
#include "ScannerError.h"

std::vector<Token> Scanner::scan_tokens()
{
    while(!isAtEnd())
    {
        start_ = current_;
        scanToken();
    }
    string str;
    tokens_.emplace_back(TokenType::EOF, str, str, line_);
    return tokens_;
}

std::vector<IError*> Scanner::get_errors()
{
    return errors_;
}

bool Scanner::isAtEnd()
{
    return current_ >= source_.size();
}

void Scanner::addToken(TokenType type)
{
    string str;
    addToken(type, str);
}

void Scanner::addToken(TokenType type, string& literal)
{
    string text = source_.substr(start_, current_ - start_);
    tokens_.emplace_back(type, text, literal, line_);
}

void Scanner::addError(string text)
{
    //errors_.push_back(std::make_unique<ScannerError>(line_, text));
}

char Scanner::peek()
{
    return isAtEnd() ? '\0' : source_.at(current_);
}

void Scanner::add_string()
{
    while(peek() != '"' && !isAtEnd())
    {
        if(peek() == '\n')
            ++line_;
        advance();
    }

    // Unterminated string.
    if(isAtEnd())
    {
        addError("Unterminated string.");
        return;
    }

    //The closing ".
    advance();

    // Trim the surrounding quotes
    std::string value = source_.substr(start_ + 1, current_ - start_ - 2);
    addToken(TokenType::STRING, value);
}

char Scanner::peekNext()
{
    if(current_ + 1 >= source_.length())
        return '\0';
    return source_.at(current_ + 1);
}

void Scanner::add_number()
{
    while(isDigit(peek()))
        advance();

    // Look for a fractional part.
    if(peek() == '.' && isDigit(peekNext()))
    {
        // Consume the "."
        advance();
        while(isDigit(peek()))
            advance();
    }
    string literal = source_.substr(start_, current_);
    addToken(TokenType::NUMBER, literal);
}

bool Scanner::isDigit(char c)
{
    return c >= '0' && c <= '9';
}

bool Scanner::isAlphaNumeric(char c)
{
    return isAlpha(c) || isDigit(c);
}

void Scanner::add_identifier()
{
    while(isAlphaNumeric(peek()))
        advance();

    string text = source_.substr(start_, current_ - start_);
    const auto it = keywords_.find(text);
    TokenType type = TokenType::IDENTIFIER;
    if(it != keywords_.end())
        type = it->second;

    addToken(type);
}

bool Scanner::isAlpha(char c)
{
    return c >= 'a' && c <= 'z' ||
           c >= 'A' && c <= 'Z' ||
           c == '_';
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
    case 'o':
        if(peek() == 'r')
        {
            addToken(TokenType::OR);
        }
        break;
    case '/':
    {
        if(match('/'))
        {
            while(peek() != '\n' && !isAtEnd())
                advance();
        }
        else if(isAlpha(c))
        {
            add_identifier();
        }
        else
        {
            addToken(TokenType::SLASH);
        }
        break;
    }
    case '"':
        add_string();
        break;
    case ' ':
    case '\r':
    case '\t':
        // Ignore whitespace.
        break;
    case '\n':
        ++line_;
        break;
    default:
        if(isDigit(c))
        {
            add_number();
        }
        else if(isAlpha(c))
        {
            add_identifier();
        }
        else
        {
            addError("Unexpected character.");
            break;
        }
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
