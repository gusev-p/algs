#include <iostream>
#include <unordered_set>

using namespace std;

struct Position {
    int Row;
    int Column;

    bool OutOfBoard(int size) const {
        return Row == 0 || Row == size + 1 ||
               Column == 0 || Column == size + 1;
    }

    bool operator==(const Position &other) const {
        return Row == other.Row && Column == other.Column;
    }

    Position &operator +=(const Position &other) {
        Row += other.Row;
        Column += other.Column;
        return *this;
    }
};

namespace std {
    template <>
    struct hash<Position> {
        size_t operator()(const Position &key) const {
            size_t result = 17;
            result = result * 31 + hash<int>()(key.Row);
            result = result * 31 + hash<int>()(key.Column);
            return result;
        }
    };
}

const Position Shifts[] {
    {1, 0},
    {1, 1},
    {0, 1},
    {-1, 1},
    {-1, 0},
    {-1, -1},
    {0, -1},
    {1, -1},
};

int main() {
    int n, k;
    cin >> n >> k;
    Position queenPosition{};
    cin >> queenPosition.Row >> queenPosition.Column;
    unordered_set<Position> obstacles;
    for (size_t i = 0; i < k; ++i) {
        int r, c;
        cin >> r >> c;
        obstacles.insert({r, c});
    }
    unsigned long long result = 0;
    for (const Position &shift : Shifts) {
        Position currentPosition = queenPosition;
        while(true) {
            currentPosition += shift;
            if (currentPosition.OutOfBoard(n) || obstacles.find(currentPosition) != obstacles.end()) {
                break;
            }
            ++result;
        }
    }
    cout << result << endl;
    return 0;
}
