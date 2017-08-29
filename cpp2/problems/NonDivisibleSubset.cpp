#include <iostream>
#include <vector>

using namespace std;

namespace {
    int main() {
        int n, k;
        cin >> n >> k;
        vector<int> residuals(static_cast<size_t>(k));
        for (size_t i = 0; i < n; ++i) {
            int v;
            cin >> v;
            ++residuals[v % k];
        }
        int result = 0;
        for (size_t i = 0; i <= k / 2; ++i) {
            result += i == 0 || i * 2 == k
                      ? min(residuals[i], 1)
                      : max(residuals[i], residuals[k - i]);
        }
        cout << result << endl;
        return 0;
    }
}
