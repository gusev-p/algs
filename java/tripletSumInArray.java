import java.util.*;
import java.lang.*;
import java.io.*;

class GFG {
	public static void main (String[] args) {
		Scanner scanner = new Scanner(System.in);
		int t = scanner.nextInt();
		for(int i = 0; i < t; i++) {
			int n = scanner.nextInt();
			int x = scanner.nextInt();
			int[] array = new int[n];
			for(int j = 0; j < n; j++)
				array[j] = scanner.nextInt();
			boolean hasTriplet = hasTriplet(array, x);
			System.out.println(hasTriplet ? 1 : 0);
		}
	}
	
	private static boolean hasTriplet(int[] array, int x) {
		Arrays.sort(array);
		for(int i = 0; i < array.length; i++) {
			int s = x - array[i];
			int left = 0;
			int right = array.length - 1;
			while(left < right) {
				if(left == i) {
					left++;
					continue;
				}
				if(right == i) {
					right--;
					continue;
				}
				int sum = array[left] + array[right];
				if(sum == s) {
					return true;
				}
				if(sum < s) {
					left++;
				}
				else {
					right--;
				}
			}
		}
		return false;
	}
}