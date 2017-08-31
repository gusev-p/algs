#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

namespace {
    int main() {
        int n, k;
        cin >> n >> k;
        vector<int> locations(static_cast<size_t>(n));
        for (size_t i = 0; i < n; ++i) {
            cin >> locations[i];
        }
        sort(locations.begin(), locations.end());
        int transmittersCount = 0;
        int zoneStart = 0;
        while(zoneStart < locations.size()) {
            int transmitterPosition = zoneStart;
            while(transmitterPosition + 1 < locations.size() && locations[transmitterPosition + 1] - locations[zoneStart] <= k) {
                ++transmitterPosition;
            }
            ++transmittersCount;
            int zoneFinish = transmitterPosition;
            while(zoneFinish + 1 < locations.size() && locations[zoneFinish + 1] - locations[transmitterPosition] <= k) {
                ++zoneFinish;
            }
            zoneStart = zoneFinish + 1;
        }
        cout << transmittersCount << endl;
        return 0;
    }
}