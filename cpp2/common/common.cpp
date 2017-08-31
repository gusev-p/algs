#include "common.h"
#include <vector>

using namespace std;

int lcs(const std::string &s1, const std::string &s2) {
    vector<int> results(s1.size() * s2.size());
    int s1Index = 0;
    int s2Index = 0;
    for (size_t i = 0; i < results.size(); ++i) {
        if (s1[s1Index] == s2[s2Index]) {
            results[i] = (s1Index > 0 && s2Index > 0 ? results[i - s1.size() - 1] : 0) + 1;
        } else {
            results[i] = max(s1Index > 0 ? results[i - 1] : 0,
                             s2Index > 0 ? results[i - s1.size()] : 0);
        }
        ++s1Index;
        if (s1Index == s1.size()) {
            s1Index = 0;
            ++s2Index;
        }
    }
    return results.back();
}
