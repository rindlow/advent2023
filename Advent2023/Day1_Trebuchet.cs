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
    private static int CalibrationValueWithText(string line) {
        int first = -1;
        int last = 0;
        List<string> digits = new() {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        foreach (int i in Enumerable.Range(0, line.Length)) {
            if (line[i] >= '0' && line[i] <= '9') {
                    if (first < 0) {
                    first = line[i] - '0';
                }
                last = line[i] - '0';
            } else {
                foreach (string digit in digits)
                    if (line.Substring(i).StartsWith(digit)) {
                        int number = digits.IndexOf(digit) + 1;
                        if (first < 0) {
                            first = number;
                        }
                        last = number;
                    }
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
