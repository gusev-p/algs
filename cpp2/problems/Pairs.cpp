#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

class UniqueIterator {
public:
    explicit UniqueIterator(const vector<int> &v)
        : Vector(v)
    {
        Move();
    }

    int GetValue() const {
        return Vector[Position];
    }

    int GetCount() const {
        return Count;
    }

    bool HasValue() const {
        return Position < Vector.size();
    }

    bool Before(const UniqueIterator &other) const {
        return Position < other.Position;
    }

    void MoveAfter(const UniqueIterator &other) {
        Position = other.Position;
        Move();
    }

    void Move() {
        Count = 0;
        while(true) {
            ++Position;
            ++Count;
            if (Position == Vector.size()) {
                break;
            }
            if (Position + 1 == Vector.size() || Vector[Position + 1] != Vector[Position]) {
                break;
            }
        }
    }

private:
    const vector<int> &Vector;
    int Position = -1;
    int Count = 0;
};

int main() {
    int n, k;
    cin >> n >> k;
    vector<int> numbers(static_cast<size_t>(n));
    for (size_t i = 0; i < n; ++i) {
        cin >> numbers[i];
    }
    sort(numbers.begin(), numbers.end());
    int result = 0;
    UniqueIterator left(numbers);
    UniqueIterator right(numbers);
    while(right.HasValue()) {
        if (!left.Before(right)) {
            right.MoveAfter(left);
            continue;
        }
        const int diff = right.GetValue() - left.GetValue();
        if (diff < k) {
            right.Move();
            continue;
        }
        if (diff == k) {
            result += left.GetCount() * right.GetCount();
        }
        left.Move();
    }
    cout << result << endl;
    return 0;
}
