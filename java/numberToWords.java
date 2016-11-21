import java.util.*;
import java.lang.*;
import java.io.*;

class GFG {
	public static void main (String[] args) {
		Scanner scanner = new Scanner(System.in);
		int t = scanner.nextInt();
		for(int i = 0; i < t; i++) {
		    int num = scanner.nextInt();
		    String wordedNumber = numberToWords(num);
		    System.out.println(wordedNumber);
		}
	}
	
	private static String numberToWords(int num) {
		if(num == 0) {
			return "zero";
		}
		String result = "";
		int thousands = num / 1000;
		if(thousands > 0) {
			result += digits[thousands] + " thousand";
		}
		num = num % 1000;
		int hundreds = num / 100;
		if(hundreds > 0) {
			result += " " + digits[hundreds] + " hundred";
		}
		int d = num % 100;
		if(d > 0) {
			if(d < 10)
				result += " and " + digits[d];
			else if(d < 20)
				result += " and " + digits2[d - 10];
			else {
				result += " and " + digits3[d / 10 - 2];
				int m = d % 10;
				if(m > 0) {
					result += " " + digits[m];
				}
			}
		}
		if(result.startsWith(" and ")) {
			result = result.substring(" and ".length());
		}
		else if(result.startsWith(" ")) {
			result = result.substring(1);
		}
		return result;
	}

	private static String[] digits = {"", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

	private static String[] digits2 = {"ten", "eleven", "twelve", "thirteen", "fourteen", "fivteen", "sixteen", "seventeen", "eighteen", "nineteen"};

	private static String[] digits3 = {"twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};
}