#include <iostream>
#include <string>
#include <cmath>
#include <vector>

using namespace std;

namespace {
    int main() {
        string s;
        cin >> s;
        auto lower = static_cast<long long>(floor(sqrt(s.size())));
        auto upper = static_cast<long long>(ceil(sqrt(s.size())));
        vector<pair<long long, long long>> cases;
        cases.emplace_back(make_pair(lower, lower));
        cases.emplace_back(make_pair(lower, upper));
        cases.emplace_back(make_pair(upper, upper));
        int min = -1;
        long long minArea = 0;
        for (int i = 0; i < cases.size(); ++i) {
            const long long area = cases[i].first * cases[i].second;
            if (area >= s.size() && (min == -1 || area < minArea)) {
                min = i;
                minArea = area;
            }
        }
        const long long rows = cases[min].first;
        const long long columns = cases[min].second;
        for (int j = 0; j < columns; ++j) {
            for (int i = 0; i < rows; ++i) {
                const long long index = i * columns + j;
                if (index < s.size()) {
                    cout << s[index];
                }
            }
            cout << " ";
        }
        cout << endl;
        return 0;
    }
}
