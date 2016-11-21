import java.util.*;
import java.lang.*;
import java.io.*;

class GFG {
	private static final Random random = new Random();

	public static void main (String[] args) {
		Scanner scanner = new Scanner(System.in);
		int t = scanner.nextInt();
		for(int i = 0; i < t; i++) {
			int n = scanner.nextInt();
			int[] array = new int[n];
			for(int j = 0; j < n; j++) {
				array[j] = scanner.nextInt();
			}
			int k = scanner.nextInt();
			int kThSmallest = quickSelect(array, 0, n - 1, k);
		    	System.out.println(kThSmallest);
		}
	}

	private static int quickSelect(int[] array, int left, int right, int k) {
		if(left == right) {
			return array[left];
		}
		int pivotIndex = left + random.nextInt(right - left + 1);
		swap(array, left, pivotIndex);
		int mid = partition(array, left, right);
		int order = mid - left + 1;
		if(k == order) {
			return array[mid];
		}
		else if(k < order) {
			return quickSelect(array, left, mid - 1, k);
		}
		else {
			return quickSelect(array, mid + 1, right, k - order);
		}
	}

	private static int partition(int[] array, int left, int right) {
		int pivot = array[left];
		int i = left + 1;
		for(int j = left + 1; j <= right; j++) {
			if(array[j] < pivot) {
				swap(array, i, j);
				i++;
			}
		}
		swap(array, left, i - 1);
		return i - 1;		
	}

	private static void swap(int[] array, int i, int j) {
		int t = array[i];
		array[i] = array[j];
		array[j] = t;
	}
}