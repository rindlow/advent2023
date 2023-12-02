namespace Advent2023;
using System.Linq;
using System.Text.RegularExpressions;

public static class Day1_Trebuchet
{
    private static readonly Regex pattern = new("^[0-9]|one|t(wo|hree)|f(our|ive)|s(ix|even)|eight|nine");
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
    private static int Digit(string line) {
        List<string> digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        if (line[0] >= '0' && line[0] <= '9') {
            return line[0] - '0';               
        } else {
            foreach (string digit in digits) {
                if (line.StartsWith(digit)) {
                    return digits.IndexOf(digit) + 1;
                }
            }
        }
        return -1;
    }
    private static int LastDigit(string line) {
        List<string> digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        char c = line.Last();
        if (c >= '0' && c <= '9') {
            return c - '0';               
        } else {
            foreach (string digit in digits) {
                if (line.EndsWith(digit)) {
                    return digits.IndexOf(digit) + 1;
                }
            }
        }
        return -1;
    }
    private static int DigitRegex(string line) {
        // string pattern = "^[0-9]|one|t(wo|hree)|f(our|ive)|s(ix|even)|eight|nine";
        Match match = pattern.Match(line);
        if (match.Success) {
            return match.Value switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => Int32.Parse(match.Value)
            };
        }
        return -1;
    }
    private static int DigitNestedIf(string line) {
        if (line[0] >= '0' && line[0] <= '9') {
            return line[0] - '0';
        } else if (line.Length >= 3 && line[0] == 'o' && line[1] == 'n' && line[2] == 'e') {
            return 1;
        } else if (line.Length >= 3 && line[0] == 't') {
            if (line[1] == 'w' && line[2] == 'o') {
                return 2;
            } else if (line.Length >= 5 && line[1] == 'h' && line[2] == 'r' && line[3] == 'e' && line[4] == 'e') {
                return 3;
            }
        } else if (line.Length >= 4 && line[0] == 'f') {
            if (line[1] == 'o' && line[2] == 'u' && line[3] == 'r') {
                return 4;
            } else if (line[1] == 'i' && line[2] == 'v' && line[3] == 'e') {
                return 5;
            }
        } else if (line.Length >= 3 && line[0] == 's') {
            if (line[1] == 'i' && line[2] == 'x') {
                return 6;
            } else if (line.Length >= 5 && line[1] == 'e' && line[2] == 'v' && line[3] == 'e' && line[4] == 'n') {
                return 7;
            }
        } else if (line.Length >= 5 && line[0] == 'e' && line[1] == 'i' && line[2] == 'g' && line[3] == 'h' && line[4] == 't') {
            return 8;
        } else if (line.Length >= 4 && line[0] == 'n' && line[1] == 'i' && line[2] == 'n' && line[3] == 'e') {
            return 9;
        } 
        return -1;
    }
    private static int CalibrationValueWithText(string line) {
        int first = -1;
        int last = 0;
        foreach (int i in Enumerable.Range(0, line.Length)) {
            int digit;
            if ((digit = DigitNestedIf(line[i..])) >= 0) {
                if (first < 0) {
                    first = digit;
                }
                last = digit;
            }    
        }
        return 10 * first + last;
    }
    private static int CalibrationValueWithText2(string line) {
        int first = 0;
        int last = 0;
        foreach (int i in Enumerable.Range(0, line.Length)) {
            int digit = Digit(line[i..]);
            if (digit >= 0) {
                first = digit;
                break;
            }
        }
        foreach (int i in Enumerable.Range(0, line.Length + 1).Reverse()) {
            int digit = LastDigit(line[..i]);
            if (digit >= 0) {
                last = digit;
                break;
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
