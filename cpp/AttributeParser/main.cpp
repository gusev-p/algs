#include <iostream>
#include <deque>
#include <unordered_map>
#include <string>
#include <sstream>
#include <fstream>

using namespace std;

using AttributesStore = unordered_map<string, string>;

class AttributesBuilder {
public:
    void ParseLine(const string& line) {
        if (line[1] == '/') {
            openTags_.pop_back();
            return;
        }
        unsigned long pos = 1;
        openTags_.emplace_back(NextKey(line, pos, " >"));
        unsigned long len = line.length();
        while(true) {
            SkipSpaces(line, pos);
            if(line[pos] == '>') {
                break;
            }
            string name = NextKey(line, pos, " =");
            pos = line.find('"', pos);
            unsigned long nextPos = line.find('"', pos + 1);
            string value = line.substr(pos + 1, nextPos - pos - 1);
            pos = nextPos + 1;
            string key;
            for (const string& k: openTags_) {
                key += '.' + k;
            }
            key.erase(key.begin());
            key += "~" + name;
            attributes_.insert({key, value});
        }
    }

    bool AnyOf(char c, const char* term) {
        for(; *term != '\0'; ++term) {
            if(*term == c) {
                return true;
            }
        }
        return false;
    }

    string NextKey(const string& s, unsigned long& pos, const char* term) {
        unsigned long start = pos;
        while(!AnyOf(s[pos], term)) {
            ++pos;
        }
        return s.substr(start, pos - start);
    }

    void SkipSpaces(const string& s, unsigned long& pos) {
        unsigned long start = pos;
        while(s[pos] == ' ') {
            ++pos;
        }
    }

    const AttributesStore GetAttributes() const {
        return attributes_;
    }

private:

    deque<string> openTags_;
    AttributesStore attributes_;
};

class RedirectIO {
public:

    RedirectIO()
        : input_("input"),
          output_("output")
    {
        oldInput_ = cin.rdbuf();
        cin.rdbuf(input_.rdbuf());

        oldOutput_ = cout.rdbuf();
        cout.rdbuf(output_.rdbuf());
    }

    ~RedirectIO() {
        cin.rdbuf(oldInput_);
        cout.rdbuf(oldOutput_);
    }

private:
    ifstream input_;
    ofstream output_;
    streambuf* oldInput_;
    streambuf *oldOutput_;
};

int main() {
    RedirectIO redirectIO;

    int linesCount;
    int queriesCount;
    cin >> linesCount;
    cin >> queriesCount;
    string _;
    getline(cin, _);

    AttributesStore attributes;
    {
        AttributesBuilder builder;
        for(int i = 0; i < linesCount; i++) {
            string line;
            getline(cin, line);
            builder.ParseLine(line);
        }
        attributes = builder.GetAttributes();
    }

    for(int i = 0; i < queriesCount; i++) {
        string query;
        getline(cin, query);
        auto it = attributes.find(query);
        if(it == attributes.end()) {
            cout << "Not Found!" << endl;
        } else {
            cout << it->second << endl;
        }
    }

    return 0;
}