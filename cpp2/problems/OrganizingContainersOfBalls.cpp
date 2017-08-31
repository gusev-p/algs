#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

namespace {
    int main() {
        int q;
        cin >> q;
        for (int t = 0; t < q; ++t) {
            size_t n;
            cin >> n;
            vector<long long> byContainers(n);
            vector<long long> byTypes(n);
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n; ++j) {
                    long long v;
                    cin >> v;
                    byContainers[i] += v;
                    byTypes[j] += v;
                }
            }
            sort(byContainers.begin(), byContainers.end());
            sort(byTypes.begin(), byTypes.end());
            const bool possible = equal(byContainers.begin(), byContainers.end(), byTypes.begin());
            cout << (possible ? "Possible" : "Impossible") << endl;
        }
        return 0;
    }
}