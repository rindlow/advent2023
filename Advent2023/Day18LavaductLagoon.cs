namespace Advent2023;
enum BorderState
{
    Outside,
    Inside,
    BorderDown,
    BorderUp
}
sealed class Lagoon
{
    HashSet<Position> _dug = [];
    Position current = new Position(0, 0);
    public void Print()
    {
        int minRow = (from pos in _dug select pos.Row).Min();
        int maxRow = (from pos in _dug select pos.Row).Max();
        int minCol = (from pos in _dug select pos.Col).Min();
        int maxCol = (from pos in _dug select pos.Col).Max();
        foreach (int row in Enumerable.Range(minRow, maxRow - minRow + 1))
        {
            foreach (int col in Enumerable.Range(minCol, maxCol - minCol + 1))
            {
                if (_dug.Contains(new Position(row, col)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }
    public void Dig(char direction, int length)
    {
        switch (direction)
        {
            case 'R':
                foreach (int col in Enumerable.Range(current.Col + 1, length))
                {
                    current = new Position(current.Row, col);
                    _dug.Add(current);
                }
                break;
            case 'L':
                foreach (int col in Enumerable.Range(current.Col - length, length).Reverse())
                {
                    current = new Position(current.Row, col);
                    _dug.Add(current);
                }
                break;
            case 'D':
                foreach (int row in Enumerable.Range(current.Row + 1, length))
                {
                    current = new Position(row, current.Col);
                    _dug.Add(current);
                }
                break;
            case 'U':
                foreach (int row in Enumerable.Range(current.Row - length, length).Reverse())
                {
                    current = new Position(row, current.Col);
                    _dug.Add(current);
                }
                break;
        }
        Console.WriteLine($"{_dug.Count} dug, current = {current}");
    }
    public int Fill()
    {
        Print();
        var x = from pos in _dug
                group pos.Col by pos.Row;
        int fill = 0;
        foreach (var g in x)
        {
            Console.WriteLine($"row {String.Join(',', g.Order())}");
            int? last = null;
            bool inside = false;
            foreach (int col in g.Order())
            {
                if (last is int lastCol)
                {
                    if (lastCol != col - 1)
                    {
                        Console.WriteLine($"col = {col}, lastCol = {lastCol}, inside = {inside}");
                        inside = !inside;
                        if (inside)
                        {
                            fill += col - lastCol - 1;
                            Console.WriteLine($"fill += {col} - {lastCol} - 1 = {fill}");
                        }
                    }
                }
                last = col;
            }
            Console.WriteLine();
        }
        return _dug.Count + fill;
    }
}
public static class Day18LavaductLagoon
{
    public static int Volume(string filename)
    {
        Lagoon lagoon = new();
        foreach (string line in File.ReadAllLines(filename))
        {
            var split = line.Split(' ');
            int length = Int32.Parse(split[1]);
            lagoon.Dig(line[0], length);

        }
        return lagoon.Fill();
    }
}
