#include <stdio.h>
#include <Windows.h>
#include <filesystem>
#include <stdio.h>
#include <string>
#include <fstream>
#include <vector>

using namespace std;

void run_file(char *file_path);
void run_prompt();

int main(int argc, char **argv)
{
    if (argc < 1)
    {
        printf("Usage: cpplox [script]");
        return -1;
    }
    if (argc == 1)
        run_file(argv[0]);
    else
        run_prompt();
    return 0;
}

void run_file(char *file_path)
{
    if (filesystem::exists(file_path))
    {
        ifstream ifs(file_path, ios::binary | ios::ate);

        ifstream::pos_type pos = ifs.tellg();
        vector<char> result(pos);

        ifs.seekg(0, ios::beg);
        ifs.read(result.data(), pos);
    }
}

void run_prompt()
{
}