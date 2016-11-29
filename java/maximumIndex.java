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
			int maxDiff = maxDiff(array);
			System.out.println(maxDiff);
		}
	}

	private static int maxDiff(int[] array) {
		int maxDiff = 0;
		int[] b = new int[101];
		for(int j = 0; j < b.length; j++) {
			b[j] = j >= array[0] ? 0 : -1;
		}		
		for(int i = 1; i < array.length; i++) {
			int v = array[i];
			for(int j = v; j < b.length && b[j] == -1; j++) {
				b[j] = i;
			}
			if(i - b[v] > maxDiff) {
				maxDiff = i - b[v];
			}
		}
		return maxDiff;
	}
}