namespace Advent2023;

public class Schematics
{
    public List<int> PartNumbers { get; }
    public Dictionary<(int, int), List<int>> Gears { get; }
    readonly string[] lines;
    readonly int nCols;
    readonly int nRows;
    public Schematics(string filename)
    {
        PartNumbers = [];
        Gears = [];
        lines = [.. File.ReadAllLines(filename)];
        nRows = lines.Length;
        nCols = lines[0].Length;
        for (int row = 0; row < nRows; row++)
        {
            int number = 0;
            int numberLen = 0;
            for (int col = 0; col < nCols; col++)
            {
                char c = lines[row][col];
                if (c >= '0' && c <= '9')
                {
                    number *= 10;
                    number += c - '0';
                    numberLen++;
                }
                else
                {
                    if (numberLen > 0 && Neighbours(row, col - numberLen, numberLen, number).Any(IsSymbol))
                    {
                        PartNumbers.Add(number);
                    }
                    number = 0;
                    numberLen = 0;
                }
            }
            if (numberLen > 0)
            {
                if (Neighbours(row, nCols - numberLen, numberLen, number).Any(IsSymbol))
                {
                    PartNumbers.Add(number);
                }
                number = 0;
                numberLen = 0;
            }
        }
    }
    private static bool IsSymbol(char c) => c != '.' && (c < '0' || c > '9');

    private void AddGear(int row, int col, int number)
    {
        if (Gears.TryGetValue((row, col), out List<int>? value))
        {
            value.Add(number);
        }
        else
        {
            Gears.Add((row, col), [number]);
        }
    }
    private string Neighbours(int row, int startcol, int len, int number)
    {
        string res = "";
        int start = Math.Max(0, startcol - 1);
        int end = Math.Min(nCols - 1, startcol + len);
        if (row > 0)
        {
            for (int col = start; col <= end; col++)
            {
                char c = lines[row - 1][col];
                res += c;
                if (c == '*')
                {
                    AddGear(row - 1, col, number);
                }
            }
        }
        if (startcol > 0)
        {
            char c = lines[row][start];
            res += c;
            if (c == '*')
            {
                AddGear(row, start, number);
            }
        }
        if (end < nCols - 1)
        {
            char c = lines[row][end];
            res += c;
            if (c == '*')
            {
                AddGear(row, end, number);
            }
        }
        if (row < nRows - 1)
        {
            for (int col = start; col <= end; col++)
            {
                char c = lines[row + 1][col];
                res += c;
                if (c == '*')
                {
                    AddGear(row + 1, col, number);
                }
            }
        }
        return res;
    }
}

public static class Day3GearRatios
{
    public static int SumOfPartNumbers(string filename)
    {
        return new Schematics(filename).PartNumbers.Sum();
    }
    public static int SumOfGearRatios(string filename)
    {
        return (from gear in new Schematics(filename).Gears
                where gear.Value.Count == 2
                select gear.Value[0] * gear.Value[1]).Sum();
    }
}
