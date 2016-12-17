#include <iostream>

using namespace std;

int main() {
	int n;
	cin >> n;
	int q;
	cin >> q;
	int** top_array = new int*[n];
	for(int i = 0; i < n; i++) {
		int k;
		cin >> k;
		int* nested_array = new int[k];
		for(int j = 0; j < k; j++) {
			int v;
			cin >> v;
			nested_array[j] = v;
		}
		top_array[i] = nested_array;
	}	
	for(int l = 0; l < q; l++) {
		int i, j;
		cin >> i;
		cin >> j;
		cout << top_array[i][j] << endl;
	}
	for(int i = 0; i < n; i++)
		delete[] top_array[i];
	delete[] top_array;
	return 0;
}