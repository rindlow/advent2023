using System.ComponentModel.Design.Serialization;
using System.Reflection.Metadata.Ecma335;

namespace Advent2023;

public static class Day3_GearRatios
{
    private static bool IsSymbol(char c) => c != '.' && (c < '0' || c > '9');
    private static bool FindSymbol(string[] lines, int row, int col, int numberLen) {
        if (row > 0) {
            if (lines[row - 1][Math.Max(0, col - numberLen - 1)..Math.Min(lines[row - 1].Length, col + 1)]
                .Any(IsSymbol)) {
                return true;
            }
        }
        if (col - numberLen > 0 && IsSymbol(lines[row][col - numberLen - 1])) {
            return true;
        }
        if (col < lines[row].Length && IsSymbol(lines[row][col])) {
            return true;
        }
        if (row < lines.Length - 1) {
            if (lines[row + 1][Math.Max(0, col - numberLen - 1)..Math.Min(lines[row + 1].Length, col + 1)]
                .Any(IsSymbol)) {
                return true;
            }
        }
        return false;
    }
    public static int SumOfPartNumbers(string filename) {
        string[] lines = [.. File.ReadAllLines(filename)];
        int sum = 0;
        for (int row = 0; row < lines.Length; row++) {
            int number = 0;
            int numberLen = 0;
            for (int col = 0; col < lines[row].Length; col++) {
                char c = lines[row][col];
                if (c >= '0' && c <= '9') {
                    number *= 10;
                    number += c - '0';
                    numberLen++;
                } else if (numberLen > 0 && FindSymbol(lines, row, col, numberLen)) {
                    sum += number;
                    number = 0;
                    numberLen = 0;
                } else {
                    number = 0;
                    numberLen = 0;
                }
            }
            if (numberLen > 0 && FindSymbol(lines, row, lines[row].Length, numberLen)) {
                sum += number;
                number = 0;
                numberLen = 0;
            }
        }
        return sum;
    }
    private static (int, int) FindGear(string[] lines, int row, int col, int numberLen) {
        if (row > 0) {
            for (int i = Math.Max(0, col - numberLen - 1); i < Math.Min(lines[row - 1].Length, col + 1); i++) {
                if (lines[row - 1][i] == '*') {
                    return (row - 1, i);
                }
            }
        }
        if (col - numberLen > 0 && lines[row][col - numberLen - 1] == '*') {
            return (row, col - numberLen - 1);
        }
        if (col < lines[row].Length && lines[row][col] == '*') {
            return (row, col);
        }
        if (row < lines.Length - 1) {
            for (int i = Math.Max(0, col - numberLen - 1); i < Math.Min(lines[row + 1].Length, col + 1); i++) {
                if (lines[row + 1][i] == '*') {
                    return (row + 1, i);
                }
            }
        }
        return (-1, -1);
    }
    public static int SumOfGearRatios(string filename) {
        string[] lines = [.. File.ReadAllLines(filename)];
        Dictionary<(int, int), List<int>> gears = [];
        for (int row = 0; row < lines.Length; row++) {
            int number = 0;
            int numberLen = 0;
            for (int col = 0; col < lines[row].Length; col++) {
                char c = lines[row][col];
                if (c >= '0' && c <= '9') {
                    number *= 10;
                    number += c - '0';
                    numberLen++;
                } else if (numberLen > 0) {
                    var pos = FindGear(lines, row, col, numberLen);
                    if (pos.Item1 >= 0) {
                        if (gears.TryGetValue(pos, out List<int>? value)) {
                            value.Add(number);
                        } else {
                            gears.Add(pos, [number]);
                        }
                    }
                    number = 0;
                    numberLen = 0;
                } else {
                    number = 0;
                    numberLen = 0;
                }
            }
            if (numberLen > 0) {
                int col = lines[row].Length;
                var pos = FindGear(lines, row, col, numberLen);
                if (pos.Item1 >= 0) {
                    if (gears.TryGetValue(pos, out List<int>? value)) {
                        value.Add(number);
                    } else {
                        gears.Add(pos, [number]);
                    }
                    number = 0;
                    numberLen = 0;
                }
            }
        }
        return (from gear in gears where gear.Value.Count == 2 select gear.Value[0] * gear.Value[1]).Sum();
    }
}
