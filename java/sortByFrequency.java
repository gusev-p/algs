import java.util.*;
import java.lang.*;
import java.io.*;

class GFG {
	public static void main (String[] args) {
		Scanner scanner = new Scanner(System.in);
		int t = scanner.nextInt();
		for(int i = 0; i < t; i++) {
			int n = scanner.nextInt();
			int[] array = new int[n];
			for(int j = 0; j < n; j++) {
				array[j] = scanner.nextInt();
			}
			sortByFrequency(array);
			printArray(array);
		}
	}

	private static void sortByFrequency(int[] array) {
		final int[] counts = new int[60];
		for(int i = 0; i < array.length; i++) {
			counts[array[i] - 1]++;
		}
		final Integer[] sortedView = new Integer[counts.length];
		for(int i = 0; i < counts.length; i++) {
			sortedView[i] = i;
		}
		Arrays.sort(sortedView, new Comparator<Integer>() {
			public int compare(Integer a, Integer b) {
				return Integer.compare(counts[b], counts[a]);
			}
		});
		int index = 0;
		for(int i = 0; i < sortedView.length; i++) {
			int sourceValue = sortedView[i];
			int count = counts[sourceValue];
			for(int j = 0; j < count; j++) {
				array[index++] = sourceValue + 1;
			}
		}
	}

	private static void printArray(int[] array) {
		for(int i = 0; i < array.length; i++) {
			if(i != 0) {
				System.out.print(" ");
			}
			System.out.print(array[i]);
		}
		System.out.println("");
	}
}