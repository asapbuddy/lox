#include <filesystem>
#include <fstream>
#include <iostream>
#include <string>

#include <vector>
#include "Core/Scanner.hpp"
#include "Core/Token.hpp"

using namespace std;

void run_file(char* file_path);
void run_prompt();
void run(const string&);

int main(int argc, char** argv)
{
    if(argc < 1)
    {
        cout << "Usage: cpplox [script]" << endl;
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
        const istreambuf_iterator<char> ifs_begin(ifs);
        const string result(ifs_begin, istreambuf_iterator<char>());
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

void run(const string& source)
{
    Scanner scanner(source);
    vector<Token> tokens = scanner.scan_tokens();
    auto errors = scanner.get_errors();

    if(!errors.empty())
        std::for_each(errors.begin(), errors.end(), [](auto& err) { err->what(cerr); });

    std::for_each(tokens.begin(), tokens.end(), [](auto& token) { cout << token << endl; });
}
