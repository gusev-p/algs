#include <iostream>
#include <common/common.h>

using namespace std;

int main() {
    int t;
    cin >> t;
    for (size_t i = 0; i < t; ++i) {
        int a,b;
        string s1,s2;
        cin >> a >> b;
        getline(cin, s1);
        getline(cin, s1);
        getline(cin, s2);
        cout << lcs(s1, s2) << endl;
    }
    return 0;
}