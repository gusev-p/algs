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
			String largestNumber = largestNumber(array);
			System.out.println(largestNumber);			
		}
	}
		
	private static String largestNumber(int[] array) {
		String[] strings = new String[array.length];
		for(int i = 0; i < array.length; i++) {
			strings[i] = String.valueOf(array[i]);
		}
		Arrays.sort(strings, new Comparator<String>() {
			public int compare(String a, String b) {
				return b.compareTo(a);				
			}
		});
		StringBuilder result = new StringBuilder();
		for(int i = 0; i < strings.length; i++) {
			result.append(strings[i]);
		}
		return result.toString();
	}
}