namespace Advent2023;
using System.Linq;

public static class Day1_Trebuchet
{
    private static int CalibrationValue(string line) {
        int first = -1;
        int last = 0;
        foreach (char c in line) {
            if (c >= '0' && c <= '9') {
                if (first < 0) {
                    first = c - '0';
                }
                last = c - '0';
            }
        }
        return 10 * first + last;
    }
    private static int DigitAtIndex(int index, string line) {
        List<string> digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        if (line[index] >= '0' && line[index] <= '9') {
            return line[index] - '0';               
        } else {
            foreach (string digit in digits) {
                if (line[index..].StartsWith(digit)) {
                    return digits.IndexOf(digit) + 1;
                }
            }
        }
        return -1;
    }
    private static int CalibrationValueWithText(string line) {
        int first = -1;
        int last = 0;
        foreach (int i in Enumerable.Range(0, line.Length)) {
            int digit;
            if ((digit = DigitAtIndex(i, line)) >= 0) {
                if (first < 0) {
                    first = digit;
                }
                last = digit;
            }    
        }
        return 10 * first + last;
    }
    public static int SumCalibrationValues(string filename) {
        return File.ReadAllLines(filename).Select(CalibrationValue).Sum();
    } 
    public static int SumCalibrationValuesWithText(string filename) {
        return File.ReadAllLines(filename).Select(CalibrationValueWithText).Sum();
    }
}
