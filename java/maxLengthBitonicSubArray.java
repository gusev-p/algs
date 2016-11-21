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
		    int maxLen = maxBitonicSubArrayLength(array);
		    System.out.println(maxLen);
		}
	}
	
	private static int max(int a, int b) {
	    return a > b ? a : b;
	}
	
	private static int maxBitonicSubArrayLength(int[] array) {
	    int i = 1;
	    int maxLen = 0;
	    while(i < array.length) {
	        int start = i - 1;
	        if(array[i] == array[i - 1]) {
	            while(i < array.length && array[i] == array[i - 1]) {
	                i++;
	            }
	            continue;
	        }
	        if(array[i] < array[i - 1]) {
	           while(i < array.length && array[i] < array[i - 1]) {
	                i++;
	           }
	           maxLen = max(maxLen, i - start);
	           continue;
	        }
	        while(i < array.length && array[i] > array[i - 1]) {
	            i++;
	        }
	        while(i < array.length && array[i] < array[i - 1]) {
                	i++;
            	}
            	maxLen = max(maxLen, i - start);
	    }
		return maxLen;
	}
}