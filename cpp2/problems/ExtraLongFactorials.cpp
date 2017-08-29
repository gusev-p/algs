#include <iostream>
#include <vector>

using namespace std;

class TBigInt {
public:
    explicit TBigInt(int v) {
        while(v > 0) {
            Digits.push_back(static_cast<char>(v % 10));
            v /= 10;
        }
    }

    friend ostream &operator<<(ostream &s, const TBigInt &v) {
        for (auto it = v.Digits.rbegin(); it != v.Digits.rend(); ++it) {
            const char d = '0' + *it;
            s << d;
        }
        return s;
    }

    friend TBigInt operator*(const TBigInt &a, const TBigInt &b) {
        if (a.Digits.empty() || b.Digits.empty()) {
            return {};
        }
        vector<char> temp(a.Digits.size() + 1);
        TBigInt result;
        result.Digits.resize(a.Digits.size() + b.Digits.size());
        for (int i = 0; i < b.Digits.size(); ++i) {
            for (char &d : temp) {
                d = 0;
            }
            char borrow = 0;
            for (int j = 0; j < a.Digits.size(); ++j) {
                char v = a.Digits[j] * b.Digits[i] + borrow;
                temp[j] = static_cast<char>(v % 10);
                borrow = static_cast<char>(v / 10);
            }
            temp.back() = borrow;
            borrow = 0;
            for (int j = 0; j < temp.size(); ++j) {
                result.Digits[i + j] += temp[j] + borrow;
                if (result.Digits[i + j] >= 10) {
                    result.Digits[i + j] -= 10;
                    borrow = 1;
                } else {
                    borrow = 0;
                }
            }
            if (borrow > 0) {
                throw logic_error("invalid borrow");
            }
        }
        size_t nonZeroSize = result.Digits.size();
        while(nonZeroSize > 1 && result.Digits[nonZeroSize - 1] == 0) {
            --nonZeroSize;
        }
        result.Digits.resize(nonZeroSize);
        return result;
    }

private:
    TBigInt() = default;

    vector<char> Digits;
};

TBigInt Factorial(int v) {
    TBigInt result(1);
    for (int i = 2; i <= v; ++i) {
        result = result * TBigInt(i);
    }
    return result;
}

namespace {
    int main() {
        int v;
        cin >> v;
        cout << Factorial(v) << endl;
        return 0;
    }
}
