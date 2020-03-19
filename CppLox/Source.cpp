#include <filesystem>
#include <fstream>
#include <iostream>
#include <string>
#include <vector>
#include "Core/Scanner.h"
#include "Core/Token.h"

using namespace std;

void run_file(char* file_path);
void run_prompt();
void run(string);
void error(int line, string message);
void report(int line, string where, string message);

bool hadError = false;

int main(int argc, char** argv)
{
    if(argc < 1)
    {
        printf("Usage: cpplox [script]");
        return -1;
    }
    if(argc == 2)
        run_file(argv[1]);
    else
        run_prompt();
    return 0;
}

void run_file(char* file_path)
{
    if(std::filesystem::exists(file_path))
    {
        ifstream ifs(file_path);
        string result((istreambuf_iterator<char>(ifs)) //TODO need to detailed explanation about (istreambuf())
                      ,
                      istreambuf_iterator<char>());
        run(result);
    }
}

void run_prompt()
{
    string content;
    while(true)
    {
        cout << "> ";
        getline(cin, content);
        run(content);
    }
}

void run(string source)
{
    Scanner scanner(source);
    vector<Token> tokens = scanner.scan_tokens();
    for(auto token : tokens)
        cout << token << endl;
}

void error(int line, string message)
{
    report(line, "", message);
}

void report(int line, string where, string message)
{
    cerr << "[line " << line << "] Error" + where + ": " + message << endl;
    hadError = true;
}
