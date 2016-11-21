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
		    for(int j = 0; j < n; j++)
		        array[j] = scanner.nextInt();
		    int[] reducedForm = makeReducedForm(array);
		    printArray(reducedForm);
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

	private static int[] makeReducedForm(final int[] array) {
		final Integer[] sortedView = new Integer[array.length];
		for(int i = 0; i < array.length; i++) {
			sortedView[i] = i;
		}
		Arrays.sort(sortedView, new Comparator<Integer>() {
			public int compare(Integer a, Integer b) {
				return Integer.compare(array[a], array[b]);
			}
		});
                final int[] result = new int[array.length];
		for(int i = 0; i < sortedView.length; i++) {
			result[sortedView[i]] = i;
		}
		return result;
	}
}