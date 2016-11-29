import java.util.*;
import java.lang.*;
import java.io.*;

class GFG {
	public static void main (String[] args) {
		Scanner scanner = new Scanner(System.in);
		int t = scanner.nextInt();
		for(int i = 0; i < t; i++) {
			long n = scanner.nextLong();
			String shortened = toBase62(n);
			System.out.println(shortened);
			long source = fromBase62(shortened);
			System.out.println(source);
		}
	}

	private static String toBase62(long num) {
		StringBuilder resultBuilder = new StringBuilder();
		while(num > 0) {
			int d = (int)(num % 62);
			resultBuilder.append(digits62.charAt(d));
			num /= 62;
		}
		return resultBuilder.reverse().toString();		
	}

	private static long fromBase62(String num) {
		long result = 0;
		for(int i = 0; i < num.length(); i++) {
			result = result * 62 + digits62.indexOf(num.charAt(i));		
		}
		return result;
	}

	private static final String digits62 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
}